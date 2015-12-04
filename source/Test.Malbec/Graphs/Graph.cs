using System;
using System.Collections.Generic;
using System.Linq;
using Malbec.Collections.Generic;
using Malbec.Graphs;

namespace Test.Malbec.Graphs
{
  public sealed class Graph
  {
    private int NextId;
    public readonly Dictionary<int, Node> Nodes = new Dictionary<int, Node>();
    public Node this[int id] => Nodes[id];
    public Graph Add(params int[] parents) => Add(true, parents);
    public Graph Add(bool isChanged, params int[] args) => Add(new ImmutableNode(this, NextId++, isChanged, args.Select(id => Nodes[id])));

    public Graph Add(Node node)
    {
      return this;
    }

    public Graph AddMutable(bool isChanged, int parent, int parent1) => Add(new MutableNode(this, NextId++, isChanged, Nodes[parent], Nodes[parent1]));
    public IEnumerable<IExternalNode> External => Nodes.Values.Where(node => node.Inputs.Count == 0);
    public override string ToString() => $"{Nodes.Values.OrderBy(node => node.Id).Select(x => x.ToLongString(20)).ToLines()}";
    public void Remove(int node) => Nodes.Remove(node);

    public Node Random(Node node, int count)
    {
      var pool = Nodes.Values.Where(n => n != node && n.CompareTo(node) <= 0).ToList();
      for (var i = 0; i < count; i++)
        pool.Add(new ImmutableNode(this, NextId++, true, MonteCarlo.Combination(pool, Math.Min(pool.Count + 1, 2)).Concat(pool.GetRange(pool.Count - i, i))));
      return pool.Last();
    }

    public void RandomReactions()
    {
      foreach (var node in Nodes.Values)
        node.IsChanged = MonteCarlo.Bool(.75);
    }
  }
}