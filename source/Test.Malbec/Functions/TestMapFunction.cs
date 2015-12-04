using System.Linq;
using Malbec.Functions;
using Malbec.Logs;
using NUnit.Framework;

namespace Malbec.Test.Functions
{
  public class TestMapFunction
  {
    [Test]
    public void TestReactΔ()
    {
      var f = new MapFunction<int, int>(x => x + 1);

      var numbers = new[] {10, 7, 12, 13, 6};

      var value = f[numbers];

      Assert.That(f.React(value, numbers.ToList().ToLog(Δ1.Empty)).Δ, Is.EqualTo(Δ1.Empty));
      Assert.That(f.React(value, numbers.ToList().Mutate(Expressions.Numbers(4).ToIns(), (key, i) => 26)).Δ, Is.EqualTo(Expressions.Numbers(4).ToIns()));
      Assert.That(f.React(value, numbers.ToList().Mutate(Expressions.Numbers(4).ToIns(), (key, i) => 13)).Δ, Is.EqualTo(Expressions.Numbers(4).ToIns()));
      Assert.That(f.React(value, numbers.ToList().Mutate(Expressions.Numbers(5).ToIns(), (key, i) => 26)).Δ, Is.EqualTo(Expressions.Numbers(5).ToIns()));
      Assert.That(f.React(value, numbers.ToList().Mutate(Expressions.Numbers(5).ToIns(), (key, i) => 7)).Δ, Is.EqualTo(Expressions.Numbers(5).ToIns()));
      Assert.That(f.React(value, numbers.ToList().Mutate(Expressions.Numbers(1).ToDel(), (key, i) => 26)).Δ, Is.EqualTo(Expressions.Numbers(1).ToDel()));
      Assert.That(f.React(value, numbers.ToList().Mutate(Expressions.Numbers(3).ToDel(), (key, i) => 26)).Δ, Is.EqualTo(Expressions.Numbers(3).ToDel()));
    }
  }
}