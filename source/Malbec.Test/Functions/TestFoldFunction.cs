using System;
using System.Collections.Generic;
using System.Linq;
using Malbec.Functions;
using Malbec.Logs;
using NUnit.Framework;

namespace Malbec.Test.Functions
{
  public class TestFoldFunction
  {
    [Test]
    public void TestInitialise()
    {
      var f = new FoldFunction<int>(Math.Max);
      Assert.Throws<InvalidOperationException>(() => { var key = f[new int[0]]; });
      for (var i = 0; i < 100; i++)
      {
        var x = MonteCarlo.Unordered(20, MonteCarlo.Number(1, 20)).ToList();
        Assert.That(f[x], Is.EqualTo(x.Aggregate(Math.Max)));
      }
    }

    [Test]
    public void TestReactValue()
    {
      var numbers = new[] {10, 11, 12, 13, 6};
      var calls = 0;

      var f = new FoldFunction<int>((x, y) =>
      {
        calls++;
        return Math.Max(x, y);
      });

      Assert.That(f.React(13, numbers.ToList().ToLog(Δ1.Empty)), Is.EqualTo(13.ToLog(false)));
      Assert.That(calls, Is.EqualTo(0));

      Assert.That(f.React(13, numbers.ToList().Mutate(Expressions.Numbers(4).ToIns(), (key, i) => 26)), Is.EqualTo(26.ToLog(true)));
      Assert.That(calls, Is.EqualTo(5));
      calls = 0;

      Assert.That(f.React(13, numbers.ToList().Mutate(Expressions.Numbers(4).ToIns(), (key, i) => 13)), Is.EqualTo(13.ToLog(false)));
      Assert.That(calls, Is.EqualTo(5));
      calls = 0;

      Assert.That(f.React(13, numbers.ToList().Mutate(Expressions.Numbers(5).ToIns(), (key, i) => 26)), Is.EqualTo(26.ToLog(true)));
      Assert.That(calls, Is.EqualTo(1));
      calls = 0;

      Assert.That(f.React(13, numbers.ToList().Mutate(Expressions.Numbers(5).ToIns(), (key, i) => 7)), Is.EqualTo(13.ToLog(false)));
      Assert.That(calls, Is.EqualTo(1));
      calls = 0;

      Assert.That(f.React(13, numbers.ToList().Mutate(Expressions.Numbers(1).ToDel(), (key, i) => 26)), Is.EqualTo(13.ToLog(false)));
      Assert.That(calls, Is.EqualTo(3));
      calls = 0;

      Assert.That(f.React(13, numbers.ToList().Mutate(Expressions.Numbers(3).ToDel(), (key, i) => 26)), Is.EqualTo(12.ToLog(true)));
      Assert.That(calls, Is.EqualTo(3));
    }
  }
}