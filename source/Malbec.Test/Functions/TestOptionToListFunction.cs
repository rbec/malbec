using System.Collections.Generic;
using Malbec.Functions;
using Malbec.Logs;
using NUnit.Framework;

namespace Malbec.Test.Functions
{
  public class TestOptionToListFunction
  {
    [Test]
    public void TestInitialise()
    {
      var f = new OptionToListFunction<char>();
      Assert.That(f['A'], Is.EqualTo(new[] {'A'}));
      Assert.That(f[null], Is.EqualTo(new char[0]));
    }

    [Test]
    public void TestReact()
    {
      var f = new OptionToListFunction<char>();
      Assert.That(f.React(new List<char> {'A'}, ((char?) 'A').ToLog(false)), Is.EqualTo(new List<char> {'A'}.ToLog(Δ1.Empty)).Using(new LogComparer<List<char>, char>()));
      Assert.That(f.React(new List<char> {'A'}, ((char?) 'B').ToLog(true)), Is.EqualTo(new List<char> {'B'}.ToLog(Δ.Sub(0))).Using(new LogComparer<List<char>, char>()));
      Assert.That(f.React(new List<char> {'B'}, ((char?) null).ToLog(true)), Is.EqualTo(new List<char>().ToLog(Δ.Del(0))).Using(new LogComparer<List<char>, char>()));
      Assert.That(f.React(new List<char>(), ((char?) 'C').ToLog(true)), Is.EqualTo(new List<char> {'C'}.ToLog(Δ.Ins(0))).Using(new LogComparer<List<char>, char>()));
    }
  }
}