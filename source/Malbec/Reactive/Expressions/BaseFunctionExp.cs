using Malbec.Graphs;
using Malbec.Reactive.Subscriptions;

namespace Malbec.Reactive.Expressions
{
  public abstract class BaseFunctionExp<TΔ, T, TInternal> : BaseExp<TΔ, T, TInternal>, INode where TInternal : T
  {
    private bool IsInitialised;
    protected abstract TInternal Initialise();
    protected abstract TΔ React(ref TInternal value);

    public override void Dispose()
    {
      IsInitialised = false;
      Value = default(TInternal);
      Δ = default(TΔ);
      base.Dispose();
    }

    public override ISub<TΔ, T> Subscribe(INode subscriber)
    {
      if (!IsInitialised)
      {
        Value = Initialise();
        IsInitialised = true;
      }
      return base.Subscribe(subscriber);
    }

    public virtual bool IsMutable => false;
    public virtual bool React() => !(Δ = React(ref Value)).Equals(default(TΔ));
    public void Clear() => Δ = default(TΔ);
  }
}