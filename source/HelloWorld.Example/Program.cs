using Malbec.Reactive.Patches;
using Malbec.Reactive.Subscribers;
using static Malbec.Reactive.Composition;

namespace HelloWorld
{
  internal class Program
  {
    private static void Main()
    {
      var var1 = Variable("Hello");
      var var2 = Variable("World");

      // Prints "Hello World!" to console
      using (F((str1, str2) => $"{str1} {str2}!", var1, var2).ToConsole())
      {
        // Prints "Goodbye World!"
        var1.Assign("Goodbye").Apply();
      }
    }
  }
}