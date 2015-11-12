namespace Malbec.Collections.Generic.Orderings
{
  public struct Int32Order : IOrdering<int>
  {
    public bool this[int x, int y] => x <= y;
  }
}