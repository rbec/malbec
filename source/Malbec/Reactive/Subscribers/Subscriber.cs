using System.Collections.Generic;
using System.Linq;
using Malbec.Graphs;
using Malbec.Reactive.Expressions;
using Malbec.Reactive.Subscriptions;

namespace Malbec.Reactive.Subscribers
{
  public abstract class Subscriber<TΔ, T> : INode
  {
    protected readonly IExp<TΔ, T> Exp;
    protected readonly ISub<TΔ, T> Sub;

    protected Subscriber(IExp<TΔ, T> exp)
    {
      Sub = (Exp = exp).Subscribe(this);
    }

    protected abstract void ReactChanged();

    public bool IsMutable => false;

    public bool React()
    {
      ReactChanged();
      return false;
    }

    public IEnumerable<INode> Subscribers => Enumerable.Empty<INode>();
    public void Clear() {}
    public void Dispose() => Sub.Dispose();
  }

  public abstract class Subscriber<TΔX, TΔY, TX, TY> : INode
  {
    protected readonly IExp<TΔX, TX> X;
    protected readonly ISub<TΔX, TX> SubX;

    protected readonly IExp<TΔY, TY> Y;
    protected readonly ISub<TΔY, TY> SubY;

    protected Subscriber(IExp<TΔX, TX> x, IExp<TΔY, TY> y)
    {
      SubX = (X = x).Subscribe(this);
      SubY = (Y = y).Subscribe(this);
    }

    protected abstract void ReactChanged();

    public bool IsMutable => false;

    public bool React()
    {
      ReactChanged();
      return false;
    }

    public IEnumerable<INode> Subscribers => Enumerable.Empty<INode>();
    public void Clear() { }
    public void Dispose()
    {
      SubX.Dispose();
      SubY.Dispose();
    }
  }
}