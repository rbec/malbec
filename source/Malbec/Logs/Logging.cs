using System;
using System.Collections.Generic;
using System.Linq;
using Malbec.Collections;
using Malbec.Collections.Generic;

namespace Malbec.Logs
{
  public static class Logging
  {
    public static Log<Δ0, T> ToLog<T>(this T value, bool isChanged) => new Log<Δ0, T>(isChanged, value);
    public static Log<Δ1, T> ToLog<T>(this T value, Δ1 δ) => new Log<Δ1, T>(δ, value);

    public static Log<Δ0, T> Assign<T>(this T value, T newValue) => newValue.ToLog(!Equals(value, newValue));

    public static Δ1 AsDel(this IReadOnlyList<int> x) => Δ1.From(x, Lists<int>.Empty);
    public static Δ1 AsSub(this IReadOnlyList<int> x) => Δ1.From(x, x);
    public static Δ1 AsIns(this IReadOnlyList<int> x) => Δ1.From(Lists<int>.Empty, x);

    public static Δ1 ToDel(this IEnumerable<int> x) => x.ToList().AsDel();
    public static Δ1 ToSub(this IEnumerable<int> x) => x.ToList().AsSub();
    public static Δ1 ToIns(this IEnumerable<int> x) => x.ToList().AsIns();

    public static int OldCount<TItem>(this ILog<Δ1, IReadOnlyList<TItem>> x) => x.Value.Count + x.Δ.Del.IntervalsCount() - x.Δ.Ins.IntervalsCount();

    public static IEnumerable<int> NotDel<TItem>(this ILog<Δ1, IReadOnlyList<TItem>> x) => x.Δ.Del.Not(x.OldCount().Ending());
    public static IEnumerable<int> NotIns<TItem>(this ILog<Δ1, IReadOnlyList<TItem>> x) => x.Δ.Ins.Not(x.Value.Count.Ending());

    public static bool IsAppend<TItem>(this ILog<Δ1, IReadOnlyList<TItem>> x) => x.Δ.Del.Count == 0 && x.Δ.Ins.Count == 2 && x.Δ.Ins[0] + x.Δ.Ins[1] == x.Value.Count;

    public static Δ1 Fold(this Δ1 δ1, Δ1 δ2) => Δ1.From(
      δ1.Del.OrKeys(δ1.Ins.Not().AndKeys(δ2.Del.Not()).Not()),
      δ2.Ins.OrKeys(δ2.Del.Not().AndKeys(δ1.Ins.Not()).Not()));

    public static Δ1 Concat(this Δ1 δ1, int count1, Δ1 δ2, int count2) => Δ1.From(δ1.Del.Concat(count1, δ2.Del), δ1.Ins.Concat(count2, δ2.Ins));

    public static Log<Δ1, List<TItem>> Mutate<TItem>(this List<TItem> value, Δ1 δ, Func<int, int, TItem> insItem)
    {
      var k = 0;
      foreach (var key in δ.Del.AsNumbers())
        value.RemoveAt(key - k++);

      k = 0;
      foreach (var key in δ.Ins.AsNumbers())
        value.Insert(key, insItem(key, k++));

      return value.ToLog(δ);
    }

    public static Log<Δ1, List<TItem>> Mutate<TItem>(this List<TItem> value, Δ1 δ, Func<int, int, TItem> insItem, Action<TItem> delItem)
    {
      var k = 0;
      foreach (var key in δ.Del.AsNumbers())
      {
        delItem(value[key - k]);
        value.RemoveAt(key - k++);
      }

      k = 0;
      foreach (var key in δ.Ins.AsNumbers())
        value.Insert(key, insItem(key, k++));

      return value.ToLog(δ);
    }
  }
}