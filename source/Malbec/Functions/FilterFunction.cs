using System.Collections.Generic;
using System.Linq;
using Malbec.Collections;
using Malbec.Collections.Generic;
using Malbec.Logs;

namespace Malbec.Functions
{
  public sealed class FilterFunction<TItem> : IFunction<Δ1, Δ1, Δ1, IReadOnlyList<TItem>, IReadOnlyList<int>, FilterList<TItem>>
  {
    public FilterList<TItem> this[IReadOnlyList<TItem> x, IReadOnlyList<int> y] => new FilterList<TItem>(x, y);

    public Log<Δ1, FilterList<TItem>> React(FilterList<TItem> value, ILog<Δ1, IReadOnlyList<TItem>> x, ILog<Δ1, IReadOnlyList<int>> y) // todo: tidy
    {
      var yOld = y.Δ.Ins.Not(y.Value).Or(y.Δ.Del).ToList();

      var yExpected = x.Δ.Ins.OrKeys(x.Δ.Del.Not().AndKeys(yOld)).ToList();

      var del = x.Δ.Ins.Not().AndKeys(y.Value.Not(yExpected)).ToList();
      var ins = yExpected.Not(y.Value).ToList();

      return value.ToLog(Δ1.From(
        yOld.AndKeys(x.Δ.Del.OrKeys(del)),
        y.Value.AndKeys(ins.Or(x.Δ.Ins))));
    }

    public void Dispose(FilterList<TItem> value) {}
  }
}