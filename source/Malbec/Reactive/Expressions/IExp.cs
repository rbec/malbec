using System.Collections.Generic;
using Malbec.Graphs;
using Malbec.Reactive.Patches;
using Malbec.Reactive.Subscriptions;

namespace Malbec.Reactive.Expressions
{
  public interface IExp<TΔ, T> : IExternalNode
  {
    ISub<TΔ, T> Subscribe(INode subscriber);
    IEnumerable<IPatch> ToPatch(T value, TΔ δ);
  }
}