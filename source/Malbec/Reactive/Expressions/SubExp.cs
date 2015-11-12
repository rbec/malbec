using System;
using System.Collections.Generic;
using Malbec.Reactive.Subscriptions;
using Malbec.Logs;
using Malbec.Reactive.Patches;

namespace Malbec.Reactive.Expressions
{
  public sealed class SubExp<T> : BaseFunctionExp<Δ0, T, T>
  {
    private readonly IExp<Δ0, IExp<Δ0, T>> X;
    private ISub<Δ0, IExp<Δ0, T>> Sub;
    private ISub<Δ0, T> SubSub;

    public SubExp(IExp<Δ0, IExp<Δ0, T>> x)
    {
      X = x;
    }

    public override bool IsMutable => true;

    protected override Δ0 React(ref T value)
    {
      if (Sub.Δ.IsChanged)
      {
        SubSub.Dispose();
        SubSub = Sub.Value.Subscribe(this);
      }
      var log = value.Assign(SubSub.Value);
      value = log.Value;
      return log.Δ;
    }

    protected override T Initialise() => (SubSub = (Sub = X.Subscribe(this)).Value.Subscribe(this)).Value;

    public override void Dispose()
    {
      SubSub.Dispose();
      Sub.Dispose();
      base.Dispose();
    }

    public override IEnumerable<IPatch> ToPatch(T value, Δ0 δ)
    {
      throw new NotImplementedException();
    }
  }
}