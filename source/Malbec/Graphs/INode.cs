using System;

namespace Malbec.Graphs
{
  public interface INode : IExternalNode, IDisposable
  {
    bool IsMutable { get; }
    bool React();
    void Clear();
  }
}