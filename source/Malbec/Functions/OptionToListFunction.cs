using System.Collections.Generic;
using Malbec.Logs;

namespace Malbec.Functions
{
  public sealed class OptionToListFunction<T> : IFunction<Δ0, Δ1, T?, List<T>> where T : struct
  {
    public List<T> this[T? x] => x.HasValue ? new List<T> {x.Value} : new List<T>();

    public void Dispose(List<T> value) {}

    public Log<Δ1, List<T>> React(List<T> value, ILog<Δ0, T?> x)
    {
      if (!x.Δ.IsChanged)
        return value;
      if (value.Count == 0)
      {
        if (x.Value.HasValue)
        {
          value.Add(x.Value.Value);
          return value.ToLog(Δ.Ins(0));
        }
        return value;
      }
      if (x.Value.HasValue)
      {
        value[0] = x.Value.Value;
        return value.ToLog(Δ.Sub(0));
      }
      value.Clear();
      return value.ToLog(Δ.Del(0));
    }
  }
}