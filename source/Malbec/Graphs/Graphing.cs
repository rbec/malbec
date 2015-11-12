using System.Collections.Generic;
using System.Linq;

namespace Malbec.Graphs
{
  public static class Graphing
  {
    private static IEnumerable<INode> React(INode node) => node.React() ? node.Subscribers : Enumerable.Empty<INode>();

    public static IEnumerable<INode> Propagate(Nodes nodes)
    {
      while (!nodes.IsEmpty)
      {
        var node = nodes.Pop();
        yield return node;
        nodes.Push(React(node));
      }
    }

    public static IEnumerable<INode> Propagate(this IExternalNode node) => Propagate(new[] {node});
    public static IEnumerable<INode> Propagate(params IExternalNode[] nodes) => Propagate(new Nodes(nodes.SelectMany(n => n.Subscribers)));
    public static IEnumerable<INode> Propagate(this IEnumerable<IExternalNode> nodes) => Propagate(new Nodes(nodes.SelectMany(n => n.Subscribers)));

    public static IEnumerable<INode> Descendents(this IExternalNode node) => node.Subscribers.Concat(node.Subscribers.SelectMany(Descendents)).Distinct();
    public static bool IsAncestorOf(this IExternalNode x, IExternalNode y) => x.Descendents().Contains(y);

    public static int CompareTo(this IExternalNode x, IExternalNode y)
    {
      if (x.IsAncestorOf(y))
        return -1;
      if (y.IsAncestorOf(x))
        return 1;
      return 0;
    }
  }
}