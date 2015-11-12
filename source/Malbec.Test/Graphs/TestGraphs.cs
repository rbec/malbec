using System;
using System.Collections.Generic;
using System.Linq;
using Malbec.Collections.Generic;
using Malbec.Graphs;
using NUnit.Framework;
using static System.Environment;

namespace Malbec.Test.Graphs
{
  public class TestGraphs
  {
    public sealed class NodeComparer : Comparer<IExternalNode>
    {
      public override int Compare(IExternalNode x, IExternalNode y) => x.CompareTo(y);
    }

    public static IComparer<IExternalNode> Comparer = new NodeComparer();

    [Test]
    public void TestIsAncestorOf()
    {
      var g = DAG.Build
        .Add()
        .Add(0)
        .Add(1)
        .Add(2, 0)
        .Add(2, 3)
        .Add(4, 0);

      Assert.False(g[0].IsAncestorOf(g[0]));
      Assert.True(g[0].IsAncestorOf(g[1]));
      Assert.True(g[0].IsAncestorOf(g[2]));
      Assert.True(g[0].IsAncestorOf(g[3]));
      Assert.True(g[0].IsAncestorOf(g[4]));
      Assert.True(g[0].IsAncestorOf(g[5]));

      Assert.False(g[1].IsAncestorOf(g[0]));
      Assert.False(g[1].IsAncestorOf(g[1]));
      Assert.True(g[1].IsAncestorOf(g[2]));
      Assert.True(g[1].IsAncestorOf(g[3]));
      Assert.True(g[1].IsAncestorOf(g[4]));
      Assert.True(g[1].IsAncestorOf(g[5]));

      Assert.False(g[2].IsAncestorOf(g[0]));
      Assert.False(g[2].IsAncestorOf(g[1]));
      Assert.False(g[2].IsAncestorOf(g[2]));
      Assert.True(g[2].IsAncestorOf(g[3]));
      Assert.True(g[2].IsAncestorOf(g[4]));
      Assert.True(g[2].IsAncestorOf(g[5]));

      Assert.False(g[3].IsAncestorOf(g[0]));
      Assert.False(g[3].IsAncestorOf(g[1]));
      Assert.False(g[3].IsAncestorOf(g[2]));
      Assert.False(g[3].IsAncestorOf(g[3]));
      Assert.True(g[3].IsAncestorOf(g[4]));
      Assert.True(g[3].IsAncestorOf(g[5]));

      Assert.False(g[4].IsAncestorOf(g[0]));
      Assert.False(g[4].IsAncestorOf(g[1]));
      Assert.False(g[4].IsAncestorOf(g[2]));
      Assert.False(g[4].IsAncestorOf(g[3]));
      Assert.False(g[4].IsAncestorOf(g[4]));
      Assert.True(g[4].IsAncestorOf(g[5]));

      Assert.False(g[5].IsAncestorOf(g[0]));
      Assert.False(g[5].IsAncestorOf(g[1]));
      Assert.False(g[5].IsAncestorOf(g[2]));
      Assert.False(g[5].IsAncestorOf(g[3]));
      Assert.False(g[5].IsAncestorOf(g[4]));
      Assert.False(g[5].IsAncestorOf(g[5]));
    }

    private static void TestReaction(bool verbose, Graph graph, IReadOnlyCollection<IExternalNode> expected, params IExternalNode[] nodes)
    {
      var external = graph.External.ToList();
      var result = nodes.Propagate().ToList();
      if (verbose)
      {
        Console.WriteLine();
        Console.WriteLine("------------------------------------------------------------");
        Console.WriteLine($"Test Reaction (Nodes = {graph.Nodes.Count} External = {external.Count})");
        Console.WriteLine("------------------------------------------------------------");
        Console.WriteLine(graph);
        Console.WriteLine();
        Console.WriteLine($"Change     {nodes.ToCSV()}");
        Console.WriteLine($"        -> {result.ToCSV()}");
      }

      Assert.That(result, Is.Ordered.Using(Comparer), $"Bad order: {result.ToCSV()}");
      Assert.That(result, Is.EquivalentTo(expected), $"{result.ToCSV()} != {expected.ToCSV()}{NewLine}Graph{NewLine}{graph}{NewLine}Request{NewLine}{nodes.ToCSV()}");
    }

    private static void TestReaction(bool verbose, Graph graph, IEnumerable<IExternalNode> expected, params IExternalNode[] nodes)
    {
      TestReaction(verbose, graph, expected as IReadOnlyCollection<IExternalNode> ?? expected.ToArray(), nodes);
    }

    private static void TestReaction(bool verbose, Graph graph, params IExternalNode[] nodes)
    {
      TestReaction(verbose, graph, DAG.LazyDescendents(nodes), nodes);
    }

    [Test]
    public void TestPropagate1()
    {
      var g = DAG.Build
        .Add()
        .Add()
        .Add(0)
        .Add(1);

      TestReaction(false, g, new[] {g[2]}, g[0]);
      TestReaction(false, g, new[] {g[3]}, g[1]);
    }

    [Test]
    public void TestPropagate2()
    {
      var g = DAG.Build
        .Add()
        .Add()
        .Add(0)
        .Add(0, 1);

      TestReaction(false, g, new[] {g[2], g[3]}, g[0]);
      TestReaction(false, g, new[] {g[3]}, g[1]);
      TestReaction(false, g, new[] {g[2], g[3]}, g[0], g[1]);
      TestReaction(false, g, new[] {g[2], g[3]}, g[1], g[0]);
    }

    [Test]
    public void TestPropagate3()
    {
      var g = DAG.Build
        .Add()
        .Add(false, 0)
        .Add(1)
        .Add(2, 0)
        .Add(2, 3)
        .Add(2, 0);

      TestReaction(false, g, new[] {g[1], g[3], g[4], g[5]}, g[0]);
    }

    [Test]
    public void TestPropagate4()
    {
      var g = DAG.Build
        .Add()
        .Add(false, 0)
        .Add(1)
        .Add(2, 0)
        .Add(2, 3)
        .Add(4, 0);

      TestReaction(false, g, new[] {g[1], g[3], g[4], g[5]}, g[0]);
    }

    [Test]
    public void TestPropagate5()
    {
      var g = DAG.Build
        .Add()
        .Add(0)
        .Add(1)
        .Add(2, 0)
        .Add(2, 3)
        .Add(4, 0);

      TestReaction(false, g, new[] {g[1], g[2], g[3], g[4], g[5]}, g[0]);
    }

    [Test]
    public void TestPropagate6()
    {
      var g = DAG.Build
        .Add()
        .Add(0)
        .Add(0)
        .Add(2)
        .Add(1)
        .Add(3, 4);

      TestReaction(false, g, new[] {g[1], g[2], g[3], g[4], g[5]}, g[0]);
    }

    [Test]
    public void TestPropagateRandom()
    {
      for (var i = 0; i < 50; i++)
      {
        var g = MonteCarlo.Graph(i);
        var external = g.External.ToList();
        for (var j = 0; j <= external.Count; j++)
          TestReaction(false, g, MonteCarlo.Combination(external, j).ToArray());
      }
    }

    [Test]
    public void TestMutate()
    {
      var g = DAG.Build
         .Add()
         .Add(0)
         .Add(1)
         .Add(2)
         .Add(1)
         .AddMutable(true, 2, 4)
         .Add(4)
         .Add(0);

      g.AddMutable(true, 3, 5)
        .Add(8)
        .Add(9, 1, 3, 6);

      //for (var i = 0; i < 10; i++)
      //{
      //  Console.WriteLine("----------------------");
      //  Console.WriteLine(i);
      //  Console.WriteLine("----------------------");

      //  Console.WriteLine(g);
      //  Console.WriteLine();

      //  Console.WriteLine(new[] {g[0]}.Propagate().ToCSV());
      //  Console.WriteLine();


      //  Console.WriteLine();
       
      //}

      //Console.WriteLine("----------------------");
      //Console.WriteLine("Final");
      //Console.WriteLine("----------------------");

      //Console.WriteLine(g);
    }
  }
}