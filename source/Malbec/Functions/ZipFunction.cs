using System;
using System.Collections.Generic;
using System.Linq;
using Malbec.Collections;
using Malbec.Collections.Generic;
using Malbec.Logs;

namespace Malbec.Functions
{
  public sealed class ZipFunction<TXItem, TYItem, TItem> : IFunction<Δ1, Δ1, Δ1, IReadOnlyList<TXItem>, IReadOnlyList<TYItem>, ZipList<TXItem, TYItem, TItem>>
  {
    private readonly Func<TXItem, TYItem, TItem> Func;

    public ZipFunction(Func<TXItem, TYItem, TItem> func)
    {
      Func = func;
    }

    public ZipList<TXItem, TYItem, TItem> this[IReadOnlyList<TXItem> x, IReadOnlyList<TYItem> y] => new ZipList<TXItem, TYItem, TItem>(Func, x, y);
    public Log<Δ1, ZipList<TXItem, TYItem, TItem>> React(ZipList<TXItem, TYItem, TItem> value, ILog<Δ1, IReadOnlyList<TXItem>> x, ILog<Δ1, IReadOnlyList<TYItem>> y)
    {
      var sub = y.Δ.Del.SubKeys(y.Δ.Ins).ToSub();
      return value.ToLog(x.Δ.Fold(sub));
    }

    public void Dispose(ZipList<TXItem, TYItem, TItem> value) { }
  }

  public sealed class ZipCachedFunction<TXItem, TYItem, TItem> : IFunction<Δ1, Δ1, Δ1, IReadOnlyList<TXItem>, IReadOnlyList<TYItem>, List<TItem>>
  {
    private readonly Func<TXItem, TYItem, TItem> Func;

    public ZipCachedFunction(Func<TXItem, TYItem, TItem> func)
    {
      Func = func;
    }

    public List<TItem> this[IReadOnlyList<TXItem> x, IReadOnlyList<TYItem> y] => x.Zip(y, Func).ToList();
    public Log<Δ1, List<TItem>> React(List<TItem> value, ILog<Δ1, IReadOnlyList<TXItem>> x, ILog<Δ1, IReadOnlyList<TYItem>> y)
    {
      return value.Mutate(x.Δ, (index, i) => Func(x.Value[index], y.Value[index])); // TODO: dispose of old values
    }

    public void Dispose(List<TItem> value) { }
  }
}