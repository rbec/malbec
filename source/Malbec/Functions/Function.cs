using System;
using Malbec.Logs;

namespace Malbec.Functions
{
  public sealed class Function<TX, T> : IFunction<Δ0, Δ0, TX, T>
  {
    private readonly Func<TX, T> Func;

    public Function(Func<TX, T> func)
    {
      Func = func;
    }

    public T this[TX x] => Func(x);
    public Log<Δ0, T> React(T value, ILog<Δ0, TX> x) => x.Δ ? value.Assign(Func(x.Value)) : value;
    public void Dispose(T value) {}
  }

  public sealed class Function<TX, TY, T> : IFunction<Δ0, Δ0, Δ0, TX, TY, T>
  {
    private readonly Func<TX, TY, T> Func;

    public Function(Func<TX, TY, T> func)
    {
      Func = func;
    }

    public T this[TX x, TY y] => Func(x, y);
    public Log<Δ0, T> React(T value, ILog<Δ0, TX> x, ILog<Δ0, TY> y) => x.Δ || y.Δ ? value.Assign(Func(x.Value, y.Value)) : value;
    public void Dispose(T value) {}
  }
}