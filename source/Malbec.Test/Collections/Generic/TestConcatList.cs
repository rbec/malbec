using System;
using System.Linq;
using Malbec.Collections.Generic;
using NUnit.Framework;

namespace Malbec.Test.Collections.Generic
{
  public class TestConcatList
  {
    [Test]
    public static void TestEnumerator()
    {
      for (var i = 0; i < 100; i++)
      {
        var list1 = MonteCarlo.Unordered().ToList();
        var list2 = MonteCarlo.Unordered().ToList();
        var concatList = new ConcatList<int>(list1, list2);

        Console.WriteLine("   {0}", list1.ToCSV(100));
        Console.WriteLine(" + {0}", list2.ToCSV(100));
        Console.WriteLine(" = {0}", concatList);
        Console.WriteLine();

        Assert.That(concatList, Is.EqualTo(list1.Concat(list2)));
      }
    }

    [Test]
    public static void TestCount()
    {
      for (var i = 0; i < 100; i++)
      {
        var list1 = MonteCarlo.Unordered().ToList();
        var list2 = MonteCarlo.Unordered().ToList();
        var concatList = new ConcatList<int>(list1, list2);

        Console.WriteLine("   {0}", list1.ToCSV(100));
        Console.WriteLine(" + {0}", list2.ToCSV(100));
        Console.WriteLine(" = {0}", concatList);
        Console.WriteLine();

        Assert.That(concatList.Count, Is.EqualTo(list1.Count + list2.Count));
      }
    }

    [Test]
    public static void TestIndexer()
    {
      for (var i = 0; i < 100; i++)
      {
        var list1 = MonteCarlo.Unordered().ToList();
        var list2 = MonteCarlo.Unordered().ToList();
        var concatList = new ConcatList<int>(list1, list2);

        Console.WriteLine("   {0}", list1.ToCSV(100));
        Console.WriteLine(" + {0}", list2.ToCSV(100));
        Console.WriteLine(" = {0}", concatList);
        Console.WriteLine();

        for (var j = 0; j < list1.Count; j++)
          Assert.That(concatList[j], Is.EqualTo(list1[j]));

        for (var j = 0; j < list2.Count; j++)
          Assert.That(concatList[list1.Count + j], Is.EqualTo(list2[j]));
      }
    }

  }
}