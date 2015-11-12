using System.Collections.Generic;

namespace Malbec.Graphs
{
  public interface IExternalNode
  {
    IEnumerable<INode> Subscribers { get; }
  }
}