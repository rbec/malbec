using System;
using System.Collections.Generic;
using System.Linq;
using Malbec.Collections;
using Malbec.Collections.Generic;
using Malbec.Collections.Generic.Orderings;
using Malbec.Reactive.Expressions;
using Malbec.Reactive.Patches;
using Malbec.Logs;
using Malbec.Test.Graphs;

namespace Malbec.Test
{
  public static class MonteCarlo
  {
    public static readonly Random Random = new Random();

    public static bool Bool(double p) => Random.NextDouble() <= p;
    public static int Number(int max) => Random.Next(max);
    public static int Number(int min, int max) => Random.Next(min, max);
    public static IEnumerable<int> Unordered(int n = 10) => PermutationInternal(Enumerable.Range(0, n), Random.Next(0, n*5/4));
    public static IEnumerable<int> Unordered(int n, int k) => PermutationInternal(Enumerable.Range(0, n), k);
    public static IEnumerable<int> Ordered(int n = 10) => PermutationInternal(Enumerable.Range(0, n), Random.Next(0, n*5/4)).Order<int, Int32Order>();
    public static IEnumerable<int> Ordered(int n, int k) => PermutationInternal(Enumerable.Range(0, n), k).Order<int, Int32Order>();
    public static IEnumerable<int> Keys(int n) => Combination(Enumerable.Range(0, n), Random.Next(0, n + 1)).Order<int, Int32Order>();
    public static IEnumerable<int> Keys(int n, int k) => Combination(Enumerable.Range(0, n), k).Order<int, Int32Order>();
    public static IEnumerable<int> Intervals(int n = 100) => Combination(Enumerable.Range(0, n), Random.Next(0, n + 1)).Order<int, Int32Order>().AsIntervals();
    public static IEnumerable<int> Intervals(int n, int k) => Combination(Enumerable.Range(0, n), k).Order<int, Int32Order>().AsIntervals();

    public static Log<Δ1, List<int>> RandomMutation(this List<int> x) => x.Mutate(Δ1(x.Count, Number(10), Number(x.Count)), (key, j) => Number(1000));
    public static Log<Δ1, List<int>> RandomMutationOrdered(this List<int> x) => x.Mutate(Δ1(x.Count, Number(20), Number(x.Count)), (key, j) => x.Count == 0 ? Number(20) : Number(key == 0 ? 0 : x[key - 1], key == x.Count ? x[key - 1] + 5 : x[key] + 1));

    public static Δ1 Δ1(int count1, int count2, int delCount) => Intervals(count1, delCount).ToDel().Fold(Intervals(count2, count2 - count1 + delCount).ToIns());

    public static IEnumerable<T> Combination<T>(IEnumerable<T> x, int y) => CombinationInternal(x.ToArray(), y);

    private static IEnumerable<T> CombinationInternal<T>(IList<T> x, int y)
    {
      for (var i = 0; i < y; i++)
      {
        var index = Random.Next(x.Count - i);
        yield return x[index];
        x[index] = x[x.Count - i - 1];
      }
    }

    private static IEnumerable<T> PermutationInternal<T>(IEnumerable<T> x, int y) => PermutationInternal(x.ToArray(), y);

    private static IEnumerable<T> PermutationInternal<T>(IReadOnlyList<T> x, int y)
    {
      for (var i = 0; i < y; i++)
        yield return x[Random.Next(x.Count)];
    }

    public static Graph Graph(int count)
    {
      var g = DAG.Build;
      for (var i = 0; i < count; i++)
        g.Add(Bool(.8), Keys(i, Number(Math.Min(i + 1, 5))).ToArray());
      return g;
    }
  }

  public static class Expressions
  {
    public static IEnumerable<int> Numbers(params int[] numbers) => numbers.AsIntervals();
    
    public static IEnumerable<IPatch> InsRange<TItem>(this IExp<Δ1, IReadOnlyList<TItem>> x, IReadOnlyList<int> keys, IReadOnlyList<TItem> values) => x.ToPatch(values, keys.AsIntervals().ToIns());

    public static Log<Δ1, List<TItem>> MutateIns<TItem>(this List<TItem> x, int key, params TItem[] items) => x.Mutate(Intervals.Single(key, items.Length).ToIns(), (i, j) => items[j]);
    public static Log<Δ1, List<TItem>> MutateSub<TItem>(this List<TItem> x, int key, params TItem[] items) => x.Mutate(Intervals.Single(key, items.Length).ToSub(), (i, j) => items[j]);
    public static Log<Δ1, List<TItem>> MutateDel<TItem>(this List<TItem> x, int key, int count = 1) => x.Mutate(Intervals.Single(key, count).ToDel(), (i, j) => default(TItem));
  }
}