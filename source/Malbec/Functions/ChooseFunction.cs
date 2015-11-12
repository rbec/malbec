using Malbec.Logs;

namespace Malbec.Functions
{
  public sealed class ChooseFunction<T> : IFunction<Δ0, Δ0, bool, T>
  {
    private readonly T A;
    private readonly T B;

    public ChooseFunction(T a, T b)
    {
      A = a;
      B = b;
    }

    public T this[bool x] => x ? B : A;
    public Log<Δ0, T> React(T value, ILog<Δ0, bool> x) => value.Assign(this[x.Value]);
    public void Dispose(T value) {}
  }
}