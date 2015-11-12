namespace Malbec.Logs
{
  public struct Log<TΔ, T> : ILog<TΔ, T>
  {
    public Log(TΔ δ, T value)
    {
      Δ = δ;
      Value = value;
    }

    public T Value { get; }
    public TΔ Δ { get; }

    public override string ToString() => $"{Value} [δ = {Δ}]";
    public static implicit operator Log<TΔ, T>(T value) => new Log<TΔ, T>(default(TΔ), value);
  }
}