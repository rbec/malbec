# Malbec
#### Data Flow Programming in C# .NET

Malbec is a functional reactive data flow programming library. The output of a program is modelled as the value of a pure function (or functions).

Key features
* Optional memoisation.
* Lazy and partial re-evaluation of nodes in the function composition graph.
* Higher order functions allowing self-modification of the graph.

Functions are represented as vertices in a directed acyclic graph with edges representing dependencies between the ouput of a function and it's use as the argument to other functions. External vertices or 'variables' are inputs to the program and might be a file on disk, an external data stream or user input events. When external nodes are modified the changes are automatically pushed through the graph in a topologically sorted order skipping the evaluation of any function whose arguments are unchanged.

#### Example 1 - Hello World

```C#
using Malbec.Reactive.Patches;
using Malbec.Reactive.Subscribers;
using static Malbec.Reactive.Composition;

namespace HelloWorld.Example
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
```

#### Example 2 - Time Series Filter & Reduce

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using Malbec.Logs;
using Malbec.Reactive.Expressions;
using Malbec.Reactive.Patches;
using Malbec.Reactive.Subscribers;
using static System.Environment;
using static Malbec.Reactive.Composition;

namespace TimeSeries.Example
{
  internal class Program
  {
    private static void Main()
    {
      var date = new DateTime(2015, 11, 20);

      var dates = Variable(
        date,
        date.AddDays(4),
        date.AddDays(9),
        date.AddDays(9),
        date.AddDays(10),
        date.AddDays(14));

      var period = Constant(date.AddDays(4), date.AddDays(10));

      var values = Variable(2, 4, 3, 1, 6, 5);

      Console.WriteLine($"{NewLine}-------------------");
      Console.WriteLine(
        "Calculate high, low and range (high - low) for period" +
        $"[{date.AddDays(4):d} - {date.AddDays(10):d})");

      var high = High(dates, values, period);
      var low = Low(dates, values, period);
      var range = F((x, y) => x - y, high, low);

      using (high.ToConsole(nameof(high)))
      using (low.ToConsole(nameof(low)))
      using (range.ToConsole(nameof(range)))
      {
        Console.WriteLine($"{NewLine}-------------------");

        Console.WriteLine($"Inserting ({date.AddDays(6):d}, 100) @ index 2");
        dates.Ins(2, date.AddDays(6)) // high, range changed, low unchanged
          .Concat(values.Ins(2, 100))
          .Apply();
        Console.WriteLine($"{NewLine}-------------------");

        Console.WriteLine($"Inserting ({date.AddDays(12):d}, 200) @ index 6");
        dates.Ins(6, date.AddDays(12)) // all unchanged (outside of the period)
          .Concat(values.Ins(6, 200))
          .Apply();
        Console.WriteLine($"{NewLine}-------------------");

        Console.WriteLine(
          $"Deleting ({date.AddDays(6):d}, 100)," +
          $"({date.AddDays(9):d}, 3)," +
          $"({date.AddDays(9):d}, 1) starting @ index 2");
        dates.Del(2, 3) // high, low and range changed
          .Concat(values.Del(2, 3))
          .Apply();
        Console.WriteLine($"{NewLine}-------------------");

        Console.WriteLine(
          $"Substitute ({date.AddDays(4):d}, 4) with ({date.AddDays(5):d}, 150) @ index 1");
        dates.Sub(1, date.AddDays(5)) // high and low changed, range unchanged
          .Concat(values.Sub(1, 150))
          .Apply();
        Console.WriteLine($"{NewLine}-------------------");
      }
    }

    private static IExp<Δ0, int> High(
      IExp<Δ1, IReadOnlyList<DateTime>> dates,
      IExp<Δ1, IReadOnlyList<int>> values,
      IExp<Δ1, IReadOnlyList<DateTime>> period)
      => TimeSeriesFold(Math.Max, dates, values, period);

    private static IExp<Δ0, int> Low(
      IExp<Δ1, IReadOnlyList<DateTime>> dates,
      IExp<Δ1, IReadOnlyList<int>> values,
      IExp<Δ1, IReadOnlyList<DateTime>> period)
      => TimeSeriesFold(Math.Min, dates, values, period);

    private static IExp<Δ0, int> TimeSeriesFold(Func<int, int, int> func,
      IExp<Δ1, IReadOnlyList<DateTime>> dates,
      IExp<Δ1, IReadOnlyList<int>> values,
      IExp<Δ1, IReadOnlyList<DateTime>> period)
      => Fold(func, Filter(values, LowerBounds(dates, period)));
  }
}
```
