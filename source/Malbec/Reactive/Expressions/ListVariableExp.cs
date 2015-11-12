using System.Collections.Generic;
using Malbec.Logs;

namespace Malbec.Reactive.Expressions
{
  public sealed class ListVariableExp<TItem> : BaseVariableExp<Δ1, IReadOnlyList<TItem>, List<TItem>>
  {
    public ListVariableExp(List<TItem> value) : base(value) {}
    protected override Δ1 Apply(ref List<TItem> value, IReadOnlyList<TItem> valueItems, Δ1 δ)
    {
      var log = value.Mutate(δ, (key, i) => valueItems[i]);
      value = log.Value;
      return log.Δ;
    }
  }
}