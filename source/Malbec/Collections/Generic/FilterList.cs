using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Malbec.Collections.Generic
{
  public sealed class FilterList<TItem> : IReadOnlyList<TItem>
  {
    private readonly IReadOnlyList<TItem> X;
    private readonly IReadOnlyList<int> Y;

    public FilterList(IReadOnlyList<TItem> x, IReadOnlyList<int> y)
    {
      X = x;
      Y = y;
    }

    public IEnumerator<TItem> GetEnumerator() => Y.AsNumbers().Select(key => X[key]).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public int Count => Y.IntervalsCount();
    public TItem this[int key] => X[Y.Value(key)];
    public override string ToString() => this.ToCSV();
  }
}