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