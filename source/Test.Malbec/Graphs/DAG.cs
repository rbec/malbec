using System.Collections.Generic;
using System.Linq;
using Malbec.Graphs;

namespace Test.Malbec.Graphs
{
  public static class DAG
  {
    public static Graph Build => new Graph();
    public static IEnumerable<INode> LazyDescendents(IExternalNode node) => node.Subscribers.Concat(node.Subscribers.Where(n => n.React()).SelectMany(LazyDescendents)).Distinct();
    public static IEnumerable<INode> LazyDescendents(params IExternalNode[] nodes) => nodes.SelectMany(LazyDescendents).Distinct();
  }
}