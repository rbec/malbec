using System;
using System.Collections.Generic;
using System.Linq;
using Malbec.Collections.Generic;
using Malbec.Collections.Generic.Orderings;
using Malbec.Functions;
using Malbec.Reactive.Expressions;
using Malbec.Logs;

namespace Malbec.Reactive
{
  public static class Composition
  {
    public static IExp<Δ0, T> Variable<T>(T value)
      => new VariableExp<T>(value);

    public static IExp<Δ1, IReadOnlyList<TItem>> Variable<TItem>(params TItem[] value)
      => new ListVariableExp<TItem>(value.ToList());

    public static IExp<Δ0, T> Constant<T>(T value)
      => new ConstantExp<Δ0, T>(value);

    public static IExp<Δ1, IReadOnlyList<TItem>> Constant<TItem>(params TItem[] value)
      => new ConstantExp<Δ1, IReadOnlyList<TItem>>(value);

    public static IExp<Δ0, T> F<TΔX, TX, T>(IFunction<TΔX, Δ0, TX, T> f, IExp<TΔX, TX> x)
      => new FunctionExp<TΔX, Δ0, TX, T, T>(f, x);

    public static IExp<Δ0, T> F<TΔX, TΔY, TX, TY, T>(IFunction<TΔX, TΔY, Δ0, TX, TY, T> f, IExp<TΔX, TX> x, IExp<TΔY, TY> y)
      => new FunctionExp<TΔX, TΔY, Δ0, TX, TY, T, T>(f, x, y);

    public static IExp<Δ1, IReadOnlyList<TItem>> F<TΔX, TX, TItem, TInternal>(IFunction<TΔX, Δ1, TX, TInternal> f, IExp<TΔX, TX> x) where TInternal : IReadOnlyList<TItem>
      => new FunctionExp<TΔX, Δ1, TX, IReadOnlyList<TItem>, TInternal>(f, x);

    public static IExp<Δ1, IReadOnlyList<TItem>> F<TΔX, TΔY, TX, TY, TItem, TInternal>(IFunction<TΔX, TΔY, Δ1, TX, TY, TInternal> f, IExp<TΔX, TX> x, IExp<TΔY, TY> y) where TInternal : IReadOnlyList<TItem>
      => new FunctionExp<TΔX, TΔY, Δ1, TX, TY, IReadOnlyList<TItem>, TInternal>(f, x, y);

    public static IExp<Δ1, IReadOnlyList<TItem>> F<TΔX, TΔY, TΔZ, TX, TY, TZ, TItem, TInternal>(IFunction<TΔX, TΔY, TΔZ, Δ1, TX, TY, TZ, TInternal> f, IExp<TΔX, TX> x, IExp<TΔY, TY> y, IExp<TΔZ, TZ> z) where TInternal : IReadOnlyList<TItem>
      => new FunctionExp<TΔX, TΔY, TΔZ, Δ1, TX, TY, TZ, IReadOnlyList<TItem>, TInternal>(f, x, y, z);

    public static IExp<Δ0, T> F<TX, T>(Func<TX, T> f, IExp<Δ0, TX> x)
      => new FunctionExp<Δ0, Δ0, TX, T, T>(new Function<TX, T>(f), x);

    public static IExp<Δ0, T> F<TX, TY, T>(Func<TX, TY, T> f, IExp<Δ0, TX> x, IExp<Δ0, TY> y)
      => new FunctionExp<Δ0, Δ0, Δ0, TX, TY, T, T>(new Function<TX, TY, T>(f), x, y);

    public static IExp<Δ0, TItem> Fold<TItem>(Func<TItem, TItem, TItem> f, IExp<Δ1, IReadOnlyList<TItem>> x)
      => F(new FoldFunction<TItem>(f), x);

    public static IExp<Δ1, IReadOnlyList<TItem>> Scan<TItem>(Func<TItem, TItem, TItem> f, IExp<Δ1, IReadOnlyList<TItem>> x)
      => F<Δ1, IReadOnlyList<TItem>, TItem, ScanList<TItem>>(new ScanFunction<TItem>(f), x);

    public static IExp<Δ1, IReadOnlyList<TItem>> Map<TXItem, TItem>(Func<TXItem, TItem> f, IExp<Δ1, IReadOnlyList<TXItem>> x)
      => F<Δ1, IReadOnlyList<TXItem>, TItem, MapList<TXItem, TItem>>(new MapFunction<TXItem, TItem>(f), x);

    public static IExp<Δ1, IReadOnlyList<TItem>> MapCached<TXItem, TItem>(Func<TXItem, TItem> f, IExp<Δ1, IReadOnlyList<TXItem>> x)
      => F<Δ1, IReadOnlyList<TXItem>, TItem, List<TItem>>(new MapCachedFunction<TXItem, TItem>(f), x);

    public static IExp<Δ1, IReadOnlyList<TItem>> Zip<TXItem, TYItem, TItem>(Func<TXItem, TYItem, TItem> f, IExp<Δ1, IReadOnlyList<TXItem>> x, IExp<Δ1, IReadOnlyList<TYItem>> y)
      => F<Δ1, Δ1, IReadOnlyList<TXItem>, IReadOnlyList<TYItem>, TItem, ZipList<TXItem, TYItem, TItem>>(new ZipFunction<TXItem, TYItem, TItem>(f), x, y);

    public static IExp<Δ1, IReadOnlyList<TItem>> ZipCached<TXItem, TYItem, TItem>(Func<TXItem, TYItem, TItem> f, IExp<Δ1, IReadOnlyList<TXItem>> x, IExp<Δ1, IReadOnlyList<TYItem>> y)
      => F<Δ1, Δ1, IReadOnlyList<TXItem>, IReadOnlyList<TYItem>, TItem, List<TItem>>(new ZipCachedFunction<TXItem, TYItem, TItem>(f), x, y);

    public static IExp<Δ1, IReadOnlyList<TItem>> Concat<TItem>(IExp<Δ1, IReadOnlyList<TItem>> x, IExp<Δ1, IReadOnlyList<TItem>> y)
      => F<Δ1, Δ1, IReadOnlyList<TItem>, IReadOnlyList<TItem>, TItem, ConcatList<TItem>>(new ConcatFunction<TItem>(), x, y);

    public static IExp<Δ0, T> Invoke<T>(IExp<Δ0, IExp<Δ0, T>> x)
      => new InvokeExp<T>(x);

    public static IExp<Δ1, IReadOnlyList<TItem>> MapInvoke<TItem>(IExp<Δ1, IReadOnlyList<IExp<Δ0, TItem>>> x)
      => new MapInvokeExp<TItem>(x);

    public static IExp<Δ1, IReadOnlyList<TItem>> Filter<TItem>(IExp<Δ1, IReadOnlyList<TItem>> x, IExp<Δ1, IReadOnlyList<int>> y)
      => F<Δ1, Δ1, IReadOnlyList<TItem>, IReadOnlyList<int>, TItem, FilterList<TItem>>(new FilterFunction<TItem>(), x, y);

    public static IExp<Δ0, int> LowerBound<TItem, TOrder>(IExp<Δ1, IReadOnlyList<TItem>> x, IExp<Δ0, TItem> y) where TOrder : struct, IOrdering<TItem>
      => F(new LowerBoundFunction<TItem, TOrder>(), x, y);

    public static IExp<Δ1, IReadOnlyList<int>> LowerBounds<TItem, TOrder>(IExp<Δ1, IReadOnlyList<TItem>> x, IExp<Δ1, IReadOnlyList<TItem>> y) where TOrder : struct, IOrdering<TItem>
      => F<Δ1, Δ1, IReadOnlyList<TItem>, IReadOnlyList<TItem>, int, List<int>>(new LowerBoundsFunction<TItem, TOrder>(), x, y);

    public static IExp<Δ1, IReadOnlyList<int>> LowerBounds(IExp<Δ1, IReadOnlyList<int>> x, IExp<Δ1, IReadOnlyList<int>> y)
      => LowerBounds<int, Int32Order>(x, y);

    public static IExp<Δ1, IReadOnlyList<int>> LowerBounds(IExp<Δ1, IReadOnlyList<DateTime>> x, IExp<Δ1, IReadOnlyList<DateTime>> y)
      => LowerBounds<DateTime, DateTimeOrder>(x, y);

    public static IExp<Δ0, int> LowerBound(IExp<Δ1, IReadOnlyList<int>> x, IExp<Δ0, int> y)
      => LowerBound<int, Int32Order>(x, y);

    public static IExp<Δ0, int> Add(IExp<Δ0, int> x, IExp<Δ0, int> y)
      => new FunctionExp<Δ0, Δ0, Δ0, int, int, int, int>(new Function<int, int, int>((a, b) => a + b), x, y);

    public static IExp<Δ1, IReadOnlyList<TItem>> File<TItem>(string path, int headerSize, int recordSize) where TItem : struct
      => new ConstantExp<Δ1, IReadOnlyList<TItem>>(new FileList<TItem>(path, headerSize, recordSize));

    public static T? AsNullable<T>(T x) where T : struct
      => x;

    public static IExp<Δ0, T?> Option<T>(IExp<Δ0, bool> x, IExp<Δ0, T> y) where T : struct
      => Invoke(Choose(new ConstantExp<Δ0, T?>(null), F(AsNullable, y), x));

    public static IExp<Δ1, IReadOnlyList<T>> ℱOptionToList<T>(IExp<Δ0, T?> x) where T : struct
      => F<Δ0, T?, T, List<T>>(new OptionToListFunction<T>(), x);

    public static IExp<Δ0, T> Choose<T>(T x, T y, IExp<Δ0, bool> z)
      => F(new ChooseFunction<T>(x, y), z);

    public static IExp<Δ0, int> Count<T>(IExp<Δ1, IReadOnlyList<T>> z)
      => F(new CountFunction<T>(), z);
  }
}