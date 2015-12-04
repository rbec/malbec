using System;
using System.Linq;
using Malbec.Functions;
using Malbec.Logs;
using NUnit.Framework;

namespace Test.Malbec.Functions
{
  public class TestScanFunction
  {
    [Test]
    public void TestReactΔ()
    {
      var f = new ScanFunction<int>(Math.Max);

      var numbers = new[] {10, 7, 12, 13, 6};

      var value = f[numbers];

      Assert.That(f.React(value, numbers.ToList().ToLog(Δ1.Empty)).Δ, Is.EqualTo(Δ1.Empty));
      Assert.That(f.React(value, numbers.ToList().Mutate(Expressions.Numbers(4).ToIns(), (key, i) => 26)).Δ, Is.EqualTo(Δ1.From(Expressions.Numbers(4), Expressions.Numbers(4, 5))));
      Assert.That(f.React(value, numbers.ToList().Mutate(Expressions.Numbers(4).ToIns(), (key, i) => 13)).Δ, Is.EqualTo(Δ1.From(Expressions.Numbers(4), Expressions.Numbers(4, 5))));
      Assert.That(f.React(value, numbers.ToList().Mutate(Expressions.Numbers(5).ToIns(), (key, i) => 26)).Δ, Is.EqualTo(Expressions.Numbers(5).ToIns()));
      Assert.That(f.React(value, numbers.ToList().Mutate(Expressions.Numbers(5).ToIns(), (key, i) => 7)).Δ, Is.EqualTo(Expressions.Numbers(5).ToIns()));
      Assert.That(f.React(value, numbers.ToList().Mutate(Expressions.Numbers(1).ToDel(), (key, i) => 26)).Δ, Is.EqualTo(Δ1.From(Expressions.Numbers(1, 2, 3, 4), Expressions.Numbers(1, 2, 3))));
      Assert.That(f.React(value, numbers.ToList().Mutate(Expressions.Numbers(3).ToDel(), (key, i) => 26)).Δ, Is.EqualTo(Δ1.From(Expressions.Numbers(3, 4), Expressions.Numbers(3))));
    }
  }
}