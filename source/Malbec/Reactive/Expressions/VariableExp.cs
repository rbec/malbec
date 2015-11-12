using Malbec.Logs;

namespace Malbec.Reactive.Expressions
{
  public sealed class VariableExp<T> : BaseVariableExp<Δ0, T, T>
  {
    public VariableExp(T value) : base(value) {}

    protected override Δ0 Apply(ref T value, T valueItems, Δ0 δ)
    {
      var log = value.Assign(valueItems);
      value = log.Value;
      return log.Δ;
    }
  }
}