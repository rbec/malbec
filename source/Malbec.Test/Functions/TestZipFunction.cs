using System;
using Malbec.Collections.Generic;
using Malbec.Functions;
using Malbec.Logs;
using NUnit.Framework;

namespace Malbec.Test.Functions
{
  public class TestZipFunction
  {
    [Test]
    public void TestInitialise()
    {
      var f = new ZipFunction<int, char, string>((x, y) => $"{x}-{y}");

      Assert.That(f[new int[0], new char[0]], Is.EqualTo(new string[0]));
      Assert.That(f[new int[0], new char[0]], Is.EqualTo(new string[0]));
      Assert.Throws<Exception>(() => { var value = f[new[] {10, 7}, new[] {'A'}]; });
      Assert.Throws<Exception>(() => { var value = f[new[] {10}, new[] {'A', 'B'}]; });
    }

    [Test]
    public void TestReactΔ()
    {
      var f = new ZipFunction<int, char, string>((x, y) => $"{x}{y}");

      var numbers = new[] {10, 7, 13, 6};
      var letters = new[] {'A', 'k', 'B', 'y'};

      var value = f[numbers, letters];

      Assert.That(f.React(value, numbers.ToLog(Δ.Ins(1)), letters.ToLog(Δ.Ins(1))), Is.EqualTo(value.ToLog(Δ.Ins(1))).Using(new LogComparer<ZipList<int, char, string>, string>()));
      Assert.That(f.React(value, numbers.ToLog(Δ.Ins(1)), letters.ToLog(Δ.Ins(1).Fold(Δ.Sub(3)))), Is.EqualTo(value.ToLog(Δ.Ins(1).Fold(Δ.Sub(3)))).Using(new LogComparer<ZipList<int, char, string>, string>()));
      Assert.That(f.React(value, numbers.ToLog(Δ.Ins(1).Fold(Δ.Sub(3))), letters.ToLog(Δ.Ins(1))), Is.EqualTo(value.ToLog(Δ.Ins(1).Fold(Δ.Sub(3)))).Using(new LogComparer<ZipList<int, char, string>, string>()));
    }
  }
}