using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Ivony.Performance
{

  /// <summary>
  /// 实现一个性能计数器
  /// </summary>
  /// <typeparam name="TEntry">计数项</typeparam>
  public class PerformanceCounter<TEntry> : IPerformanceCounter<TEntry>
  {

    public PerformanceCounter( string dataSource )
    {
      DataSource = dataSource;
    }


    private DateTime lastTiemStamp = DateTime.UtcNow;


    private ConcurrentBag<TEntry> _collection = new ConcurrentBag<TEntry>();


    /// <summary>
    /// 数据源名称
    /// </summary>
    public string DataSource { get; }

    /// <summary>
    /// 当性能计数源产生了一个计数项时调用此方法计数
    /// </summary>
    /// <param name="entry">计数项</param>
    public virtual void Push( TEntry entry )
    {
      _collection.Add( entry );
    }

    /// <summary>
    /// 获取到目前为止搜集到的性能数据
    /// </summary>
    /// <returns>目前为止搜集到的所有计数项</returns>
    public virtual IPerformanceData<TEntry> GetPerformanceData( DateTime timeStamp )
    {
      var collected = GetCollected();
      var result = new PerformanceData<TEntry>( DataSource, new DateTimeRange( lastTiemStamp, timeStamp ), collected.ToArray() ); 
      lastTiemStamp = timeStamp;
      return result;
    }

    /// <summary>
    /// 获取当前已经搜集到的计数项，并将计数项清空重新记录
    /// </summary>
    /// <returns></returns>
    protected ConcurrentBag<TEntry> GetCollected()
    {
      return Interlocked.Exchange( ref _collection, new ConcurrentBag<TEntry>() );
    }
  }
}
