using System.Collections.Generic;
using Malbec.Logs;

namespace Malbec.Test
{
  public sealed class LogComparer<T, TItem> : IEqualityComparer<Log<Δ1, T>>
    where T : IReadOnlyList<TItem>
  {
    public bool Equals(Log<Δ1, T> x, Log<Δ1, T> y)
    {
      if (!x.Δ.Equals(y.Δ))
        return false;

      if (x.Value.Count != y.Value.Count)
        return false;

      for (var i = 0; i < x.Value.Count; i++)
        if (!Equals(x.Value[i], y.Value[i]))
          return false;

      return true;
    }

    public int GetHashCode(Log<Δ1, T> obj)
    {
      throw new System.NotImplementedException();
    }
  }
}