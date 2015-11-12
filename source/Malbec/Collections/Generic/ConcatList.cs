using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Malbec.Collections.Generic
{
  public sealed class ConcatList<TItem> : IReadOnlyList<TItem>
  {
    private readonly IReadOnlyList<TItem> X; 
    private readonly IReadOnlyList<TItem> Y;

    public ConcatList(IReadOnlyList<TItem> x, IReadOnlyList<TItem> y)
    {
      X = x;
      Y = y;
    }

    public IEnumerator<TItem> GetEnumerator() => X.Concat(Y).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public int Count => X.Count + Y.Count;
    public TItem this[int key] => key < X.Count ? X[key] : Y[key - X.Count];
    public override string ToString() => this.ToCSV();
  }
}