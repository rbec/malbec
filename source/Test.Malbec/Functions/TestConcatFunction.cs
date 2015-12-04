using System.Linq;
using Malbec.Collections.Generic;
using Malbec.Functions;
using Malbec.Logs;
using NUnit.Framework;

namespace Test.Malbec.Functions
{
  public class TestConcatFunction
  {
    [Test]
    public void TestInitialise()
    {
      var f = new ConcatFunction<char>();
      var x = new[] {'A', 'B', 'C'};
      var y = new[] {'W', 'X', 'Y', 'Z'};
      Assert.That(f[x, y], Is.EqualTo(x.Concat(y)));
    }

    [Test]
    public void TestReactUnchanged()
    {
      var f = new ConcatFunction<char>();
      var x = new[] {'A', 'B', 'C'};
      var y = new[] {'W', 'X', 'Y', 'Z'};
      var v = f[x, y];
      Assert.That(f.React(v, x.ToLog(Δ1.Empty), y.ToLog(Δ1.Empty)), Is.EqualTo(v.ToLog(Δ1.Empty)).Using(new LogComparer<ConcatList<char>, char>()));
    }

    [Test]
    public void TestReact1()
    {
      var f = new ConcatFunction<char>();
      var x = new[] {'A', 'B', 'C'}.ToList();
      var y = new[] {'W', 'X', 'Y', 'Z'};
      var v = f[x, y];
      Assert.That(f.React(v, x.MutateIns(2, 'D'), y.ToLog(Δ1.Empty)), Is.EqualTo(v.ToLog(Δ.Ins(2))).Using(new LogComparer<ConcatList<char>, char>()));
    }

    [Test]
    public void TestReact2()
    {
      var f = new ConcatFunction<char>();
      var x = new[] {'A', 'B', 'C'}.ToList();
      var y = new[] {'W', 'X', 'Y', 'Z'}.ToList();
      var v = f[x, y];
      Assert.That(f.React(v, x.ToLog(Δ1.Empty), y.MutateIns(2, 'D')), Is.EqualTo(v.ToLog(Δ.Ins(5))).Using(new LogComparer<ConcatList<char>, char>()));
    }

    [Test]
    public void TestReact3()
    {
      var f = new ConcatFunction<char>();
      var x = new[] { 'A', 'B', 'C' }.ToList();
      var y = new[] { 'W', 'X', 'Y', 'Z' }.ToList();
      var v = f[x, y];
      Assert.That(f.React(v, x.MutateIns(2, 'D'), y.MutateIns(2, 'D')), Is.EqualTo(v.ToLog(Δ.Ins(2, 6))).Using(new LogComparer<ConcatList<char>, char>()));
    }

    [Test]
    public void TestReact4()
    {
      var f = new ConcatFunction<char>();
      var x = new[] { 'A', 'B', 'C' }.ToList();
      var y = new[] { 'W', 'X', 'Y', 'Z' }.ToList();
      var v = f[x, y];
      Assert.That(f.React(v, x.MutateDel(2), y.MutateIns(2, 'D')), Is.EqualTo(v.ToLog(Δ.Del(2).Fold(Δ.Ins(4)))).Using(new LogComparer<ConcatList<char>, char>()));
    }
  }
}