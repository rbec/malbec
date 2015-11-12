using Malbec.Functions;
using Malbec.Logs;
using NUnit.Framework;

namespace Malbec.Test.Functions
{
  public class TestChooseFunction
  {
    [Test]
    public void TestInitialise()
    {
      var f = new ChooseFunction<char>('A', 'B');
      Assert.That(f[false], Is.EqualTo('A'));
      Assert.That(f[true], Is.EqualTo('B'));
    }

    [Test]
    public void TestReact()
    {
      var f = new ChooseFunction<char>('A', 'B');
      Assert.That(f.React('A', false.ToLog(false)), Is.EqualTo('A'.ToLog(false)));
      Assert.That(f.React('A', true.ToLog(true)), Is.EqualTo('B'.ToLog(true)));
      Assert.That(f.React('B', true.ToLog(false)), Is.EqualTo('B'.ToLog(false)));
      Assert.That(f.React('B', false.ToLog(true)), Is.EqualTo('A'.ToLog(true)));
    }
  }
}