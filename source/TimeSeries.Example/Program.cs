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
    private static readonly DateTime Date = new DateTime(2015, 11, 20);

    private static void Main()
    {
      var dates = Variable(
        Date,
        Date.AddDays(4),
        Date.AddDays(9),
        Date.AddDays(9),
        Date.AddDays(10),
        Date.AddDays(14));

      var values = Variable(2, 4, 3, 1, 6, 5);
      var period = Constant(Date.AddDays(4), Date.AddDays(10));

      var high = Fold(Math.Max, Filter(values, LowerBounds(dates, period)));
      var low = Fold(Math.Min, Filter(values, LowerBounds(dates, period)));
      var range = F((x, y) => x - y, high, low);

      using (dates.ToConsole($"{nameof(dates),7}", date => $"{date:d}"))
      using (values.ToConsole($"{nameof(values),7}", item => $"{item,10}"))
      using (period.ToConsole($"{nameof(period),7}", date => $"{date:d}"))
      using (high.ToConsole($"{nameof(high),7}"))
      using (low.ToConsole($"{nameof(low),7}"))
      using (range.ToConsole($"{nameof(range),7}"))
      {
        Console.WriteLine($"{NewLine}Insert @ index 2");
        dates.Ins(2, Date.AddDays(6))
          .Concat(values.Ins(2, 100))
          .Apply();

        Console.WriteLine($"{NewLine}Insert @ index 6");
        dates.Ins(6, Date.AddDays(12))
          .Concat(values.Ins(6, 200))
          .Apply();

        Console.WriteLine($"{NewLine}Delete indices 2 - 4");
        dates.Del(2, 3)
          .Concat(values.Del(2, 3))
          .Apply();

        Console.WriteLine($"{NewLine}Substitute values @ index 1");
        values.Sub(1, 150)
          .Apply();
      }
      Console.WriteLine();
    }
  }
}