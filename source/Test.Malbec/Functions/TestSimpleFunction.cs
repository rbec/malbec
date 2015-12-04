using Malbec.Functions;
using Malbec.Logs;
using NUnit.Framework;

namespace Malbec.Test.Functions
{
  public class TestSimpleFunction
  {
    private static int Mod3(int n) => n % 3;

    [Test]
    public void TestReact()
    {
      var f = new Function<int, int>(Mod3);
      Assert.That(f[7], Is.EqualTo(1));
      Assert.That(f.React(1, 8.ToLog(true)), Is.EqualTo(2.ToLog(true)));
      Assert.That(f.React(2, 11.ToLog(true)), Is.EqualTo(2.ToLog(false)));
    }
  }
}