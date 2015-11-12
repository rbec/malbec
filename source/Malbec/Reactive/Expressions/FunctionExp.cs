using System;
using System.Collections.Generic;
using Malbec.Functions;
using Malbec.Reactive.Patches;
using Malbec.Reactive.Subscriptions;

namespace Malbec.Reactive.Expressions
{
  public class FunctionExp<TΔX, TΔ, TX, T, TInternal> : BaseFunctionExp<TΔ, T, TInternal> where TInternal : T
  {
    protected IFunction<TΔX, TΔ, TX, TInternal> Function;
    private readonly IExp<TΔX, TX> X;
    private ISub<TΔX, TX> SubX;

    public FunctionExp(IFunction<TΔX, TΔ, TX, TInternal> function, IExp<TΔX, TX> x)
    {
      Function = function;
      X = x;
    }

    protected override TΔ React(ref TInternal value)
    {
      var log = Function.React(value, SubX);
      value = log.Value;
      return log.Δ;
    }

    protected override TInternal Initialise() => Function[(SubX = X.Subscribe(this)).Value];

    public override void Dispose()
    {
      Function.Dispose(Value);
      SubX.Dispose();
      base.Dispose();
    }

    public override IEnumerable<IPatch> ToPatch(T value, TΔ δ)
    {
      throw new NotImplementedException();
    }

    public override string ToString() => string.Format(Function.ToString(), X);
  }

  public class FunctionExp<TΔX, TΔY, TΔ, TX, TY, T, TInternal> : BaseFunctionExp<TΔ, T, TInternal> where TInternal : T
  {
    protected IFunction<TΔX, TΔY, TΔ, TX, TY, TInternal> Function;
    private readonly IExp<TΔX, TX> X;
    private readonly IExp<TΔY, TY> Y;
    private ISub<TΔX, TX> SubX;
    private ISub<TΔY, TY> SubY;

    public FunctionExp(IFunction<TΔX, TΔY, TΔ, TX, TY, TInternal> function, IExp<TΔX, TX> x, IExp<TΔY, TY> y)
    {
      Function = function;
      X = x;
      Y = y;
    }

    protected override TΔ React(ref TInternal value)
    {
      var log = Function.React(value, SubX, SubY);
      value = log.Value;
      return log.Δ;
    }

    protected override TInternal Initialise() => Function[(SubX = X.Subscribe(this)).Value, (SubY = Y.Subscribe(this)).Value];

    public override void Dispose()
    {
      Function.Dispose(Value);
      SubX.Dispose();
      SubY.Dispose();
      base.Dispose();
    }

    public override IEnumerable<IPatch> ToPatch(T value, TΔ δ)
    {
      throw new NotImplementedException();
    }

    public override string ToString() => string.Format(Function.ToString(), X, Y);
  }

  public class FunctionExp<TΔX, TΔY, TΔZ, TΔ, TX, TY, TZ, T, TInternal> : BaseFunctionExp<TΔ, T, TInternal> where TInternal : T
  {
    protected IFunction<TΔX, TΔY, TΔZ, TΔ, TX, TY, TZ, TInternal> Function;
    private readonly IExp<TΔX, TX> X;
    private readonly IExp<TΔY, TY> Y;
    private readonly IExp<TΔZ, TZ> Z;
    private ISub<TΔX, TX> SubX;
    private ISub<TΔY, TY> SubY;
    private ISub<TΔZ, TZ> SubZ;

    public FunctionExp(IFunction<TΔX, TΔY, TΔZ, TΔ, TX, TY, TZ, TInternal> function, IExp<TΔX, TX> x, IExp<TΔY, TY> y, IExp<TΔZ, TZ> z)
    {
      Function = function;
      X = x;
      Y = y;
      Z = z;
    }

    protected override TΔ React(ref TInternal value)
    {
      var log = Function.React(value, SubX, SubY, SubZ);
      value = log.Value;
      return log.Δ;
    }

    protected override TInternal Initialise() => Function[(SubX = X.Subscribe(this)).Value, (SubY = Y.Subscribe(this)).Value, (SubZ = Z.Subscribe(this)).Value];

    public override void Dispose()
    {
      Function.Dispose(Value);
      SubX.Dispose();
      SubY.Dispose();
      SubZ.Dispose();
      base.Dispose();
    }

    public override IEnumerable<IPatch> ToPatch(T value, TΔ δ)
    {
      throw new NotImplementedException();
    }
  }
}