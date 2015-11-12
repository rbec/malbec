using System.Collections.Generic;
using Malbec.Logs;

namespace Malbec.Functions
{
  public sealed class CountFunction<TItem> : IFunction<Δ1, Δ0, IReadOnlyList<TItem>, int>
  {
    public int this[IReadOnlyList<TItem> x] => x.Count;

    public Log<Δ0, int> React(int value, ILog<Δ1, IReadOnlyList<TItem>> x)
    {
      return value.Assign(x.Value.Count);
    }

    public void Dispose(int value) { }
  }
}