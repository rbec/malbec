using System;
using System.Collections.Generic;
using Malbec.Reactive.Patches;

namespace Malbec.Reactive.Expressions
{
  public sealed class ConstantExp<TΔ, T> : BaseExp<TΔ, T, T>
  {
    public ConstantExp(T value)
    {
      Value = value;
    }

    public override IEnumerable<IPatch> ToPatch(T value, TΔ δ)
    {
      throw new InvalidOperationException("Cannot update constant expression");
    }

    public static implicit operator ConstantExp<TΔ, T>(T value) => new ConstantExp<TΔ, T>(value);
  }
}