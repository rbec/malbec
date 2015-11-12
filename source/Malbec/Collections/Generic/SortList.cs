using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Malbec.Collections.Generic.Orderings;

namespace Malbec.Collections.Generic
{
  public sealed class SortList<TItem, TOrder> : IReadOnlyList<TItem> where TOrder : struct, IOrdering<TItem>
  {
    private readonly IReadOnlyList<TItem> X;

    public SortList(IReadOnlyList<TItem> x)
    {
      X = x;
    }

    public IEnumerator<TItem> GetEnumerator() => X.Order<TItem, TOrder>().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public int Count => X.Count;
    public TItem this[int key] => this.Skip(key).First();
    public override string ToString() => this.ToCSV();
  }
}