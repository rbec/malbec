using System.Collections.Generic;
using Malbec.Graphs;

namespace Malbec.Test.Graphs
{
  public sealed class ImmutableNode : Node
  {
    public override bool IsMutable => false;
    public override bool React() => IsChanged;
    public ImmutableNode(Graph graph, int id, bool isChanged, IEnumerable<Node> args) : base(graph, id, isChanged, args) {}
  }
}