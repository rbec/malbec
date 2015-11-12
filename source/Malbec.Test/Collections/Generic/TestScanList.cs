using System;
using System.Linq;
using Malbec.Collections.Generic;
using NUnit.Framework;

namespace Malbec.Test.Collections.Generic
{
  public class TestScanList
  {
    private static int Function(int n1, int n2) => (int) ((1664525*(long) n1 + n2)%int.MaxValue);

    [Test]
    public static void TestEnumerator()
    {
      for (var i = 0; i < 100; i++)
      {
        var x = MonteCarlo.Unordered().ToList();

        var scan = new ScanList<int>(Function, x);

        Console.WriteLine("   {0}", x.ToCSV(100));
        Console.WriteLine(" = {0}", scan);
        Console.WriteLine();

        Assert.That(scan, Is.EqualTo(x.Scan(Function)));
      }
    }

    [Test]
    public static void TestCount()
    {
      for (var i = 0; i < 100; i++)
      {
        var x = MonteCarlo.Unordered().ToList();

        var scan = new ScanList<int>(Function, x);

        Console.WriteLine("   {0}", x.ToCSV(100));
        Console.WriteLine(" = {0}", scan);
        Console.WriteLine();

        Assert.That(scan.Count, Is.EqualTo(x.Count));
      }
    }

    [Test]
    public static void TestIndexer()
    {
      for (var i = 0; i < 100; i++)
      {
        var x = MonteCarlo.Unordered().ToList();

        var scan = new ScanList<int>(Function, x);

        Console.WriteLine("   {0}", x.ToCSV(100));
        Console.WriteLine(" = {0}", scan);
        Console.WriteLine();

        if (x.Count > 0)
        {
          var seed = x[0];
          Assert.That(scan[0], Is.EqualTo(seed));
          for (var j = 1; j < x.Count; j++)
            Assert.That(scan[j], Is.EqualTo(seed = Function(seed, x[j])));
        }
      }
    }
  }
}