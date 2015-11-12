using System.Collections.Generic;
using System.Linq;
using Malbec.Collections;
using Malbec.Collections.Generic;
using Malbec.Collections.Generic.Orderings;
using Malbec.Logs;

namespace Malbec.Functions
{
  public sealed class LowerBoundsFunction<TItem, TOrder> : IFunction<Δ1, Δ1, Δ1, IReadOnlyList<TItem>, IReadOnlyList<TItem>, List<int>> where TOrder : struct, IOrdering<TItem>
  {
    public List<int> this[IReadOnlyList<TItem> x, IReadOnlyList<TItem> y] => y.Select(x.LowerBound<TItem, TOrder>).ToExternal().ToList();
    public void Dispose(List<int> value) {}

    public Log<Δ1, List<int>> React(List<int> value, ILog<Δ1, IReadOnlyList<TItem>> x, ILog<Δ1, IReadOnlyList<TItem>> y)
    {
      if (x.Δ.IsEmpty && y.Δ.IsEmpty)
        return value;

      var copy = value.ToList();
      value.Clear();

      value.AddRange(y.Value.Select(x.Value.LowerBound<TItem, TOrder>).ToExternal());

      var δ1 = Δ1.From(value.Not(copy), copy.Not(value));

      return value.ToLog(δ1);
    }
  }
}