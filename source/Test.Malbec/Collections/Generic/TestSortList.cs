using System;
using System.Linq;
using Malbec.Collections.Generic;
using Malbec.Collections.Generic.Orderings;
using NUnit.Framework;

namespace Test.Malbec.Collections.Generic
{
  public class TestSortList
  {
    [Test]
    public static void TestEnumerator()
    {
      for (var i = 0; i < 100; i++)
      {
        var x = MonteCarlo.Unordered().ToList();

        var ordered = new SortList<int, Int32Order>(x);

        Console.WriteLine("   {0}", x.ToCSV(100));
        Console.WriteLine(" = {0}", ordered);
        Console.WriteLine();

        Assert.That(ordered, Is.EqualTo(x.OrderBy(n => n)));
      }
    }

    [Test]
    public static void TestCount()
    {
      for (var i = 0; i < 100; i++)
      {
        var x = MonteCarlo.Unordered().ToList();

        var ordered = new SortList<int, Int32Order>(x);

        Console.WriteLine("   {0}", x.ToCSV(100));
        Console.WriteLine(" = {0}", ordered);
        Console.WriteLine();

        Assert.That(ordered.Count, Is.EqualTo(x.Count));
      }
    }

    [Test]
    public static void TestIndexer()
    {
      for (var i = 0; i < 100; i++)
      {
        var x = MonteCarlo.Unordered().ToList();

        var ordered = new SortList<int, Int32Order>(x);

        Console.WriteLine("   {0}", x.ToCSV(100));
        Console.WriteLine(" = {0}", ordered);
        Console.WriteLine();

        var xSorted = x.OrderBy(n => n).ToArray();
        for (var j = 0; j < x.Count; j++)
          Assert.That(ordered[j], Is.EqualTo(xSorted[j]));
      }
    }
  }
}