using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Malbec.Collections.Generic
{
  public sealed class ScanList<TItem> : IReadOnlyList<TItem>
  {
    private readonly Func<TItem, TItem, TItem> Function;
    private readonly IReadOnlyList<TItem> X;

    public ScanList(Func<TItem, TItem, TItem> f, IReadOnlyList<TItem> x)
    {
      Function = f;
      X = x;
    }

    public IEnumerator<TItem> GetEnumerator() => X.Scan(Function).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public int Count => X.Count;
    public TItem this[int index] => this.Skip(index).First();
    public override string ToString() => this.ToCSV();
  }
}