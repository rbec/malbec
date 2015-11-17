using System;
using System.Collections.Generic;
using System.Linq;

namespace Malbec.Collections
{
  public static class Intervals
  {
    public static IEnumerable<int> Starting(this int x)
    {
      yield return x;
    }

    public static IEnumerable<int> Single(int x, int y)
    {
      yield return x;
      yield return y;
    }

    public static IEnumerable<int> Ending(this int x) => Single(0, x);

    public static IEnumerable<int> AsIntervals(this IEnumerable<int> x)
    {
      using (var e = x.GetEnumerator())
        if (e.MoveNext())
        {
          var number = e.Current;
          var last = number;
          yield return number;
          while (e.MoveNext())
          {
            if (e.Current > ++number)
            {
              yield return last = number - last;
              yield return last = e.Current - last;
            }
            number = e.Current;
          }
          yield return ++number - last;
        }
    }

    public static IEnumerable<int> AsNumbers(this IEnumerable<int> x)
    {
      using (var e = x.GetEnumerator())
        if (e.MoveNext())
        {
          var p = false;
          var last1 = 0;
          var last2 = e.Current;
          while (e.MoveNext())
          {
            if (p = !p)
              for (var i = last1 + last2; i < last2 + e.Current; i++)
                yield return i;
            last1 = last2;
            last2 = e.Current;
          }
          if (p) yield break;
          for (var i = last1 + last2; i < int.MaxValue; i++)
            yield return i;
        }
    }

    public static IEnumerable<int> ToExternal(this IEnumerable<int> x)
    {
      var l = 0;
      foreach (var i in x)
        yield return l = i - l;
    }

    public static IEnumerable<int> ToInternal(this IEnumerable<int> x)
    {
      var l = 0;
      foreach (var i in x)
      {
        yield return l + i;
        l = i;
      }
    }

    public static IEnumerable<int> WithoutPairs(this IEnumerable<int> x)
    {
      using (var e = x.GetEnumerator())
        if (e.MoveNext())
        {
          var parity = true;
          var last = e.Current;
          while (e.MoveNext())
          {
            if (e.Current == last)
              parity = !parity;
            else
            {
              if (parity)
                yield return last;
              else
                parity = true;
              last = e.Current;
            }
          }
          if (parity)
            yield return last;
        }
    }

    public static IEnumerable<int> And(this IEnumerable<int> x, IEnumerable<int> y) => Merge(x, y, false, false);
    public static IEnumerable<int> Or(this IEnumerable<int> x, IEnumerable<int> y) => Merge(x, y, true, true);
    public static IEnumerable<int> Not(this IEnumerable<int> x, IEnumerable<int> y) => Merge(x, y, true, false);
    public static IEnumerable<int> Not(this IEnumerable<int> x) => x.Not(0.Starting());
    public static IEnumerable<int> AndKeys(this IEnumerable<int> x, IEnumerable<int> y) => x.AndKeysInternal(y).WithoutPairs().ToExternal();
    public static IEnumerable<int> OrKeys(this IEnumerable<int> x, IEnumerable<int> y) => x.ToInternal().OrKeysInternal(y.ToInternal()).ToExternal().Or(x); // TODO: FIX
    public static IEnumerable<int> SubKeys(this IEnumerable<int> del, IEnumerable<int> ins) => SubsKeysInternal(del.ToInternal(), ins.ToInternal()).ToExternal();

    #region Internal
    private static IEnumerable<int> Merge(IEnumerable<int> x, IEnumerable<int> y, bool px, bool py)
    {
      using (var ex = x.GetEnumerator())
      using (var ey = y.GetEnumerator())
      {
        var l = 0;
        var lx = 0;
        var ly = 0;
        var nx = ex.MoveNext();
        var ny = ey.MoveNext();
        while (nx && ny)
        {
          if (ex.Current + lx < ey.Current + ly)
          {
            if (py)
              yield return l = ex.Current + lx - l;
            lx = ex.Current;
            nx = ex.MoveNext();
            px = !px;
          }
          else if (ex.Current + lx > ey.Current + ly)
          {
            if (px)
              yield return l = ey.Current + ly - l;
            ly = ey.Current;
            ny = ey.MoveNext();
            py = !py;
          }
          else
          {
            if (px == py)
              yield return l = ex.Current + lx - l;
            lx = ex.Current;
            ly = ey.Current;

            nx = ex.MoveNext();
            px = !px;
            ny = ey.MoveNext();
            py = !py;
          }
        }
        if (py)
          while (nx)
          {
            yield return l = ex.Current + lx - l;
            lx = ex.Current;
            nx = ex.MoveNext();
          }
        if (px)
          while (ny)
          {
            yield return l = ey.Current + ly - l;
            ly = ey.Current;
            ny = ey.MoveNext();
          }
      }
    }

    private static IEnumerable<int> AndKeysInternal(this IEnumerable<int> x, IEnumerable<int> y)
    {
      using (var ex = x.GetEnumerator())
      using (var ey = y.GetEnumerator())
      {
        var px = false;
        var py = false;
        var lx = 0;
        var ly = 0;
        var nx = ex.MoveNext();
        var ny = ey.MoveNext();
        while (nx && ny)
        {
          if (ex.Current + lx < ey.Current + ly)
          {
            if (py)
              yield return px ? ex.Current : lx;
            lx = ex.Current;
            nx = ex.MoveNext();
            px = !px;
          }
          else if (ex.Current + lx > ey.Current + ly)
          {
            if (px)
              yield return ey.Current - lx + ly;
            ly = ey.Current;
            ny = ey.MoveNext();
            py = !py;
          }
          else
          {
            if ((px = !px) == (py = !py))
              yield return px ? lx : ex.Current;
            lx = ex.Current;
            ly = ey.Current;

            nx = ex.MoveNext();
            ny = ey.MoveNext();
          }
        }
        if (py)
          while (nx)
          {
            yield return (px = !px) ? lx : ex.Current;
            lx = ex.Current;
            nx = ex.MoveNext();
          }
        if (px)
          while (ny)
          {
            yield return ey.Current - lx + ly;
            ly = ey.Current;
            ny = ey.MoveNext();
          }
      }
    }
    
    private static IEnumerable<int> OrKeysInternal(this IEnumerable<int> x, IEnumerable<int> y)
    {
      using (var e1 = x.GetEnumerator())
      using (var e2 = y.GetEnumerator())
      {
        var b1 = e1.MoveNext();
        var b2 = e2.MoveNext();
        int n1 = 0, n2 = 0;
        var sum = 0;
        while (b1 && b2)
        {
          if (e1.Current < e2.Current + sum)
          {
            if (n1 % 2 == 0)
            {
              var last = e1.Current; // todo: make sure this always works, even when past end of enumerator
              b1 = e1.MoveNext();
              sum += e1.Current - last;
            }
            else
              b1 = e1.MoveNext();
            n1++;
          }
          else if (e1.Current > e2.Current + sum)
          {
            yield return e2.Current + sum;
            b2 = e2.MoveNext();
            n2++;
          }
          else
          {
            yield return e2.Current + sum;

            if (n1 % 2 == 0)
            {
              var last = e1.Current;
              b1 = e1.MoveNext();
              sum += e1.Current - last;
            }
            else
              b1 = e1.MoveNext();

            n1++;
            b2 = e2.MoveNext();
            n2++;
          }
        }
        while (b2)
        {
          yield return e2.Current + sum;
          b2 = e2.MoveNext();
        }
      }
    }

    private static IEnumerable<int> SubsKeysInternal(this IEnumerable<int> del, IEnumerable<int> ins)
    {
      using (var eDel = del.GetEnumerator())
      using (var eIns = ins.GetEnumerator())
      {
        var pDel = true;
        var pIns = true;
        var bDel = eDel.MoveNext();
        var bIns = eIns.MoveNext();
        var sum = 0;
        while (bDel && bIns)
        {
          if (eDel.Current + sum == eIns.Current)
          {
            var lastIns = eIns.Current;
            var lastDel = eDel.Current;

            if (pIns && pDel)
            {
              yield return eIns.Current;
              bIns = eIns.MoveNext();
              bDel = eDel.MoveNext();

              var sIns = bIns ? eIns.Current - lastIns : int.MaxValue - lastIns;
              var sDel = bDel ? eDel.Current - lastDel : int.MaxValue - lastDel;

              if (sIns < sDel)
                yield return eIns.Current;
              else
                yield return lastIns + eDel.Current - lastDel;

              if (bIns)
                sum += eIns.Current - lastIns;
              if (bDel)
                sum -= eDel.Current - lastDel;
            }
            else
            {

              if (pIns)
              {
                if (bIns = eIns.MoveNext())
                  sum += eIns.Current - lastIns;
              }
              else
                bIns = eIns.MoveNext();

              if (pDel)
              {
                if (bDel = eDel.MoveNext())
                  sum -= eDel.Current - lastDel;
              }
              else
                bDel = eDel.MoveNext();
            }

            pIns = !pIns;
            pDel = !pDel;
          }
          else if (eDel.Current + sum < eIns.Current)
          {
            if (pDel)
            {
              var last = eDel.Current;
              bDel = eDel.MoveNext();
              sum -= eDel.Current - last;
            }
            else
              bDel = eDel.MoveNext();

            pDel = !pDel;
          }
          else
          {
            if (pIns)
            {
              var last = eIns.Current;
              if (bIns = eIns.MoveNext())
                sum += eIns.Current - last;
            }
            else
              bIns = eIns.MoveNext();

            pIns = !pIns;
          }
        }
      }
    }
    
    #endregion

    public static string AsString(this IEnumerable<int> x) => string.Join(", ", x.AsStringInternal());

    private static IEnumerable<string> AsStringInternal(this IEnumerable<int> x)
    {
      using (var ex = x.GetEnumerator())
      {
        var l2 = 0;
        while (ex.MoveNext())
        {
          var l1 = ex.Current;
          if (ex.MoveNext())
          {
            var start = l1 + l2;
            var end = ex.Current + l1;
            if (start < end--)
              yield return start == end ? $"{start}" : $"[{start}-{end}]";
            l2 = ex.Current;
          }
          else
          {
            yield return $"[{l1 + l2}...]";
            yield break;
          }
        }
      }
    }

    public static string IntervalsAsStars(this IReadOnlyList<int> x, char c, int count) => string.Join(string.Empty, Enumerable.Range(0, count).Select(n => x.IntervalsContain(n) ? c.ToString() : " "));

    public static bool IntervalsContain(this IReadOnlyList<int> x, int y)
    {
      var count = x.Count;
      var start = 0;
      while (count > 0)
      {
        var δ = count >> 1;
        var key = start + δ;
        if (y >= (key == 0 ? 0 : x[key - 1]) + x[key])
          start += count - δ;
        count -= count - δ;
      }
      return start % 2 != 0;
    }

    public static int IntervalsCount(this IReadOnlyList<int> x)
    {
      if (x.Count % 2 == 1)
        throw new Exception();
      return x.Count == 0 ? 0 : x[x.Count - 1];
    }

    public static int Value(this IReadOnlyList<int> x, int y)
    {
      if (y < 0)
        throw new ArgumentOutOfRangeException();
      var count = x.Count / 2;
      var start = 0;
      while (count > 0)
      {
        var δ = count >> 1;
        if (y >= x[(start + δ) * 2 + 1])
          start += count - δ;
        count -= count - δ;
      }
      return x[start * 2] + y;
    }

    public static IEnumerable<int> Concat(this IEnumerable<int> x, int y, IEnumerable<int> z) => y.Ending().OrKeys(z.Not()).Not().Or(x);
  }
}