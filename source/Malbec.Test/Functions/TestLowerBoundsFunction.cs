using System.Collections.Generic;
using System.Linq;
using Malbec.Collections;
using Malbec.Collections.Generic;
using Malbec.Collections.Generic.Orderings;
using Malbec.Functions;
using Malbec.Logs;
using NUnit.Framework;

namespace Malbec.Test.Functions
{
  public class TestLowerBoundsFunction
  {
    [Test]
    public void TestInitialise()
    {
      var f = new LowerBoundsFunction<int, Int32Order>();
      var x = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
      Assert.That(f[x, new[] {3, 6}], Is.EqualTo(new[] {3, 3}));
      Assert.That(f[x, new[] {3, 6, 7}], Is.EqualTo(new[] {3, 3, 4}));
      Assert.That(f[x, new[] {3, 6, 7, 9}], Is.EqualTo(new[] {3, 3, 4, 5}));
    }

    [Test]
    public void TestInitialiseRandomly()
    {
      var f = new LowerBoundsFunction<int, Int32Order>();

      for (var i = 0; i < 100; i++)
      {
        var x = MonteCarlo.Ordered(20).ToList();
        var y = MonteCarlo.Ordered(20).ToList();
        Assert.That(f[x, y], Is.EqualTo(y.Select(x.LowerBound<int, Int32Order>).ToExternal()));
      }
    }

    [Test]
    public void TestReact()
    {
      var f = new LowerBoundsFunction<int, Int32Order>();
      var x = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
      var y = new[] {3, 10}.ToList();
      var v = f[x, y];
      Assert.That(f.React(v, x.ToLog(Δ1.Empty), y.ToLog(Δ1.Empty)), Is.EqualTo(v.ToLog(Δ1.Empty)));
      Assert.That(f.React(v, x.ToLog(Δ1.Empty), y.Mutate(new[] {1, 2}.ToIns(), (i, i1) => i + 5)), Is.EqualTo(v.ToLog(new[] {6, 1}.ToDel())).Using(new LogComparer<List<int>, int>()));
    }

    [Test]
    public void TestReactRandomly()
    {
      var f = new LowerBoundsFunction<int, Int32Order>();

      for (var i = 0; i < 100; i++)
      {
        var x = MonteCarlo.Ordered(20).ToList();
        var y = MonteCarlo.Ordered(20).ToList();
        var v = f[x, y];

        var vOld = v.ToList();
        var xLog = x.RandomMutationOrdered();
        var yLog = x.RandomMutationOrdered();

        Assert.That(f.React(v, xLog, yLog), Is.EqualTo(v.ToLog(Δ1.From(v.Not(vOld), vOld.Not(v)))).Using(new LogComparer<List<int>, int>()));
      }
    }
  }
}