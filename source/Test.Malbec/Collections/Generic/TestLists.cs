using System;
using System.Collections.Generic;
using System.Linq;
using Malbec.Collections.Generic;
using Malbec.Collections.Generic.Orderings;
using NUnit.Framework;

namespace Malbec.Test.Collections.Generic
{
  public class TestLists
  {
    [Test] public static void TestLowerBound() => TestOrderedSearch(()=> MonteCarlo.Ordered(150).ToList(), Lists.LowerBound<int, Int32Order>, LowerBound);
    [Test] public static void TestLowerBoundEx() => TestOrderedSearch(() => MonteCarlo.Ordered(150).ToList(), Lists.LowerBoundEx<int, Int32Order>, LowerBoundEx);
    [Test] public static void TestUpperBound() => TestOrderedSearch(() => MonteCarlo.Ordered(150).ToList(), Lists.UpperBound<int, Int32Order>, UpperBound);
    [Test] public static void TestUpperBoundEx() => TestOrderedSearch(() => MonteCarlo.Ordered(150).ToList(), Lists.UpperBoundEx<int, Int32Order>, UpperBoundEx);

    public static void TestOrderedSearch<T>(Func<IReadOnlyList<int>> generator, Func<IReadOnlyList<int>, int, int, int, T> f1, Func<IReadOnlyList<int>, int, int, int, T> f2)
    {
      var array = generator();
      Assert.Throws<IndexOutOfRangeException>(() => f1(array, 3, -1, array.Count));
      Assert.Throws<IndexOutOfRangeException>(() => f1(array, 3, 0, array.Count + 1));
      Assert.Throws<IndexOutOfRangeException>(() => f1(array, 3, 1, -1));
      for (var i = 0; i < 1000; i++)
      {
        array = generator();
        var maxValue = array.Count == 0 ? 100 : array[array.Count - 1] + 100;
        for (var j = 0; j < 10; j++)
        {
          var search = Random.Next(maxValue);
          var start = Random.Next(array.Count + 1);
          var length = Random.Next(start, array.Count + 1) - start;
          Assert.That(f1(array, search, start, length), Is.EqualTo(f2(array, search, start, length)));
        }
      }
    }

    private static int LowerBound(IReadOnlyList<int> array, int value, int start, int length)
    {
      var key = start;
      while (key < start + length && array[key] < value)
        key++;
      return key;
    }

    private static int LowerBoundEx(IReadOnlyList<int> array, int value, int start, int length)
    {
      var key = start;
      while (key < start + length && array[key] <= value)
        key++;
      return key;
    }

    private static int UpperBound(IReadOnlyList<int> array, int value, int start, int length)
    {
      var key = start + length - 1;
      while (key >= start && array[key] > value)
        key--;
      return key;
    }

    private static int UpperBoundEx(IReadOnlyList<int> array, int value, int start, int length)
    {
      var key = start + length - 1;
      while (key >= start && array[key] >= value)
        key--;
      return key;
    }

    private static readonly Random Random = new Random();
  }
}