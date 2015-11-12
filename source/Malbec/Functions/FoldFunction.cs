using System;
using System.Collections.Generic;
using System.Linq;
using Malbec.Logs;

namespace Malbec.Functions
{
  public sealed class FoldFunction<TItem> : IFunction<Δ1, Δ0, IReadOnlyList<TItem>, TItem>
  {
    private readonly Func<TItem, TItem, TItem> Func;

    public FoldFunction(Func<TItem, TItem, TItem> func)
    {
      Func = func;
    }

    public TItem this[IReadOnlyList<TItem> x] => x.Aggregate(Func);

    public Log<Δ0, TItem> React(TItem value, ILog<Δ1, IReadOnlyList<TItem>> x)
    {
      if (x.Δ.IsEmpty)
        return value;
      return value.Assign(x.IsAppend()
        ? x.Value.Skip(x.Δ.Ins[0]).Aggregate(value, Func)
        : this[x.Value]);
    }

    public void Dispose(TItem value) {}
  }
}