using System;
using System.Collections.Generic;
using Malbec.Collections.Generic;
using Malbec.Graphs;
using Malbec.Logs;
using Malbec.Reactive.Expressions;

namespace Malbec.Reactive.Subscribers
{
  public sealed class ConsoleSubscriber<TΔ, T> : Subscriber<TΔ, T>
  {
    private readonly Func<T, string> Formatter;
    private readonly Func<TΔ, T, string> ReactFormatter;

    public ConsoleSubscriber(IExp<TΔ, T> exp, Func<T, string> formatter, Func<TΔ, T, string> reactFormatter) : base(exp)
    {
      Formatter = formatter;
      ReactFormatter = reactFormatter;
      Console.WriteLine(Formatter(Sub.Value));
    }

    protected override void ReactChanged() => Console.WriteLine(ReactFormatter(Sub.Δ, Sub.Value));
  }

  public static class Subscribing
  {
    public static INode ToConsole<T>(this IExp<Δ0, T> x) => new ConsoleSubscriber<Δ0, T>(x, v => $"{v}", (δ, v) => $"{v}");
    public static INode ToConsole<T>(this IExp<Δ0, T> x, string name) => new ConsoleSubscriber<Δ0, T>(x, v => $"{name} = {v}", (δ, v) => $"{name} = {v}");
    public static INode ToConsole<T>(this IExp<Δ1, IReadOnlyList<T>> x, string name) => new ConsoleSubscriber<Δ1, IReadOnlyList<T>>(x, v => $"{name} = {v.ToCSV()}", (δ, v) => $"{name} = {v.ToCSV()}{Environment.NewLine}    <{δ}>");
  }
}