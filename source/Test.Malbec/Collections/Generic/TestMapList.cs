using System;
using System.Linq;
using Malbec.Collections.Generic;
using NUnit.Framework;

namespace Test.Malbec.Collections.Generic
{
  public class TestMapList
  {
    private static int Mod3(int n) => n%3;

    [Test]
    public static void TestEnumerator()
    {
      for (var i = 0; i < 100; i++)
      {
        var x = MonteCarlo.Unordered().ToList();

        var mapped = new MapList<int, int>(Mod3, x);

        Console.WriteLine("   {0}", x.ToCSV(100));
        Console.WriteLine(" = {0}", mapped);
        Console.WriteLine();

        Assert.That(mapped, Is.EqualTo(x.Select(Mod3)));
      }
    }

    [Test]
    public static void TestCount()
    {
      for (var i = 0; i < 100; i++)
      {
        var x = MonteCarlo.Unordered().ToList();

        var mapped = new MapList<int, int>(Mod3, x);

        Console.WriteLine("   {0}", x.ToCSV(100));
        Console.WriteLine(" = {0}", mapped);
        Console.WriteLine();

        Assert.That(mapped.Count, Is.EqualTo(x.Count));
      }
    }

    [Test]
    public static void TestIndexer()
    {
      for (var i = 0; i < 100; i++)
      {
        var x = MonteCarlo.Unordered().ToList();

        var mapped = new MapList<int, int>(Mod3, x);

        Console.WriteLine("   {0}", x.ToCSV(100));
        Console.WriteLine(" = {0}", mapped);
        Console.WriteLine();

        for (var j = 0; j < x.Count; j++)
          Assert.That(mapped[j], Is.EqualTo(Mod3(x[j])));
      }
    }
  }
}