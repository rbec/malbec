using System.Collections.Generic;
using Malbec.Collections.Generic;
using Malbec.Collections.Generic.Orderings;
using Malbec.Logs;

namespace Malbec.Functions
{
  public sealed class LowerBoundFunction<TItem, TOrder> : IFunction<Δ1, Δ0, Δ0, IReadOnlyList<TItem>, TItem, int> where TOrder : struct, IOrdering<TItem>
  {
    public int this[IReadOnlyList<TItem> x, TItem y] => x.LowerBound<TItem, TOrder>(y);
    public void Dispose(int value) {}
    public Log<Δ0, int> React(int value, ILog<Δ1, IReadOnlyList<TItem>> x, ILog<Δ0, TItem> y) => y.Δ || (!x.Δ.IsEmpty && x.Δ.First <= value) ? value.Assign(this[x.Value, y.Value]) : value;
  }
}