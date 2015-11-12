using System;
using System.Collections.Generic;
using Malbec.Collections;
using Malbec.Collections.Generic;
using Malbec.Logs;

namespace Malbec.Functions
{
  public sealed class ScanFunction<TItem> : IFunction<Δ1, Δ1, IReadOnlyList<TItem>, ScanList<TItem>>
  {
    private readonly Func<TItem, TItem, TItem> Func;

    public ScanFunction(Func<TItem, TItem, TItem> func)
    {
      Func = func;
    }

    public ScanList<TItem> this[IReadOnlyList<TItem> x] => new ScanList<TItem>(Func, x);

    public Log<Δ1, ScanList<TItem>> React(ScanList<TItem> value, ILog<Δ1, IReadOnlyList<TItem>> x)
    {
      return x.Δ.IsEmpty
        ? value
        : value.ToLog(Δ1.From(x.Δ.First.Starting().And(x.OldCount().Ending()), x.Δ.First.Starting().And(x.Value.Count.Ending())));
    }

    public void Dispose(ScanList<TItem> value) {}
  }
}