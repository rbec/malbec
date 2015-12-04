using System;
using System.Linq;
using Malbec.Collections;
using Malbec.Logs;
using NUnit.Framework;

namespace Test.Malbec.Δs
{
  public class TestΔ1
  {
    [Test]
    public void TestOr()
    {
      var random = new Random();

      var n = 1000;
      for (var i = 0; i < 100; i++)
      {
        var original = MonteCarlo.Keys(20).ToList();
        var working = original.ToList();

        Console.WriteLine(original);

        var δ = Δ1.Empty;
        for (var j = 0; j < 10; j++)
        {
          if (random.Next(2) == 0 && working.Count > 0)
          {
            var key = random.Next(working.Count);
            working.RemoveAt(key);
            δ = δ.Fold(Expressions.Numbers(key).ToDel());
          }
          else
          {
            var key = random.Next(working.Count + 1);
            working.Insert(key, n++);
            δ = δ.Fold(Expressions.Numbers(key).ToIns());
          }
        }

        Console.WriteLine(δ);
        Console.WriteLine(working);

        var k = 0;
        foreach (var key in δ.Del.AsNumbers())
        {
          original.RemoveAt(key - k);
          k++;
        }

        foreach (var key in δ.Ins.AsNumbers())
          original.Insert(key, working[key]);

        Console.WriteLine(original);
        Console.WriteLine("---------------------");
        Console.WriteLine();

        Assert.That(original, Is.EqualTo(working));
      }
    }
  }
}