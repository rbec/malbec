using System.Collections.Generic;

namespace Malbec.Collections.Generic.Orderings
{
  internal sealed class Comparer<T, TOrder> : Comparer<T>
    where TOrder : struct, IOrdering<T>
  {
    public override int Compare(T x, T y)
    {
      var xc = default(TOrder)[x, y];
      var yc = default(TOrder)[y, x];

      if (xc && yc) return 0;
      return xc ? -1 : 1;
    }
  }
}