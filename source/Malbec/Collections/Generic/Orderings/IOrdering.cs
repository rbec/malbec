namespace Malbec.Collections.Generic.Orderings
{
  public interface IOrdering<in T>
  {
    bool this[T x, T y] { get; }
  }
}