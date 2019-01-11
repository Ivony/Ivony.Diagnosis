using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;


using Timer = System.Timers.Timer;

namespace Ivony.Performance
{

  /// <summary>
  /// 定义性能计数器基类型
  /// </summary>
  public abstract class PerformanceCounterBase<TEntry, TReport> : IPerformanceCounter<TReport> where TReport : IPerformanceReport
  {

    private ConcurrentBag<TEntry> counter;
    private Timer timer;
    private DateTime timestamp;


    /// <summary>
    /// 初始化 PerformanceCounterBase 实例
    /// </summary>
    /// <param name="interval">发送报告的间隔时间</param>
    /// <param name="reportCollector">收集性能报告的收集器</param>
    protected PerformanceCounterBase()
    {
      counter = new ConcurrentBag<TEntry>();
    }

    public virtual Task<TReport> CreateReportAsync()
    {
      var _counter = counter;
      counter = new ConcurrentBag<TEntry>();
      Thread.MemoryBarrier();


      var begin = timestamp;
      var end = timestamp = DateTime.UtcNow;
      Thread.MemoryBarrier();

      return CreateReportAsync( begin, end, _counter.ToArray() );

    }

    protected abstract Task<TReport> CreateReportAsync( DateTime begin, DateTime end, TEntry[] data );


    protected void Increase( TEntry entry )
    {
      counter.Add( entry );
    }


  }
}
