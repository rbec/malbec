using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Malbec.Graphs
{
  public sealed class Nodes : IEnumerable<INode>
  {
    public readonly List<INode> Immutable = new List<INode>();
    public readonly List<INode> Mutable = new List<INode>();

    public Nodes(IEnumerable<INode> nodes)
    {
      Push(nodes);
    }

    public void Push(IEnumerable<INode> nodes)
    {
      foreach (var node in nodes)
        if (node.IsMutable)
        {
          if (!Mutable.Contains(node))
            Mutable.Add(node);
        }
        else if (!Immutable.Contains(node))
          Immutable.Add(node);
    }

    public INode Pop()
    {
      INode found = null;
      foreach (var node in Immutable)
        if (this.All(n => n.CompareTo(node) >= 0))
        {
          found = node;
          break;
        }

      if (found != null)
      {
        Immutable.Remove(found);
        return found;
      }
      foreach (var node in Mutable)
        if (this.All(n => n.CompareTo(node) >= 0))
        {
          found = node;
          break;
        }

      if (found != null)
      {
        Mutable.Remove(found);
        return found;
      }
      throw new Exception();
    }

    public IEnumerator<INode> GetEnumerator() => Immutable.Concat(Mutable).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public bool IsEmpty => Immutable.Count == 0 && Mutable.Count == 0;
  }
}