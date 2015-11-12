using System;
using System.Collections.Generic;
using Malbec.Collections.Generic;
using Malbec.Logs;
using Malbec.Reactive.Expressions;
using Malbec.Reactive.Patches;
using Malbec.Reactive.Subscribers;
using NUnit.Framework;

namespace Malbec.Test.Reactive
{
  public sealed class TestSubscriber<TΔ, T> : Subscriber<TΔ, T>
  {
    private readonly Func<T, string> XFormatter;
    private bool Flag;
    private TΔ ΔX;

    public TestSubscriber(IExp<TΔ, T> x, Func<T, string> xFormatter) : base(x)
    {
      XFormatter = xFormatter;
      Console.WriteLine($"{XFormatter(Sub.Value)}");
    }

    public void Assert(IEnumerable<IPatch> patchs, Log<TΔ, T> x)
    {
      Flag = x.Δ.Equals(default(TΔ));
      patchs.Apply();
      Console.WriteLine();
      Console.WriteLine($"{XFormatter(Sub.Value)}{Environment.NewLine}    <{ΔX}>");

      NUnit.Framework.Assert.True(Flag, "Didn't update as expected");
      NUnit.Framework.Assert.That(Sub.Value, Is.EqualTo(x.Value));
      NUnit.Framework.Assert.That(ΔX, Is.EqualTo(x.Δ));
      ΔX = default(TΔ);
    }

    public void Throws(IEnumerable<IPatch> patchs)
    {
      NUnit.Framework.Assert.Throws<Exception>(patchs.Apply, "Exception was not thrown");
      ΔX = default(TΔ);
    }

    protected override void ReactChanged()
    {
      Flag = !Flag;
      ΔX = Sub.Δ;
    }
  }

  public sealed class TestSubscriber<TΔX, TΔY, TX, TY> : Subscriber<TΔX, TΔY, TX, TY>
  {
    private readonly Func<TX, string> XFormatter;
    private readonly Func<TY, string> YFormatter;
    private bool Flag;
    private TΔX ΔX;
    private TΔY ΔY;

    public TestSubscriber(IExp<TΔX, TX> x, IExp<TΔY, TY> y, Func<TX, string> xFormatter, Func<TY, string> yFormatter) : base(x, y)
    {
      XFormatter = xFormatter;
      YFormatter = yFormatter;
      Console.WriteLine($"{XFormatter(SubX.Value)}");
      Console.WriteLine($"{YFormatter(SubY.Value)}");
    }

    public void Assert(IEnumerable<IPatch> patchs, Log<TΔX, TX> x, Log<TΔY, TY> y)
    {
      Flag = x.Δ.Equals(default(TΔX)) && y.Δ.Equals(default(TΔY));
      patchs.Apply();
      Console.WriteLine();
      Console.WriteLine($"{XFormatter(SubX.Value)}{Environment.NewLine}    <{ΔX}>");
      Console.WriteLine($"{YFormatter(SubY.Value)}{Environment.NewLine}    <{ΔY}>");

      NUnit.Framework.Assert.True(Flag, "Didn't update as expected");
      NUnit.Framework.Assert.That(SubX.Value, Is.EqualTo(x.Value));
      NUnit.Framework.Assert.That(ΔX, Is.EqualTo(x.Δ));
      NUnit.Framework.Assert.That(SubY.Value, Is.EqualTo(y.Value));
      NUnit.Framework.Assert.That(ΔY, Is.EqualTo(y.Δ));
      ΔX = default(TΔX);
      ΔY = default(TΔY);
    }

    protected override void ReactChanged()
    {
      Flag = !Flag;
      ΔX = SubX.Δ;
      ΔY = SubY.Δ;
    }
  }

  public static class Tests
  {
    public static TestSubscriber<Δ0, T> Subscribe<T>(string xName, IExp<Δ0, T> x) => new TestSubscriber<Δ0, T>(x, v => $"{xName} = {v}");
    public static TestSubscriber<Δ1, IReadOnlyList<T>> Subscribe<T>(string xName, IExp<Δ1, IReadOnlyList<T>> x) => new TestSubscriber<Δ1, IReadOnlyList<T>>(x, v => $"{xName} = {v.ToCSV()}");

    public static TestSubscriber<Δ0, Δ0, TX, TY> Subscribe<TX, TY>(string xName, IExp<Δ0, TX> x, string yName, IExp<Δ0, TY> y) => new TestSubscriber<Δ0, Δ0, TX, TY>(x, y, v => $"{xName} = {v}", v => $"{yName} = {v}");
    public static TestSubscriber<Δ1, Δ0, IReadOnlyList<TX>, TY> Subscribe<TX, TY>(string xName, IExp<Δ1, IReadOnlyList<TX>> x, string yName, IExp<Δ0, TY> y) => new TestSubscriber<Δ1, Δ0, IReadOnlyList<TX>, TY>(x, y, v => $"{xName} = {v.ToCSV()}", v => $"{yName} = {v}");
    public static TestSubscriber<Δ0, Δ1, TX, IReadOnlyList<TY>> Subscribe<TX, TY>(string xName, IExp<Δ0, TX> x, string yName, IExp<Δ1, IReadOnlyList<TY>> y) => new TestSubscriber<Δ0, Δ1, TX, IReadOnlyList<TY>>(x, y, v => $"{xName} = {v}", v => $"{yName} = {v.ToCSV()}");
    public static TestSubscriber<Δ1, Δ1, IReadOnlyList<TX>, IReadOnlyList<TY>> Subscribe<TX, TY>(string xName, IExp<Δ1, IReadOnlyList<TX>> x, string yName, IExp<Δ1, IReadOnlyList<TY>> y) => new TestSubscriber<Δ1, Δ1, IReadOnlyList<TX>, IReadOnlyList<TY>>(x, y, v => $"{xName} = {v.ToCSV()}", v => $"{yName} = {v.ToCSV()}");
  }
}