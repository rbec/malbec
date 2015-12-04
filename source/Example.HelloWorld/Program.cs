using Malbec.Reactive;
using Malbec.Reactive.Patches;
using Malbec.Reactive.Subscribers;

namespace Example.HelloWorld
{
  internal class Program
  {
    private static void Main()
    {
      var var1 = Composition.Variable("Hello");
      var var2 = Composition.Variable("World");

      // Prints "Hello World!" to console
      using (Composition.F((str1, str2) => $"{str1} {str2}!", var1, var2).ToConsole())
      {
        // Prints "Goodbye World!"
        var1.Assign("Goodbye").Apply();
      }
    }
  }
}