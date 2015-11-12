using System;
using System.Collections.Generic;
using System.Linq;
using Malbec.Collections;
using Malbec.Collections.Generic;
using Malbec.Collections.Generic.Orderings;
using NUnit.Framework;

namespace Malbec.Test.Collections
{
  public class TestIntervals
  {
    public static IEnumerable<int> AsSimpleIntervals(IEnumerable<int> x)
    {
      using (var e = x.GetEnumerator())
        if (e.MoveNext())
        {
          var number = e.Current;
          yield return number;
          while (e.MoveNext())
          {
            if (e.Current > ++number)
            {
              yield return number;
              yield return e.Current;
            }
            number = e.Current;
          }
          yield return ++number;
        }
    }

    [Test]
    public void TestToExternal()
    {
      for (var i = 0; i < 100; i++)
      {
        var x = MonteCarlo.Keys(50).ToList();
        Assert.That(AsSimpleIntervals(x).ToExternal(), Is.EqualTo(x.AsIntervals()));
      }
    }

    [Test]
    public void TestToInternal()
    {
      for (var i = 0; i < 100; i++)
      {
        var x = MonteCarlo.Keys(50).ToList();
        Assert.That(x.AsIntervals().ToInternal(), Is.EqualTo(AsSimpleIntervals(x)));
      }
    }

    [Test]
    public void TestCount()
    {
      for (var i = 0; i < 100; i++)
      {
        var x = MonteCarlo.Keys(50).ToList();
        Assert.That(x.AsIntervals().ToList().IntervalsCount(), Is.EqualTo(x.Count));
      }
    }

    [Test]
    public void TestCountInfinite()
    {
      Assert.Throws<Exception>(() => Intervals.Single(5, 5).Not().ToList().IntervalsCount());
    }

    [Test]
    public void TestValue()
    {
      var x1 = new[] {1, 1, 2, 4, 4, 4, 4, 7};

      Assert.That(x1.Value(0), Is.EqualTo(1));
      Assert.That(x1.Value(1), Is.EqualTo(3));
      Assert.That(x1.Value(2), Is.EqualTo(4));
      Assert.That(x1.Value(3), Is.EqualTo(5));
      Assert.That(x1.Value(4), Is.EqualTo(8));
      Assert.That(x1.Value(5), Is.EqualTo(9));
      Assert.That(x1.Value(6), Is.EqualTo(10));
      Assert.Throws<ArgumentOutOfRangeException>(() => x1.Value(-1));
      Assert.Throws<ArgumentOutOfRangeException>(() => x1.Value(7));

      var x2 = new[] {1, 3, 4, 5, 8, 9, 10}.AsIntervals().ToList();

      Assert.That(x2.Value(0), Is.EqualTo(1));
      Assert.That(x2.Value(1), Is.EqualTo(3));
      Assert.That(x2.Value(2), Is.EqualTo(4));
      Assert.That(x2.Value(3), Is.EqualTo(5));
      Assert.That(x2.Value(4), Is.EqualTo(8));
      Assert.That(x2.Value(5), Is.EqualTo(9));
      Assert.That(x2.Value(6), Is.EqualTo(10));

      Assert.Throws<ArgumentOutOfRangeException>(() => x2.Value(-1));
      Assert.Throws<ArgumentOutOfRangeException>(() => x2.Value(7));
    }

    [Test]
    public void TestValueInfinite()
    {
      var x1 = new[] {1, 1, 2, 4, 4, 4, 4};

      Assert.Throws<ArgumentOutOfRangeException>(() => x1.Value(-1));
      Assert.That(x1.Value(0), Is.EqualTo(1));
      Assert.That(x1.Value(1), Is.EqualTo(3));
      Assert.That(x1.Value(2), Is.EqualTo(4));
      Assert.That(x1.Value(3), Is.EqualTo(5));
      Assert.That(x1.Value(4), Is.EqualTo(8));
      Assert.That(x1.Value(5), Is.EqualTo(9));
      Assert.That(x1.Value(6), Is.EqualTo(10));
      Assert.That(x1.Value(7), Is.EqualTo(11));
      Assert.That(x1.Value(10000), Is.EqualTo(10004));

      var x2 = new[] {1, 1, 2, 4, 4};

      Assert.Throws<ArgumentOutOfRangeException>(() => x2.Value(-1));
      Assert.That(x2.Value(0), Is.EqualTo(1));
      Assert.That(x2.Value(1), Is.EqualTo(3));
      Assert.That(x2.Value(2), Is.EqualTo(4));
      Assert.That(x2.Value(3), Is.EqualTo(5));
      Assert.That(x2.Value(4), Is.EqualTo(8));
      Assert.That(x2.Value(5), Is.EqualTo(9));
      Assert.That(x2.Value(6), Is.EqualTo(10));
      Assert.That(x1.Value(7), Is.EqualTo(11));
      Assert.That(x1.Value(10000), Is.EqualTo(10004));
    }

    [Test]
    public void TestValueRandomly()
    {
      for (var i = 0; i < 100; i++)
      {
        var x = MonteCarlo.Intervals(50).ToList();
        Console.WriteLine(x.ToCSV(100));
        var y = x.AsNumbers().ToList();
        for (var j = 0; j < y.Count; j++)
          Assert.That(x.Value(j), Is.EqualTo(y[j]));
      }
    }

    [Test]
    public void TestContainsRandomly()
    {
      for (var i = 0; i < 100; i++)
      {
        var x = MonteCarlo.Intervals(50).ToList();
        Console.WriteLine(x.ToCSV(100));
        var xn = x.Not(100.Ending()).ToList();

        foreach (var j in x.AsNumbers().ToList())
          Assert.True(x.IntervalsContain(j));

        foreach (var j in xn.AsNumbers().ToList())
          Assert.False(x.IntervalsContain(j));
        Assert.False(x.IntervalsContain(-1));
      }
    }

    [Test]
    public void TestAsString()
    {
      Assert.That(new[] {1}.AsIntervals().AsString(), Is.EqualTo("1"));
      Assert.That(new[] {1, 2}.AsIntervals().AsString(), Is.EqualTo("[1-2]"));
      Assert.That(new[] {1, 2, 3}.AsIntervals().AsString(), Is.EqualTo("[1-3]"));
      Assert.That(new[] {1, 2, 3, 5}.AsIntervals().AsString(), Is.EqualTo("[1-3], 5"));
      Assert.That(new[] {1, 2, 3, 5, 6}.AsIntervals().AsString(), Is.EqualTo("[1-3], [5-6]"));
      Assert.That(new[] {1, 2, 3, 2, 3, 5, 6}.AsIntervals().AsString(), Is.EqualTo("[1-3], [5-6]"));
      Assert.That(new[] {1, 2}.AsIntervals().Not().AsString(), Is.EqualTo("0, [3...]"));
    }

    [Test]
    public void TestNumbersAsIntervals()
    {
      Assert.That(new int[0].AsIntervals(), Is.EqualTo(new int[0]));
      Assert.That(new[] {1, 3}.AsIntervals(), Is.EqualTo(new[] {1, 1, 2, 2}));
      Assert.That(new[] {1, 2, 3}.AsIntervals(), Is.EqualTo(new[] {1, 3}));
      Assert.That(new[] {1, 3, 8, 9, 9}.AsIntervals(), Is.EqualTo(new[] {1, 1, 2, 2, 6, 4}));
    }

    [Test]
    public void TestIntervalsAsNumbers()
    {
      Assert.That(new int[0].AsNumbers(), Is.EqualTo(new int[0]));
      Assert.That(new[] {1, 1, 2, 2}.AsNumbers(), Is.EqualTo(new[] {1, 3}));
      Assert.That(new[] {1, 3}.AsNumbers(), Is.EqualTo(new[] {1, 2, 3}));
      Assert.That(new[] {1, 1, 2, 2, 6, 4}.AsNumbers(), Is.EqualTo(new[] {1, 3, 8, 9}));
      Assert.That(new[] {1, 3, 2}.AsNumbers().TakeWhile(n => n < 10), Is.EqualTo(new[] {1, 2, 3, 5, 6, 7, 8, 9}));
    }

    [Test]
    public void TestIntervalsAsNumbersRandom()
    {
      for (var i = 0; i < 100; i++)
      {
        var numbers = MonteCarlo.Keys(50).ToList();
        Assert.That(numbers.AsIntervals().AsNumbers(), Is.EqualTo(numbers));
      }
    }

    [Test]
    public static void TestNotIntervals()
    {
      var empty = new int[0].ToList();
      var a = new[] {0, 3}.ToList();
      var b = new[] {2, 2}.ToList();
      var c = new[] {0, 2}.ToList();
      var d = new[] {1, 1}.ToList();
      var e = new[] {2, 1}.ToList();
      var f = new[] {0, 1}.ToList();
      var g = new[] {3, 1}.ToList();

      Assert.That(b.Not(a), Is.EqualTo(c));
      Assert.That(a.Not(b), Is.EqualTo(g));
      Assert.That(d.Not(c), Is.EqualTo(f));
      Assert.That(c.Not(d), Is.EqualTo(empty));
      Assert.That(e.Not(c), Is.EqualTo(c));
      Assert.That(c.Not(e), Is.EqualTo(e));

      Assert.That(a.Not(), Is.EqualTo(new[] {3}));
    }

    [Test]
    public void TestNotIntervalsRandom()
    {
      for (var i = 0; i < 100; i++)
      {
        var xn = MonteCarlo.Keys(50).ToList();
        var yn = MonteCarlo.Keys(50).ToList();
        var xi = xn.AsIntervals().ToList();
        var yi = yn.AsIntervals().ToList();
        Assert.That(yn.Except(xn).Order(), Is.EqualTo(xi.Not(yi).AsNumbers()));
        Assert.That(xn.Except(yn).Order(), Is.EqualTo(yi.Not(xi).AsNumbers()));
      }
    }

    [Test]
    public static void TestOrIntervals()
    {
      var a = new[] {1, 2, 2, 3}.ToList();
      var b = new[] {2, 2}.ToList();

      Assert.That(a.Or(a), Is.EqualTo(a));
      Assert.That(b.Or(b), Is.EqualTo(b));
      Assert.That(a.Or(b), Is.EqualTo(new[] {1, 4}));
      Assert.That(b.Or(a), Is.EqualTo(new[] {1, 4}));
    }

    [Test]
    public void TestOrIntervalsRandom()
    {
      for (var i = 0; i < 100; i++)
      {
        var xn = MonteCarlo.Keys(50).ToList();
        var yn = MonteCarlo.Keys(50).ToList();
        var xi = xn.AsIntervals().ToList();
        var yi = yn.AsIntervals().ToList();
        Assert.That(xn.Union(yn).Order(), Is.EqualTo(xi.Or(yi).AsNumbers()));
        Assert.That(yn.Union(xn).Order(), Is.EqualTo(yi.Or(xi).AsNumbers()));
      }
    }

    [Test]
    public void TestOrIntervalsOneInfiniteRandom()
    {
      for (var i = 0; i < 100; i++)
      {
        var xn = MonteCarlo.Keys(50).ToList();
        var yn = MonteCarlo.Keys(50).ToList();
        var ynNot = Enumerable.Range(0, 100).Except(yn).ToList();
        var xi = xn.AsIntervals().ToList();
        var yi = yn.AsIntervals().Not().ToList();
        Assert.That(xn.Union(ynNot).Order(), Is.EqualTo(xi.Or(yi).AsNumbers().TakeWhile(n => n < 100)));
        Assert.That(ynNot.Union(xn).Order(), Is.EqualTo(yi.Or(xi).AsNumbers().TakeWhile(n => n < 100)));
      }
    }

    [Test]
    public void TestOrIntervalsBothInfiniteRandom()
    {
      for (var i = 0; i < 100; i++)
      {
        var xn = MonteCarlo.Keys(50).ToList();
        var yn = MonteCarlo.Keys(50).ToList();
        var xnNot = Enumerable.Range(0, 100).Except(xn).ToList();
        var ynNot = Enumerable.Range(0, 100).Except(yn).ToList();
        var xi = xn.AsIntervals().Not().ToList();
        var yi = yn.AsIntervals().Not().ToList();
        Assert.That(xnNot.Union(ynNot).Order(), Is.EqualTo(xi.Or(yi).AsNumbers().TakeWhile(n => n < 100)));
        Assert.That(ynNot.Union(xnNot).Order(), Is.EqualTo(yi.Or(xi).AsNumbers().TakeWhile(n => n < 100)));
      }
    }

    [Test]
    public static void TestAndIntervals()
    {
      var a = new[] {1, 2, 2, 3}.ToList();
      var b = new[] {2, 2}.ToList();

      Console.WriteLine(a.And(a).ToList());
      Assert.That(a.And(a), Is.EqualTo(a));
      Assert.That(b.And(b), Is.EqualTo(b));
      Assert.That(a.And(b), Is.EqualTo(new[] {2, 1}));
      Assert.That(b.And(a), Is.EqualTo(new[] {2, 1}));
    }

    [Test]
    public void TestAndIntervalsRandom()
    {
      for (var i = 0; i < 100; i++)
      {
        var xn = MonteCarlo.Keys(50).ToList();
        var yn = MonteCarlo.Keys(50).ToList();
        var xi = xn.AsIntervals().ToList();
        var yi = yn.AsIntervals().ToList();
        Assert.That(xn.Intersect(yn).Order(), Is.EqualTo(xi.And(yi).AsNumbers()));
        Assert.That(yn.Intersect(xn).Order(), Is.EqualTo(yi.And(xi).AsNumbers()));
      }
    }

    [Test]
    public void TestAndIntervalsOneInfiniteRandom()
    {
      for (var i = 0; i < 100; i++)
      {
        var xn = MonteCarlo.Keys(50).ToList();
        var yn = MonteCarlo.Keys(50).ToList();
        var ynNot = Enumerable.Range(0, 100).Except(yn).ToList();
        var xi = xn.AsIntervals().ToList();
        var yi = yn.AsIntervals().Not().ToList();
        Assert.That(xn.Intersect(ynNot).Order(), Is.EqualTo(xi.And(yi).AsNumbers().TakeWhile(n => n < 100)));
        Assert.That(ynNot.Intersect(xn).Order(), Is.EqualTo(yi.And(xi).AsNumbers().TakeWhile(n => n < 100)));
      }
    }

    [Test]
    public void TestAndIntervalsBothInfiniteRandom()
    {
      for (var i = 0; i < 100; i++)
      {
        var xn = MonteCarlo.Keys(50).ToList();
        var yn = MonteCarlo.Keys(50).ToList();
        var xnNot = Enumerable.Range(0, 100).Except(xn).ToList();
        var ynNot = Enumerable.Range(0, 100).Except(yn).ToList();
        var xi = xn.AsIntervals().Not().ToList();
        var yi = yn.AsIntervals().Not().ToList();
        Assert.That(xnNot.Intersect(ynNot).Order(), Is.EqualTo(xi.And(yi).AsNumbers().TakeWhile(n => n < 100)));
        Assert.That(ynNot.Intersect(xnNot).Order(), Is.EqualTo(yi.And(xi).AsNumbers().TakeWhile(n => n < 100)));
      }
    }

    [Test]
    public static void TestAndKeys()
    {
      var a = new[] {1, 2, 2, 3};
      var b = new[] {2, 2};

      Assert.That(a.AndKeys(a), Is.EqualTo(new[] {0, 3}));
      Assert.That(b.AndKeys(b), Is.EqualTo(new[] {0, 2}));
      Assert.That(a.AndKeys(b), Is.EqualTo(new[] {1, 1}));
      Assert.That(b.AndKeys(a), Is.EqualTo(new[] {0, 1}));
    }


    [Test]
    public void TestAndKeysRandom()
    {
      for (var i = 0; i < 100; i++)
      {
        var xn = MonteCarlo.Keys(50).ToList();
        var yn = MonteCarlo.Keys(50).ToList();
        var xi = xn.AsIntervals().ToList();
        var yi = yn.AsIntervals().ToList();
        var a = xn.Intersect(yn).Order().Select(n => xn.IndexOf(n)).ToList();
        var b = xi.AndKeys(yi).AsNumbers().ToList();

        Console.WriteLine(a.ToCSV(100));
        Console.WriteLine(b.ToCSV(100));
        Console.WriteLine();
        Assert.That(a, Is.EqualTo(b));
        Assert.That(yn.Intersect(xn).Order().Select(n => yn.IndexOf(n)), Is.EqualTo(yi.AndKeys(xi).AsNumbers()));
      }
    }

    [Test]
    public void TestAndKeysOneInfiniteRandom()
    {
      for (var i = 0; i < 100; i++)
      {
        var xn = MonteCarlo.Keys(50).ToList();
        var yn = MonteCarlo.Keys(50).ToList();
        var ynNot = Enumerable.Range(0, 100).Except(yn).ToList();
        var xi = xn.AsIntervals().ToList();
        var yi = yn.AsIntervals().Not().ToList();
        Assert.That(xn.Intersect(ynNot).Order().Select(n => xn.IndexOf(n)), Is.EqualTo(xi.AndKeys(yi).AsNumbers().TakeWhile(n => n < 100)));
        Assert.That(ynNot.Intersect(xn).Order().Select(n => ynNot.IndexOf(n)), Is.EqualTo(yi.AndKeys(xi).AsNumbers().TakeWhile(n => n < 100)));
      }
    }

    [Test]
    public void TestAndKeysBothInfiniteRandom()
    {
      for (var i = 0; i < 100; i++)
      {
        var xn = MonteCarlo.Keys(50).ToList();
        var yn = MonteCarlo.Keys(50).ToList();
        var xnNot = Enumerable.Range(0, 100).Except(xn).ToList();
        var ynNot = Enumerable.Range(0, 100).Except(yn).ToList();
        var xi = xn.AsIntervals().Not().ToList();
        var yi = yn.AsIntervals().Not().ToList();
        Assert.That(xnNot.Intersect(ynNot).Order().Select(n => xnNot.IndexOf(n)).TakeWhile(n => n < 50), Is.EqualTo(xi.AndKeys(yi).AsNumbers().TakeWhile(n => n < 50)));
        Assert.That(ynNot.Intersect(xnNot).Order().Select(n => ynNot.IndexOf(n)).TakeWhile(n => n < 50), Is.EqualTo(yi.AndKeys(xi).AsNumbers().TakeWhile(n => n < 50)));
      }
    }

    [Test]
    public void TestOrKeys()
    {
      var x = new[] {1, 1, 2, 2};
      var y = new[] {2, 2, 4, 3};
      var z = new[] {0, 2, 1, 3};

      Assert.That(x.OrKeys(new[] {0, 2, 2, 3}), Is.EqualTo(new[] {0, 4, 2, 5}));
      Assert.That(y.OrKeys(new[] {1, 2, 2, 3}), Is.EqualTo(new[] {1, 4, 2, 6}));
      Assert.That(z.OrKeys(new[] {1, 2, 2, 3}), Is.EqualTo(new[] {0, 2, 1, 5, 2, 6}));
    }

    [Test]
    public void TestOrKeysRandom()
    {
      for (var i = 0; i < 100; i++)
      {
        var xn = MonteCarlo.Keys(20).ToList();
        var yn = MonteCarlo.Keys(20).ToList();
        var xi = xn.AsIntervals().ToList();
        var yi = yn.AsIntervals().ToList();

        var xnNot = Enumerable.Range(0, 40).Except(xn).OrderBy(n => n).ToList();
        var yReindex = yn.Select(y => xnNot[y]).ToList();
        var union = xn.Union(yReindex).Order<int, Int32Order>().ToList();
        Console.WriteLine("{0,30} U {1,30} = {2}", xn, yn, union);
        Console.WriteLine("{0,30} U {1,30} = {2}", xi, yi, xi.OrKeys(yi).ToList());
        Console.WriteLine();

        Assert.That(union, Is.EqualTo(xi.OrKeys(yi).AsNumbers()));
      }
    }

    [Test]
    public void TestConcat()
    {
      Assert.That(new[] {1, 2, 3}.AsIntervals().Concat(5, new int[0].AsIntervals()).AsNumbers(), Is.EqualTo(new[] {1, 2, 3}));
      Assert.That(new[] {1, 2, 3}.AsIntervals().Concat(5, new[] {2}.AsIntervals()).AsNumbers(), Is.EqualTo(new[] {1, 2, 3, 7}));
      Assert.That(new[] {1, 3}.AsIntervals().Concat(5, new[] {2, 3}.AsIntervals()).AsNumbers(), Is.EqualTo(new[] {1, 3, 7, 8}));
      Assert.That(new[] {1, 4}.AsIntervals().Concat(5, new[] {0, 1}.AsIntervals()).AsNumbers(), Is.EqualTo(new[] {1, 4, 5, 6}));
    }
  }
}