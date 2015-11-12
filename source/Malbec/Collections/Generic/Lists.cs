using System;
using System.Collections.Generic;
using Malbec.Collections.Generic.Orderings;

namespace Malbec.Collections.Generic
{
  public static class Lists<TItem>
  {
    public static readonly IReadOnlyList<TItem> Empty = new TItem[0];
  }

  public static class Lists
  {
    public static void CheckBounds(int count, int key)
    {
      if (key < 0)
        throw new IndexOutOfRangeException(nameof(key));
      if (key >= count)
        throw new IndexOutOfRangeException(nameof(key));
    }

    public static void CheckBounds(int listCount, int start, int count)
    {
      if (start < 0)
        throw new IndexOutOfRangeException(nameof(start));
      if (count < 0)
        throw new IndexOutOfRangeException(nameof(count));
      if (start + count > listCount)
        throw new IndexOutOfRangeException();
    }

    public static int Search<TItem>(this IReadOnlyList<TItem> items, TItem item, int start, int count, Func<TItem, TItem, bool> predicate)
    {
      CheckBounds(items.Count, start, count);
      while (count > 0)
      {
        var δ = count >> 1;
        if (predicate(item, items[start + δ]))
          start += count - δ;
        count -= count - δ;
      }
      return start;
    }

    public static int LowerBound<TItem, TOrder>(this IReadOnlyList<TItem> items, TItem item, int start, int count) where TOrder : struct, IOrdering<TItem> => items.Search(item, start, count, (item1, item2) => !default(TOrder)[item1, item2]);
    public static int LowerBoundEx<TItem, TOrder>(this IReadOnlyList<TItem> items, TItem item, int start, int count) where TOrder : struct, IOrdering<TItem> => items.Search(item, start, count, (item1, item2) => default(TOrder)[item2, item1]);
    public static int UpperBound<TItem, TOrder>(this IReadOnlyList<TItem> items, TItem item, int start, int count) where TOrder : struct, IOrdering<TItem> => items.LowerBoundEx<TItem, TOrder>(item, start, count) - 1;
    public static int UpperBoundEx<TItem, TOrder>(this IReadOnlyList<TItem> items, TItem item, int start, int count) where TOrder : struct, IOrdering<TItem> => items.LowerBound<TItem, TOrder>(item, start, count) - 1;

    public static int LowerBound<TItem, TOrder>(this IReadOnlyList<TItem> items, TItem item) where TOrder : struct, IOrdering<TItem> => items.LowerBound<TItem, TOrder>(item, 0, items.Count);
    public static int LowerBoundEx<TItem, TOrder>(this IReadOnlyList<TItem> items, TItem item) where TOrder : struct, IOrdering<TItem> => items.LowerBoundEx<TItem, TOrder>(item, 0, items.Count);
    public static int UpperBound<TItem, TOrder>(this IReadOnlyList<TItem> items, TItem item) where TOrder : struct, IOrdering<TItem> => items.UpperBound<TItem, TOrder>(item, 0, items.Count);
    public static int UpperBoundEx<TItem, TOrder>(this IReadOnlyList<TItem> items, TItem item) where TOrder : struct, IOrdering<TItem> => items.UpperBoundEx<TItem, TOrder>(item, 0, items.Count);

    public static int? TryLowerBound<TItem, TOrder>(this IReadOnlyList<TItem> items, TItem item) where TOrder : struct, IOrdering<TItem>
    {
      var index = items.LowerBound<TItem, TOrder>(item, 0, items.Count);
      return index < items.Count ? index : (int?)null;
    }
  }
}