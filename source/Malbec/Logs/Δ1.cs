using System;
using System.Collections.Generic;
using System.Linq;
using Malbec.Collections;
using Malbec.Collections.Generic;

namespace Malbec.Logs
{
  public struct Δ1 : IEquatable<Δ1>
  {
    public static readonly Δ1 Empty = new Δ1();

    private readonly IReadOnlyList<int> DelInternal;
    private readonly IReadOnlyList<int> InsInternal;

    public IReadOnlyList<int> Del => DelInternal ?? Lists<int>.Empty;
    public IReadOnlyList<int> Ins => InsInternal ?? Lists<int>.Empty;

    private Δ1(IReadOnlyList<int> del, IReadOnlyList<int> ins)
    {
      DelInternal = del;
      InsInternal = ins;
    }

    public static Δ1 From(IReadOnlyList<int> del, IReadOnlyList<int> ins)
    {
      return del.Count == 0 && ins.Count == 0 ? Empty : new Δ1(del, ins);
    }

    public static Δ1 From(IEnumerable<int> del, IEnumerable<int> ins)
    {
      return From(del.ToList(), ins.ToList());
    }

    public bool IsEmpty => Del.Count == 0 && Ins.Count == 0;

    public int First
    {
      get
      {
        if (Del.Count == 0)
          return Ins[0];
        return Ins.Count == 0 ? Del[0] : Math.Min(Del[0], Ins[0]);
      }
    }

    public bool Equals(Δ1 other)
    {
      if (Del.Count != other.Del.Count || Ins.Count != other.Ins.Count)
        return false;

      for (var i = 0; i < Del.Count; i++)
        if (Del[i] != other.Del[i])
          return false;

      for (var i = 0; i < Ins.Count; i++)
        if (Ins[i] != other.Ins[i])
          return false;

      return true;
    }

    public override string ToString()
    {
      if (Del.Count == 0 && Ins.Count == 0)
        return "Unchanged";
      if (Ins.Count == 0)
        return $"Deletions: {Del.AsString()}";
      if (Del.Count == 0)
        return $"Insertions: {Ins.AsString()}";
      return Del.AsString() == Ins.AsString() ? $"Substitutions: {Ins.AsString()}" : $"Deletions: {Del.AsString()} Insertions: {Ins.AsString()}";
    }
  }

  public static class Δ
  {
    public static Δ1 Ins(params int[] keys) => keys.AsIntervals().ToIns();
    public static Δ1 Sub(params int[] keys) => keys.AsIntervals().ToSub();
    public static Δ1 Del(params int[] keys) => keys.AsIntervals().ToDel();
  }
}