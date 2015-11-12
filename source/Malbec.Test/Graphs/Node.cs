using System;
using System.Collections.Generic;
using System.Linq;
using Malbec.Collections.Generic;
using Malbec.Graphs;

namespace Malbec.Test.Graphs
{
  public abstract class Node : INode
  {
    public readonly int Id;
    protected readonly Graph Graph;
    public bool IsChanged;

    protected Node(Graph graph, int id, bool isChanged, IEnumerable<Node> args)
    {
      Id = id;
      IsChanged = isChanged;
      Graph = graph;
      Graph.Nodes.Add(Id, this);
      Inputs = args.Select(node => node.Subscribe(this)).ToList();
    }

    private readonly List<Subscription> Outputs = new List<Subscription>();
    public readonly List<IDisposable> Inputs;

   // public int CompareTo(IExternalNode other) => Graphing.CompareTo(this, other);
    public virtual IDisposable Subscribe(INode subscriber) => new Subscription(this, subscriber);

    private sealed class Subscription : IDisposable
    {
      private readonly Node Node;
      public readonly INode Subscriber;

      public Subscription(Node node, INode subscriber)
      {
        (Node = node).Outputs.Add(this);
        Subscriber = subscriber;
      }

      public void Dispose()
      {
        Node.Outputs.Remove(this);
        if (Node.Outputs.Count == 0)
          Node.Dispose();
      }

      public override string ToString() => Node.ToString();
    }

    public virtual void Dispose()
    {
      foreach (var input in Inputs)
        input.Dispose();
      Graph.Remove(Id);
    }

    public IEnumerable<INode> Subscribers => Outputs.Select(subscription => subscription.Subscriber);
    public override string ToString() => $"{Id}";
    public abstract bool IsMutable { get; }
    public abstract bool React();
    public void Clear()
    {
      throw new NotImplementedException();
    }

    public string ToLongString(int padding)
    {
      var isMutableString = IsMutable ? "*" : " ";
      var outputsString = Outputs.Count == 0 ? string.Empty : $" -> {Subscribers.ToCSV()}";
      var inputsString = Inputs.Count == 0 ? string.Empty : $"{Inputs.ToCSV()} ->";
      return $"{inputsString.PadLeft(padding)}{Id,5}  {(IsChanged ? $"{isMutableString}    " : $"{isMutableString} [X]")}{outputsString}";
    }
  }
}