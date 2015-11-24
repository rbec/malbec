using System;
using System.Collections.Generic;
using System.Linq;
using Malbec.Collections.Generic.Orderings;

namespace Malbec.Collections.Generic
{
  public static class Enumerables
  {
    public static string ToCSV<T>(this IEnumerable<T> items, int max = 100)
      => items.ToCSV(value => value.ToString());

    public static string ToCSV<T>(this IEnumerable<T> items, Func<T, string> itemFormatter, int max = 100) 
      => $"{{{string.Join(", ", items.Take(max).Select(itemFormatter))}}}";

    public static string ToLines<T>(this IEnumerable<T> items, int max = 100)
      => $"{string.Join(Environment.NewLine, items.Take(max))}";

    public static IEnumerable<TItem> Order<TItem, TOrder>(this IEnumerable<TItem> items) where TOrder : struct, IOrdering<TItem> 
      => items.OrderBy(value => value, new Comparer<TItem, TOrder>());

    public static IEnumerable<int> Order(this IEnumerable<int> items)
      => items.Order<int, Int32Order>();

    public static IEnumerable<TItem> Scan<TItem>(this IEnumerable<TItem> items, Func<TItem, TItem, TItem> f)
    {
      using (var e = items.GetEnumerator())
        if (e.MoveNext())
        {
          var seed = e.Current;
          yield return seed;
          while (e.MoveNext())
            yield return seed = f(seed, e.Current);
        }
    }
  }
}