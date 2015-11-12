using System;

namespace Malbec.Logs
{
  public struct Δ0 : IEquatable<bool>
  {
    public static readonly Δ0 Unchanged = new Δ0();
    public static readonly Δ0 Changed = new Δ0(true);

    public readonly bool IsChanged;

    private Δ0(bool isChanged)
    {
      IsChanged = isChanged;
    }

    public static implicit operator bool(Δ0 δ) => δ.IsChanged;
    public static implicit operator Δ0(bool isChanged) => new Δ0(isChanged);
    public bool Equals(bool other) => other == IsChanged;
    public override string ToString() => IsChanged ? "Changed" : "Unchanged";
  }
}