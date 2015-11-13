using System;
using System.Collections.Generic;
using System.Linq;
using Malbec.Collections.Generic.Orderings;
using Malbec.Logs;
using Malbec.Reactive.Patches;
using NUnit.Framework;
using static System.Math;
using static Malbec.Reactive.Composition;

namespace Malbec.Test.Reactive
{
  public class Integration
  {
    private static IReadOnlyList<T> Expected<T>(params T[] x) => x;

    [Test]
    public static void Simple1()
    {
      var x = Variable(7);
      var y = ℱ(n => n + 1, x);
      var z = ℱ(n => n%2, y);
      using (var test = Tests.Subscribe(nameof(y), y, nameof(z), z))
      {
        test.Assert(x.Assign(10), 11.ToLog(true), 1.ToLog(true));
        test.Assert(x.Assign(7), 8.ToLog(true), 0.ToLog(true));
        test.Assert(x.Assign(7), 8, 0);
        test.Assert(x.Assign(9), 10.ToLog(true), 0);
      }
    }

    [Test]
    public static void List()
    {
      var x = Variable("A", "B", "C");
      using (var test = Tests.Subscribe(nameof(x), x))
      {
        test.Assert(x.Ins(1, "x"), Expected("A", "x", "B", "C").ToLog(Δ.Ins(1)));
        test.Assert(x.Del(2, 2), Expected("A", "x").ToLog(Δ.Del(2, 3)));
      }
    }

    [Test]
    public static void HigherOrder1()
    {
      var x = Variable(7);
      var y = Variable(x);
      var z = Variable(22);
      using (var test = Tests.Subscribe(nameof(y), Invoke(y)))
      {
        test.Assert(x.Assign(10), 10.ToLog(true));
        test.Assert(y.Assign(z), 22.ToLog(true));
        test.Assert(x.Assign(33), 22);
        test.Assert(y.Assign(x), 33.ToLog(true));
        test.Assert(y.Assign(z).Concat(z.Assign(9)), 9.ToLog(true));
        test.Assert(x.Assign(8).Concat(y.Assign(x)), 8.ToLog(true));
        test.Assert(x.Assign(11).Concat(y.Assign(z)), 9.ToLog(true));
      }
    }

    [Test]
    public static void HigherOrder2()
    {
      var a = Variable("A");
      var b = Variable("B");
      var c = ℱ(v => $"{v}+", b);
      var x = Variable(a);
      var y = Variable(c);
      using (var test = Tests.Subscribe(nameof(x), Invoke(x), nameof(y), Invoke(y)))
      {
        test.Assert(x.Assign(c), "B+".ToLog(true), "B+");
        test.Assert(x.Assign(a), "A".ToLog(true), "B+");
        test.Assert(x.Assign(c).Concat(b.Assign("C")), "C+".ToLog(true), "C+".ToLog(true));
      }
    }

    [Test]
    public static void TestMap()
    {
      var x = Variable(1, 5, 100);
      var y = Map(value => value + 1, x);
      using (var test = Tests.Subscribe(nameof(y), y))
      {
        test.Assert(x.Sub(1, 77), Expected(2, 78, 101).ToLog(Δ.Sub(1)));
        test.Assert(x.Ins(1, 999, 1000, 888, 6111), Expected(2, 1000, 1001, 889, 6112, 78, 101).ToLog(Δ.Ins(1, 2, 3, 4)));
      }
    }

    [Test]
    public static void TestMapInvoke()
    {
      var w = Variable(1);
      var x = Variable(2);
      var y = Variable(3);
      var z = Variable(w, x, y);
      using (var test = Tests.Subscribe(nameof(z), MapInvoke(z)))
      {
        test.Assert(z.Del(1), Expected(1, 3).ToLog(Δ.Del(1)));
        test.Assert(y.Assign(27), Expected(1, 27).ToLog(Δ.Sub(1)));
        test.Assert(z.InsRange(Expected(1, 3), Expected(x, x)), Expected(1, 2, 27, 2).ToLog(Δ.Ins(1, 3)));
        test.Assert(x.Assign(12), Expected(1, 12, 27, 12).ToLog(Δ.Sub(1, 3)));
        test.Assert(x.Assign(13).Concat(z.Sub(1, w)), Expected(1, 1, 27, 13).ToLog(Δ.Sub(1, 3)));
      }
    }

    [Test]
    public static void TestChoose()
    {
      var flag = Variable(false);
      var option = Choose(77, 78, flag);
      using (var test = Tests.Subscribe(nameof(option), option))
        test.Assert(flag.Assign(true), 78.ToLog(true));
    }

    private static double Sqrt(double x)
    {
      if (x < 0)
        throw new Exception("Nooooo!");
      return Math.Sqrt(x);
    }

    [Test]
    public static void TestOption()
    {
      var value = Variable(2.0);
      var option = Option(ℱ(v => v >= 0, value), ℱ(Sqrt, value));
      using (var test = Tests.Subscribe(nameof(option), option))
      {
        test.Assert(value.Assign(4), ((double?) 2.0).ToLog(true));
        test.Throws(value.Assign(-4));
      }
    }

    [Test]
    public static void HighsAndLows()
    {
      var prices = Variable(2, 4, 3, 1, 6, 5);
      var times = Variable(1, 5, 10, 10, 11, 15);
      var high = Fold(
        Max,
        Filter(
          prices,
          LowerBounds<int, Int32Order>(
            times,
            Constant(5, 11))));

      var low = Fold(
        Min,
        Filter(
          prices,
          LowerBounds<int, Int32Order>(
            times,
            Constant(5, 11))));

      using (var test = Tests.Subscribe(nameof(high), high, nameof(low), low))
      {
        test.Assert(prices.Sub(1, 25), 25.ToLog(true), 1);
        test.Assert(prices.Sub(1, -1), 3.ToLog(true), (-1).ToLog(true));
        test.Assert(times.Sub(1, 4), 3, 1.ToLog(true));
      }
    }
  }
}