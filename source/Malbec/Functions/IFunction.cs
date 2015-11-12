using Malbec.Logs;

namespace Malbec.Functions
{
  public interface IFunction<in TΔX, TΔ, in TX, T>
  {
    T this[TX x] { get; }
    Log<TΔ, T> React(T value, ILog<TΔX, TX> x);
    void Dispose(T value);
  }

  public interface IFunction<in TΔX, in TΔY, TΔ, in TX, in TY, T>
  {
    T this[TX x, TY y] { get; }
    Log<TΔ, T> React(T value, ILog<TΔX, TX> x, ILog<TΔY, TY> y);
    void Dispose(T value);
  }

  public interface IFunction<in TΔX, in TΔY, in TΔZ, TΔ, in TX, in TY, in TZ, T>
  {
    T this[TX x, TY y, TZ z] { get; }
    Log<TΔ, T> React(T value, ILog<TΔX, TX> x, ILog<TΔY, TY> y, ILog<TΔZ, TZ> z);
    void Dispose(T value);
  }
}