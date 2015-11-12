using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Malbec.Collections.Generic
{
  public class MapList<TX, TXItem, TItem> : IReadOnlyList<TItem> where TX : IReadOnlyList<TXItem>
  {
    private readonly Func<TXItem, TItem> Function;
    internal readonly TX X;

    public MapList(Func<TXItem, TItem> function, TX x)
    {
      Function = function;
      X = x;
    }

    public IEnumerator<TItem> GetEnumerator() => X.Select(Function).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public int Count => X.Count;
    public TItem this[int key] => Function(X[key]);
    public override string ToString() => this.ToCSV();
  }

  public sealed class MapList<TXItem, TItem> : MapList<IReadOnlyList<TXItem>, TXItem, TItem>
  {
    public MapList(Func<TXItem, TItem> function, IReadOnlyList<TXItem> x) : base(function, x) {}
  }
}