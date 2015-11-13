using System;
using System.Collections.Generic;
using System.Linq;
using Malbec.Collections;
using Malbec.Collections.Generic;
using Malbec.Reactive.Subscriptions;
using Malbec.Logs;
using Malbec.Reactive.Patches;

namespace Malbec.Reactive.Expressions
{
  public sealed class MapInvokeExp<T> : BaseFunctionExp<Δ1, IReadOnlyList<T>, MapList<List<ISub<Δ0, T>>, ISub<Δ0, T>, T>>
  {
    private readonly IExp<Δ1, IReadOnlyList<IExp<Δ0, T>>> X;
    private ISub<Δ1, IReadOnlyList<IExp<Δ0, T>>> Sub;

    public MapInvokeExp(IExp<Δ1, IReadOnlyList<IExp<Δ0, T>>> x)
    {
      X = x;
    }

    public override bool IsMutable => true;

    protected override Δ1 React(ref MapList<List<ISub<Δ0, T>>, ISub<Δ0, T>, T> value)
    {
      var temp = value;
      var mutate = value.X.Mutate(Sub.Δ, (key, i) => Sub.Value[key].Subscribe(this), sub => sub.Dispose());

      var δ2 = Sub.NotIns().AsNumbers().Where(key => temp.X[key].Δ).AsIntervals().ToSub();
      
      return value.ToLog(mutate.Δ.Fold(δ2)).Δ;
    }

    protected override MapList<List<ISub<Δ0, T>>, ISub<Δ0, T>, T> Initialise() => new MapList<List<ISub<Δ0, T>>, ISub<Δ0, T>, T>(s => s.Value, (Sub = X.Subscribe(this)).Value.Select(exp => exp.Subscribe(this)).ToList());

    public override void Dispose()
    {
      foreach (var sub in Value.X)
        sub.Dispose();
      Sub.Dispose();
      base.Dispose();
    }

    public override IEnumerable<IPatch> ToPatch(IReadOnlyList<T> value, Δ1 δ)
    {
      throw new NotImplementedException();
    }
  }
}
