using System;

namespace Malbec.Collections.Generic.Orderings
{
  public struct DateTimeOrder : IOrdering<DateTime>
  {
    public bool this[DateTime x, DateTime y] => x <= y;
  }
}