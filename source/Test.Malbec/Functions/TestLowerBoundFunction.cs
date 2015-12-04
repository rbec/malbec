using System.Linq;
using Malbec.Collections.Generic.Orderings;
using Malbec.Functions;
using Malbec.Logs;
using NUnit.Framework;

namespace Test.Malbec.Functions
{
  public class TestLowerBoundFunction
  {
    [Test]
    public void TestReact()
    {
      var f = new LowerBoundFunction<int, Int32Order>();
      var x = new[] {1, 1, 3, 5, 6, 9}.ToList();

      Assert.That(f.React(3, x.ToLog(Δ1.Empty), 4.ToLog(false)), Is.EqualTo(3.ToLog(false)));
      Assert.That(f.React(3, x.ToLog(Δ1.Empty), 4.ToLog(true)), Is.EqualTo(3.ToLog(false)));
      Assert.That(f.React(3, x.ToLog(Δ1.Empty), 5.ToLog(true)), Is.EqualTo(3.ToLog(false)));
      Assert.That(f.React(3, x.ToLog(Δ1.Empty), 3.ToLog(true)), Is.EqualTo(2.ToLog(true)));
      Assert.That(f.React(3, x.Mutate(Expressions.Numbers(5).ToSub(), (key, j) => 20), 4.ToLog(false)), Is.EqualTo(3.ToLog(false)));
      Assert.That(f.React(3, x.Mutate(Expressions.Numbers(3).ToSub(), (key, j) => 3), 4.ToLog(false)), Is.EqualTo(4.ToLog(true)));
    }

    [Test]
    public void TestReactXRandomly()
    {
      var f = new LowerBoundFunction<int, Int32Order>();

      for (var i = 0; i < 100; i++)
      {
        var x = MonteCarlo.Ordered(40, MonteCarlo.Number(20)).ToList();
        var y = MonteCarlo.Number(40);
        var v = f[x, y];

        var xLog = x.RandomMutationOrdered();
        Assert.That(f.React(v, xLog, y.ToLog(false)), Is.EqualTo(v.Assign(f[x, y])));
      }
    }

    [Test]
    public void TestReactYRandomly()
    {
      var f = new LowerBoundFunction<int, Int32Order>();

      for (var i = 0; i < 100; i++)
      {
        var x = MonteCarlo.Ordered(40, MonteCarlo.Number(20)).ToList();
        var y = MonteCarlo.Number(40);
        var v = f[x, y];
        var y2 = MonteCarlo.Number(40);
        Assert.That(f.React(v, x.ToLog(Δ1.Empty), y.Assign(y2)), Is.EqualTo(v.Assign(f[x, y2])));
      }
    }

    [Test]
    public void TestReactXYRandomly()
    {
      var f = new LowerBoundFunction<int, Int32Order>();

      for (var i = 0; i < 100; i++)
      {
        var x = MonteCarlo.Ordered(40, MonteCarlo.Number(20)).ToList();
        var y = MonteCarlo.Number(40);
        var v = f[x, y];
        var y2 = MonteCarlo.Number(40);
        var xLog = x.RandomMutationOrdered();
        Assert.That(f.React(v, xLog, y.Assign(y2)), Is.EqualTo(v.Assign(f[x, y2])));
      }
    }
  }
}