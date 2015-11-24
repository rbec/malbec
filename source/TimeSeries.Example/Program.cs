using System;
using System.Linq;
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

      var values = Variable(2, 4, 3, 1, 6, 5);
      var period = Constant(date.AddDays(4), date.AddDays(10));

      Console.WriteLine("0) Initialise");

      var high = Fold(Math.Max, Filter(values, LowerBounds(dates, period)));
      var low = Fold(Math.Min, Filter(values, LowerBounds(dates, period)));
      var range = F((x, y) => x - y, high, low);

      using (high.ToConsole($"   {nameof(high)}")) // prints 'high = 4'
      using (low.ToConsole($"    {nameof(low)}")) // prints 'low = 1'
      using (range.ToConsole($"  {nameof(range)}")) // prints 'range = 3'
      {

        Console.WriteLine($"{NewLine}1) Inserting ({date.AddDays(6):d}, 100) @ index 2");
        dates.Ins(2, date.AddDays(6))
          .Concat(values.Ins(2, 100))
          .Apply(); // prints 'high = 100  range = 99'

        Console.WriteLine($"{NewLine}2) Inserting ({date.AddDays(12):d}, 200) @ index 6");
        dates.Ins(6, date.AddDays(12))
          .Concat(values.Ins(6, 200))
          .Apply(); // prints nothing

        Console.WriteLine(
          $"{NewLine}3) Deleting ({date.AddDays(6):d}, 100), " +
          $"({date.AddDays(9):d}, 3), " +
          $"({date.AddDays(9):d}, 1) starting @ index 2");
        dates.Del(2, 3) // prints 'high = 4    low = 4  range = 0'
          .Concat(values.Del(2, 3))
          .Apply();

        Console.WriteLine(
          $"{NewLine}4) Substitute ({date.AddDays(4):d}, 4) with ({date.AddDays(5):d}, 150) @ index 1");
        dates.Sub(1, date.AddDays(5))
          .Concat(values.Sub(1, 150))
          .Apply(); // prints 'high = 150    low = 150'
      }
      Console.ReadLine();
    }
  }
}