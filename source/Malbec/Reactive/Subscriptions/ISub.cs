using System;
using Malbec.Logs;

namespace Malbec.Reactive.Subscriptions
{
  public interface ISub<out TΔ, out T> : ILog<TΔ, T>, IDisposable {}
}