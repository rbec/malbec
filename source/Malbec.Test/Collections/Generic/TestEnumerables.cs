using System.Linq;
using Malbec.Collections;
using Malbec.Collections.Generic;
using Malbec.Collections.Generic.Orderings;
using NUnit.Framework;

namespace Malbec.Test.Collections.Generic
{
  public class TestEnumerables
  {
    [Test]
    public static void TestExcludePairedCardinalities()
    {
      Assert.That(Lists<int>.Empty.PostProcess(), Is.EqualTo(new int[] {}));
      Assert.That(2.Starting().PostProcess(), Is.EqualTo(new[] {2}));
      Assert.That(new[] {2, 3}.PostProcess(), Is.EqualTo(new[] {2, 3}));
      Assert.That(new[] {2, 3, 3}.PostProcess(), Is.EqualTo(new[] {2}));
      Assert.That(new[] {2, 3, 3, 3}.PostProcess(), Is.EqualTo(new[] {2, 3}));
      Assert.That(new[] {2, 3, 3, 3, 3}.PostProcess(), Is.EqualTo(new[] {2}));
      Assert.That(new[] {2, 3, 3, 3, 3, 3}.PostProcess(), Is.EqualTo(new[] {2, 3}));
      Assert.That(new[] {2, 2, 3, 3, 3, 3}.PostProcess(), Is.EqualTo(new int[] {}));
      Assert.That(new[] {2, 2, 3, 3, 3, 5}.PostProcess(), Is.EqualTo(new[] {3, 5}));
    }

    [Test]
    public static void TestOrder()
    {
      Assert.That(new int[0].Order<int, Int32Order>(), Is.EqualTo(new int[0]));
      Assert.That(new[] {1}.Order<int, Int32Order>(), Is.EqualTo(new[] {1}));

      for (var i = 0; i < 100; i++)
      {
        var items = MonteCarlo.Unordered(100).ToList();
        Assert.That(items.Order<int, Int32Order>(), Is.EqualTo(items.OrderBy(n => n)));
      }
    }

    private static int Function(int n1, int n2) => (int) ((1664525*(long) n1 + n2)%int.MaxValue);

    [Test]
    public static void TestScan()
    {
      for (var i = 0; i < 100; i++)
      {
        var x = MonteCarlo.Unordered().ToList();
        var j = 0;
        if (x.Count > 0)
        {
          var seed = x[j++];
          Assert.That(x.Scan(Function).First(), Is.EqualTo(seed));
          foreach (var item in x.Scan(Function).Skip(1))
            Assert.That(item, Is.EqualTo(seed = Function(seed, x[j++])));
        }
        Assert.That(j, Is.EqualTo(x.Count));
      }
    }
  }
}