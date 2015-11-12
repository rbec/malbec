using System;
using System.Linq;
using Malbec.Collections;
using Malbec.Collections.Generic;
using NUnit.Framework;

namespace Malbec.Test.Collections.Generic
{
  public class TestFilterList
  {
    [Test]
    public static void TestEnumerator()
    {
      for (var i = 0; i < 100; i++)
      {
        var items = MonteCarlo.Unordered().ToList();
        var keys = MonteCarlo.Keys(items.Count).AsIntervals().ToList();

        var filteredItems = new FilterList<int>(items, keys);

        Console.WriteLine("   {0}", items.ToCSV(100));
        Console.WriteLine(" [ {0} ]", keys);
        Console.WriteLine(" = {0}", filteredItems);
        Console.WriteLine();

        Assert.That(filteredItems, Is.EqualTo(keys.AsNumbers().Select(key => items[key])));
      }
    }

    [Test]
    public static void TestCount()
    {
      for (var i = 0; i < 100; i++)
      {
        var items = MonteCarlo.Unordered().ToList();
        var keys = MonteCarlo.Keys(items.Count).AsIntervals().ToList();

        var filteredItems = new FilterList<int>(items, keys);

        Console.WriteLine("   {0}", items.ToCSV(100));
        Console.WriteLine(" [ {0} ]", keys);
        Console.WriteLine(" = {0}", filteredItems);
        Console.WriteLine();

        Assert.That(filteredItems.Count, Is.EqualTo(keys.IntervalsCount()));
      }
    }

    [Test]
    public static void TestIndexer()
    {
      for (var i = 0; i < 100; i++)
      {
        var items = MonteCarlo.Unordered().ToList();
        var keys = MonteCarlo.Keys(items.Count).AsIntervals().ToList();

        var filteredItems = new FilterList<int>(items, keys);

        Console.WriteLine("   {0}", items.ToCSV(100));
        Console.WriteLine(" [ {0} ]", keys);
        Console.WriteLine(" = {0}", filteredItems);
        Console.WriteLine();

        for (var j = 0; j < keys.IntervalsCount(); j++)
          Assert.That(filteredItems[j], Is.EqualTo(items[keys.Value(j)]));
      }
    }
  }
}