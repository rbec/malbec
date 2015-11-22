using System;
using System.Linq;
using Malbec.Collections;
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

    [Test]
    public void TestRandom()
    {
      var f = new ZipFunction<int, int, int>((n, m) => n ^ m);

      var x = MonteCarlo.Unordered(1000, MonteCarlo.Number(10)).ToList();
      var y = MonteCarlo.Unordered(1000, x.Count).ToList();

      var value = f[x, y];
      var original = value.ToList();
   //   var working = original.ToList();

      var endCount = MonteCarlo.Number(10);

      Console.WriteLine("x = {0}", x.ToCSV());
      Console.WriteLine("y = {0}", y.ToCSV());
      Console.WriteLine("endCount = {0}", endCount);

      var xMinDel = Math.Max(0, x.Count - endCount);
      var yMinDel = Math.Max(0, y.Count - endCount);

      var xDelCount = MonteCarlo.Number(x.Count - xMinDel) + xMinDel;
      var yDelCount = MonteCarlo.Number(y.Count - yMinDel) + yMinDel;

      Console.WriteLine("xDelCount = {0}", xDelCount);
      Console.WriteLine("yDelCount = {0}", yDelCount);

      var δx = MonteCarlo.Δ1(x.Count, endCount, xDelCount);
      var δy = MonteCarlo.Δ1(y.Count, endCount, yDelCount);

      Console.WriteLine("dx = {0}", δx);
      Console.WriteLine("dy = {0}", δy);

      var xLog = x.Mutate(δx, (index, j) => MonteCarlo.Number(1000));
      var yLog = y.Mutate(δy, (index, j) => MonteCarlo.Number(1000));

      //Console.WriteLine("dx = {0}", xLog.Δ);
      //Console.WriteLine("dy = {0}", yLog.Δ);

      var log = f.React(value,
        xLog,
        yLog);

      Console.WriteLine("d = {0}", log.Δ);

      var k = 0;
      foreach (var key in log.Δ.Del.AsNumbers())
      {
        original.RemoveAt(key - k);
        k++;
      }

      foreach (var key in log.Δ.Ins.AsNumbers())
        original.Insert(key, value[key]);

      //Console.WriteLine(original);
      //Console.WriteLine("---------------------");
      //Console.WriteLine();

      Assert.That(original, Is.EqualTo(value));
    }
  }
}