﻿using System;
using System.Linq;
using Malbec.Reactive;
using Malbec.Reactive.Patches;
using Malbec.Reactive.Subscribers;

namespace Example.TimeSeries
{
  internal class Program
  {
    private static void Main()
    {
      var t = new DateTime(2015, 11, 20);
      var dates = Composition.Variable(t, t.AddDays(4), t.AddDays(9), t.AddDays(9), t.AddDays(11));
      var values = Composition.Variable(2, 4, 3, 1, 6);
      var period = Composition.Constant(t.AddDays(4), t.AddDays(10));

      var high = Composition.Fold(Math.Max, Composition.Filter(values, Composition.LowerBounds(dates, period)));
      var low = Composition.Fold(Math.Min, Composition.Filter(values, Composition.LowerBounds(dates, period)));
      var range = Composition.F((x, y) => x - y, high, low);

      using (dates.ToConsole("  dates", date => $"{date:dd/MM/yy}"))
      using (values.ToConsole(" values", item => $"{item,8}"))
      using (period.ToConsole(" period", date => $"{date:dd/MM/yy}"))
      using (high.ToConsole("   high"))
      using (low.ToConsole("    low"))
      using (range.ToConsole("  range"))
      {
        dates.Ins(2, t.AddDays(6))
          .Concat(values.Ins(2, 100))
          .Apply("Insert @ index 2");

        dates.Ins(6, t.AddDays(12))
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