# Malbec
#### Data Flow Programming in C# .NET

Malbec is a functional reactive [data flow programming](https://en.wikipedia.org/wiki/Dataflow_programming) library. The output of a program is modelled as the value of a pure function.

Key features
* Optional memoisation.
* Lazy and partial re-evaluation of nodes in the function composition graph.
* Higher order functions allowing self-modification of the graph.

Functions are represented as vertices in a [directed acyclic graph](https://en.wikipedia.org/wiki/Directed_acyclic_graph) with edges representing dependencies between the ouput of a function and it's use as the argument to other functions. External vertices or 'variables' are inputs to the program and might be a file on disk, an external data stream or user input events. When external nodes are modified the changes are automatically pushed through the graph in a topologically sorted order skipping the evaluation of any function whose arguments are unchanged.

#### Keys Concepts & Interfaces
```C#
interface INode
{
  IEnumerable<INode> Subscribers { get; }
  bool React();
}

interface IExpression<TΔ, T> : INode
{
  ISubscription<TΔ, T> Subscribe(INode subscriber);
  IEnumerable<IPatch> ToPatch(T value, TΔ δ);
}

interface ISubscription<TΔ, T> : IDisposable
{
  T Value { get; }
  TΔ Δ { get; }
}

interface IPatch
{
  IEnumerable<INode> Apply();
}
```
*Note: this is not the exact code but a slightly modified version to make the ideas clearer.*

* `INode` represents a directed acyclic graph. `Subscribers` is the nodes that depend on the value of this node. `React()` is called if any of the nodes this node depends upon change and returns whether this node has changed as a result.
* `IExpression<TΔ, T>` represents a (possibly) changing value of any type `T`. `TΔ` is a type that describes *how* the value has changed. For example an `int` may simply have a boolean flag specifying whether it *has* changed. An `IReadOnlyList<TItem>` might have `TΔ` as a data structure describing the string edits that have occurred. (Think [Edit Distance](https://en.wikipedia.org/wiki/Edit_distance) but where we are interested in the actual edits, instead of just than their number.)
* `ISubscription<TΔ, T>` allows a subscribing node access to the data in one of it's subscriptions.
* `IPatch` knows how to mutate a node or nodes in the function graph. `Apply()` effects that mutation and returns all the `INode`s that consequently need updating.

Because each node has access to *how* it's arguments have changed, rather than just the new value, it becomes possible to do a *partial* re-evaluation of a function which in some cases may be many orders of magnitude faster than a full re-evaluation.

#### Example 1 - Hello World
Defines two input variables "Hello" and "World" and defines a function that concatenates them. The output of this function is then sent to the console.

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
The first variable is then changed from "Hello" to "Goodbye" resulting in a change to the output.

#### Example 2 - Time Series: Filter & Reduce
Defines a time series consisting of a list of dates and a list of integers. Constructs functions for the high, low and range (high - low) for a specific period within the time series and outputs them to the screen.

```C#
using System;
using System.Linq;
using Malbec.Reactive.Patches;
using Malbec.Reactive.Subscribers;
using static Malbec.Reactive.Composition;

namespace TimeSeries.Example
{
  internal class Program
  {
    private static readonly DateTime Date = new DateTime(2015, 11, 20);
    private const int PAD = 12;

    private static void Main()
    {
      var dates = Variable(Date, Date.AddDays(4), Date.AddDays(9), Date.AddDays(9), Date.AddDays(11));
      var values = Variable(2, 4, 3, 1, 6);
      var period = Constant(Date.AddDays(4), Date.AddDays(10));

      var high = Fold(Math.Max, Filter(values, LowerBounds(dates, period)));
      var low = Fold(Math.Min, Filter(values, LowerBounds(dates, period)));
      var range = F((x, y) => x - y, high, low);

      using (dates.ToConsole($"{nameof(dates),PAD}", date => $"{date:dd/MM/yy}"))
      using (values.ToConsole($"{nameof(values),PAD}", item => $"{item,8}"))
      using (period.ToConsole($"{nameof(period),PAD}", date => $"{date:dd/MM/yy}"))
      using (high.ToConsole($"{nameof(high),PAD}"))
      using (low.ToConsole($"{nameof(low),PAD}"))
      using (range.ToConsole($"{nameof(range),PAD}"))
      {
        dates.Ins(2, Date.AddDays(6))
          .Concat(values.Ins(2, 100))
          .Apply("Insert @ index 2");

        dates.Ins(6, Date.AddDays(12))
          .Concat(values.Ins(6, 200))
          .Apply("Insert @ index 6");

        dates.Del(2, 3)
          .Concat(values.Del(2, 3))
          .Apply("Delete indices 2 - 4");

        values.Sub(1, 150)
          .Apply("Substitute values @ index 1");
      }
    }
  }
}
```
###### Console output
```
       dates = {20/11/15, 24/11/15, 29/11/15, 29/11/15, 01/12/15}
      values = {       2,        4,        3,        1,        6}
      period = {24/11/15, 30/11/15}
        high = 4
         low = 1
       range = 3
Insert @ index 2
       dates = {20/11/15, 24/11/15, 26/11/15, 29/11/15, 29/11/15, 01/12/15}
      values = {       2,        4,      100,        3,        1,        6}
        high = 100
       range = 99
Insert @ index 6
       dates = {20/11/15, 24/11/15, 26/11/15, 29/11/15, 29/11/15, 01/12/15, 02/12/15}
      values = {       2,        4,      100,        3,        1,        6,      200}
Delete indices 2 - 4
       dates = {20/11/15, 24/11/15, 01/12/15, 02/12/15}
      values = {       2,        4,        6,      200}
        high = 4
         low = 4
       range = 0
Substitute values @ index 1
      values = {       2,      150,        6,      200}
        high = 150
         low = 150
```
Insertions, substitutions and deletions are made to the time series resulting in changes to the high, low and range, which are lazily recalculated.
