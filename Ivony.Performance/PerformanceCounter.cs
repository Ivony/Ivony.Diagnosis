using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Performance
{

  /// <summary>
  /// 实现一个性能计数器
  /// </summary>
  /// <typeparam name="TEntry">计数项</typeparam>
  public class PerformanceCounter<TEntry>
  {

    private ConcurrentBag<TEntry> _collection = new ConcurrentBag<TEntry>();


    /// <summary>
    /// 当性能计数源产生了一个计数项时调用此方法计数
    /// </summary>
    /// <param name="entry">计数项</param>
    public void AddEntry( TEntry entry )
    {
      _collection.Add( entry );
    }

    /// <summary>
    /// 获取到目前为止搜集到的计数项
    /// </summary>
    /// <returns>目前为止搜集到的所有计数项</returns>
    public TEntry[] GetEntries()
    {
      var collected = _collection;
      _collection = new ConcurrentBag<TEntry>();

      return collected.ToArray();
    }


    /// <summary>
    /// 转换为 IObserver 对象
    /// </summary>
    /// <returns></returns>
    public IObserver<TEntry> AsObserver() => new Observer( this );


    private class Observer : IObserver<TEntry>
    {
      private readonly PerformanceCounter<TEntry> _counter;

      public Observer( PerformanceCounter<TEntry> counter )
      {
        _counter = counter;
      }

      public void OnCompleted()
      {
      }

      public void OnError( Exception error )
      {
      }

      public void OnNext( TEntry value )
      {
        _counter.AddEntry( value );
      }
    }
  }
}
