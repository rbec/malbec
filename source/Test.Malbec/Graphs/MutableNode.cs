namespace Test.Malbec.Graphs
{
  public sealed class MutableNode : Node
  {
    public override bool IsMutable => true;

    public override bool React()
    {
      var length = Inputs.Count;
      Inputs.Add(Graph.Random(this, 3).Subscribe(this));

      for (var i = 1; i < length; i++)
        Inputs[i].Dispose();
      Inputs.RemoveRange(1, length - 1);
      return IsChanged;
    }

    public MutableNode(Graph graph, int id, bool isChanged, Node arg, Node arg1) : base(graph, id, isChanged, new[] {arg, arg1}) {}
  }
}