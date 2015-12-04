using System;
using System.Linq;
using Malbec.Collections;
using Malbec.Collections.Generic;
using Malbec.Functions;
using Malbec.Logs;
using NUnit.Framework;

namespace Test.Malbec.Functions
{
  public class TestFilterFunction
  {
    private readonly LogComparer<FilterList<int>, int> Comparer = new LogComparer<FilterList<int>, int>();

    [Test]
    public void TestReactXChanged()
    {
      var f = new FilterFunction<int>();

      var x = Enumerable.Range(0, 10).ToList();
      var y = Intervals.Single(5, 5).ToList();

      var value = f[x, y];

      Assert.That(f.React(value, x.ToLog(Expressions.Numbers(4).ToSub()), y.ToLog(Δ1.Empty)), Is.EqualTo(value.ToLog(Δ1.Empty)).Using(Comparer));
      Assert.That(f.React(value, x.ToLog(Expressions.Numbers(6).ToSub()), y.ToLog(Δ1.Empty)), Is.EqualTo(value.ToLog(Expressions.Numbers(1).ToSub())).Using(Comparer));
      Assert.That(f.React(value, x.ToLog(Expressions.Numbers(4, 6).ToSub()), y.ToLog(Δ1.Empty)), Is.EqualTo(value.ToLog(Expressions.Numbers(1).ToSub())).Using(Comparer));
    }

    [Test]
    public void TestReactYChanged()
    {
      var f = new FilterFunction<int>();

      var x = Enumerable.Range(0, 10).ToList();

      var y1 = Intervals.Single(5, 5).ToList();
      var y2 = Intervals.Single(4, 6).ToList();

      var value = f[x, y1];

      var δ = Δ1.From(y2.Not(y1), y1.Not(y2));

      Console.WriteLine(δ.Del);
      Console.WriteLine(δ.Ins);

      Assert.That(f.React(value, x.ToLog(Δ1.Empty), y2.ToLog(δ)), Is.EqualTo(value.ToLog(Expressions.Numbers(0).ToIns())).Using(Comparer));

      value = f[x, y2];
      Assert.That(f.React(value, x.ToLog(Expressions.Numbers(6).ToSub()), y2.ToLog(δ)), Is.EqualTo(value.ToLog(Expressions.Numbers(0).ToIns().Fold(Expressions.Numbers(2).ToSub()))).Using(Comparer));

      value = f[x, y2];
      Assert.That(f.React(value, x.ToLog(Expressions.Numbers(3, 9).ToSub()), y2.ToLog(δ)), Is.EqualTo(value.ToLog(Expressions.Numbers(0).ToIns().Fold(Expressions.Numbers(5).ToSub()))).Using(Comparer));
    }

    [Test]
    public void TestReactRandom()
    {
      var f = new FilterFunction<int>();

      for (var i = 0; i < 100; i++)
      {
        var x = MonteCarlo.Unordered(20, MonteCarlo.Number(10)).ToList();
        var y = MonteCarlo.Intervals(x.Count).ToList();

        var x0 = x.ToList();
        var y0 = y.ToList();

        var v = f[x, y];

        var v0 = v.ToList();

        var xf = y.AsNumbers().Select(j => x[j]).ToList();

        var xLog = x.RandomMutation();

        var yNew = MonteCarlo.Intervals(x.Count).ToList();

        var δy = Δ1.From(yNew.Not(y), y.Not(yNew));

        y.Clear();
        y.AddRange(yNew);

        var δv = f.React(v, xLog, y.ToLog(δy));

        var x2Filter = y.AsNumbers().Select(j => x[j]).ToList();

        xf.Mutate(δv.Δ, (key, j) => x2Filter[key]);

        Console.WriteLine("x    |{0}|", string.Join(string.Empty, x0));
        Console.WriteLine(" -   |{0}|", xLog.Δ.Del.IntervalsAsStars('-', x0.Count));
        Console.WriteLine(" +   |{0}|", xLog.Δ.Ins.IntervalsAsStars('+', x.Count));
        Console.WriteLine("x    |{0}|", string.Join(string.Empty, x));

        Console.WriteLine();

        Console.WriteLine("y    |{0}|", y0.IntervalsAsStars('*', x0.Count));
        Console.WriteLine(" -   |{0}|", δy.Del.IntervalsAsStars('-', x0.Count));
        Console.WriteLine(" +   |{0}|", δy.Ins.IntervalsAsStars('+', x.Count));
        Console.WriteLine("y    |{0}|", y.IntervalsAsStars('*', x.Count));

        Console.WriteLine();

        Console.WriteLine("v    |{0}|", string.Join(string.Empty, v0));
        Console.WriteLine(" -   |{0}|", δv.Δ.Del.IntervalsAsStars('-', v0.Count));
        Console.WriteLine(" +   |{0}|", δv.Δ.Ins.IntervalsAsStars('+', v.Count));
        Console.WriteLine("v    |{0}|", string.Join(string.Empty, v));

        Console.WriteLine();
        Console.WriteLine("//////////////////////////////////////////////");
        Console.WriteLine();

        Assert.That(xf, Is.EqualTo(x2Filter));
      }
    }
  }
}