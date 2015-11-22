using System.Collections.Generic;
using System.Linq;
using Malbec.Collections;
using Malbec.Graphs;
using Malbec.Reactive.Expressions;
using Malbec.Logs;

namespace Malbec.Reactive.Patches
{
  public static class Patching
  {
    public static void Apply(params IPatch[] patches) => Apply(patches as IReadOnlyCollection<IPatch>);

    public static void Apply(this IReadOnlyCollection<IPatch> patches) // TODO: need to ensure that Variables are unique (or union edits)
    {
      var changed = Graphing.Propagate(new Nodes(patches.SelectMany(update => update.Process()).ToList())).ToList();
      foreach (var s in changed)
        s.Clear();
      foreach (var patch in patches)
        patch.Clear();
    }

    public static void Apply(this IEnumerable<IPatch> patches) => Apply(patches.ToArray());

    public static IEnumerable<IPatch> Assign<T>(this IExp<Δ0, T> expression, T value) => expression.ToPatch(value, true);


    public static IEnumerable<IPatch> Ins<TItem>(this IExp<Δ1, IReadOnlyList<TItem>> x, int key, params TItem[] items) => x.ToPatch(items, Intervals.Single(key, items.Length).ToIns());
    public static IEnumerable<IPatch> Sub<TItem>(this IExp<Δ1, IReadOnlyList<TItem>> x, int key, params TItem[] items) => x.ToPatch(items, Intervals.Single(key, items.Length).ToSub());
    public static IEnumerable<IPatch> Del<TItem>(this IExp<Δ1, IReadOnlyList<TItem>> x, int key, int count = 1) => x.ToPatch(new TItem[0], Intervals.Single(key, count).ToDel());
  }
}