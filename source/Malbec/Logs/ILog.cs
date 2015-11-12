namespace Malbec.Logs
{
  public interface ILog<out TΔ, out T>
  {
    T Value { get; }
    TΔ Δ { get; }
  }
}