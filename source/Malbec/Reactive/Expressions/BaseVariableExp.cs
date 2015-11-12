using System.Collections.Generic;
using System.Linq;
using Malbec.Graphs;
using Malbec.Reactive.Patches;

namespace Malbec.Reactive.Expressions
{
  public abstract class BaseVariableExp<TΔ, T, TInternal> : BaseExp<TΔ, T, TInternal> where TInternal : T
  {
    protected BaseVariableExp(TInternal value)
    {
      Value = value;
    }

    protected abstract TΔ Apply(ref TInternal value, T valueItems, TΔ δ);

    public override IEnumerable<IPatch> ToPatch(T value, TΔ δ)
    {
      yield return new VariablePatch {Variable = this, Value = value, Δ = δ};
    }

    private sealed class VariablePatch : IPatch
    {
      public BaseVariableExp<TΔ, T, TInternal> Variable;
      public T Value;
      public TΔ Δ;
      public IEnumerable<INode> Process() => (Variable.Δ = Variable.Apply(ref Variable.Value, Value, Δ)).Equals(default(TΔ)) ? Enumerable.Empty<INode>() : Variable.Subscribers;
      public void Clear() => Variable.Δ = default(TΔ);
    }
  }
}