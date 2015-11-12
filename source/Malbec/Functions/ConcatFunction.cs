using System.Collections.Generic;
using Malbec.Collections.Generic;
using Malbec.Logs;

namespace Malbec.Functions
{
  public sealed class ConcatFunction<TItem> : IFunction<Δ1, Δ1, Δ1, IReadOnlyList<TItem>, IReadOnlyList<TItem>, ConcatList<TItem>>
  {
    public ConcatList<TItem> this[IReadOnlyList<TItem> x, IReadOnlyList<TItem> y] => new ConcatList<TItem>(x, y);
    public void Dispose(ConcatList<TItem> value) { }
    public Log<Δ1, ConcatList<TItem>> React(ConcatList<TItem> value, ILog<Δ1, IReadOnlyList<TItem>> x, ILog<Δ1, IReadOnlyList<TItem>> y)
      => value.ToLog(x.Δ.Concat(x.OldCount(), y.Δ, x.Value.Count));
  }
}