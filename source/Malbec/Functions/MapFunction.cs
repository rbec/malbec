using System;
using System.Collections.Generic;
using System.Linq;
using Malbec.Collections.Generic;
using Malbec.Logs;

namespace Malbec.Functions
{
  public sealed class MapFunction<TXItem, TItem> : IFunction<Δ1, Δ1, IReadOnlyList<TXItem>, MapList<TXItem, TItem>>
  {
    private readonly Func<TXItem, TItem> Func;

    public MapFunction(Func<TXItem, TItem> func)
    {
      Func = func;
    }

    public MapList<TXItem, TItem> this[IReadOnlyList<TXItem> x] => new MapList<TXItem, TItem>(Func, x);
    public Log<Δ1, MapList<TXItem, TItem>> React(MapList<TXItem, TItem> value, ILog<Δ1, IReadOnlyList<TXItem>> x)
    {
      return value.ToLog(x.Δ);
    }

    public void Dispose(MapList<TXItem, TItem> value) {}
  }

  public sealed class MapCachedFunction<TXItem, TItem> : IFunction<Δ1, Δ1, IReadOnlyList<TXItem>, List<TItem>>
  {
    private readonly Func<TXItem, TItem> Func;

    public MapCachedFunction(Func<TXItem, TItem> func)
    {
      Func = func;
    }

    public List<TItem> this[IReadOnlyList<TXItem> x] => x.Select(Func).ToList();
    public Log<Δ1, List<TItem>> React(List<TItem> value, ILog<Δ1, IReadOnlyList<TXItem>> x) => value.Mutate(x.Δ, (index, i) => Func(x.Value[index]));

    public void Dispose(List<TItem> value) { }
  }
}