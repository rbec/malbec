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

    public ConsoleSubscriber(IExp<TΔ, T> exp, Func<T, string> formatter) : base(exp)
    {
      Formatter = formatter;
      Console.WriteLine($"{Formatter(Sub.Value)}");
    }

    protected override void ReactChanged() => Console.WriteLine($"{Formatter(Sub.Value)}{Environment.NewLine}    <{Sub.Δ}>");
  }

  public static class Subscribing
  {
    public static INode ToConsole<T>(this IExp<Δ0, T> x, string xName) => new ConsoleSubscriber<Δ0, T>(x, v => $"{xName} = {v}");
    public static INode ToConsole<T>(this IExp<Δ1, IReadOnlyList<T>> x, string xName) => new ConsoleSubscriber<Δ1, IReadOnlyList<T>>(x, v => $"{xName} = {v.ToCSV()}");
  }
}