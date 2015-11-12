using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Malbec.Collections.Generic
{
  public class ZipList<TXItem, TYItem, TItem> : IReadOnlyList<TItem>
  {
    private readonly Func<TXItem, TYItem, TItem> Function;
    public readonly IReadOnlyList<TXItem> X;
    public readonly IReadOnlyList<TYItem> Y;

    public ZipList(Func<TXItem, TYItem, TItem> f, IReadOnlyList<TXItem> x, IReadOnlyList<TYItem> y)
    {
      if (x.Count != y.Count)
        throw new Exception("Lists are not the same length");
      Function = f;
      X = x;
      Y = y;
    }

    public IEnumerator<TItem> GetEnumerator() => X.Zip(Y, Function).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public int Count => X.Count;
    public TItem this[int index] => Function(X[index], Y[index]);
    public override string ToString() => this.ToCSV();
  }
}