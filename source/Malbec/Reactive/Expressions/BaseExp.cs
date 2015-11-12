using System.Collections.Generic;
using System.Linq;
using Malbec.Graphs;
using Malbec.Reactive.Patches;
using Malbec.Reactive.Subscriptions;

namespace Malbec.Reactive.Expressions
{
  public abstract class BaseExp<TΔ, T, TImpl> : IExp<TΔ, T> where TImpl : T
  {
    protected TImpl Value;
    protected TΔ Δ;
    private readonly List<Sub> Subscriptions = new List<Sub>();

    public virtual void Dispose() {}
    public virtual ISub<TΔ, T> Subscribe(INode subscriber) => new Sub(this, subscriber);
    public abstract IEnumerable<IPatch> ToPatch(T value, TΔ δ);

    private sealed class Sub : ISub<TΔ, T>
    {
      private readonly BaseExp<TΔ, T, TImpl> Exp;
      public readonly INode Subscriber;

      public Sub(BaseExp<TΔ, T, TImpl> exp, INode subscriber)
      {
        (Exp = exp).Subscriptions.Add(this);
        Subscriber = subscriber;
      }

      public void Dispose()
      {
        Exp.Subscriptions.Remove(this);
        if (Exp.Subscriptions.Count == 0)
          Exp.Dispose();
      }

      public T Value => Exp.Value;
      public TΔ Δ => Exp.Δ;
    }

    public IEnumerable<INode> Subscribers => Subscriptions.Select(subscription => subscription.Subscriber);
  }
}