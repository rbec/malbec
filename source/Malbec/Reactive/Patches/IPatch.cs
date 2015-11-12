using System.Collections.Generic;
using Malbec.Graphs;

namespace Malbec.Reactive.Patches
{
  public interface IPatch
  {
    IEnumerable<INode> Process();
    void Clear();
  }
}