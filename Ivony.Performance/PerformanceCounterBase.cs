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
  public abstract class PerformanceCounterBase<TEntry, TReport> : IPerformanceSource<TReport> where TReport : IPerformanceReport
  {

    private ConcurrentBag<TEntry> counter;
    private DateTime timestamp;


    /// <summary>
    /// 初始化 PerformanceCounterBase 实例
    /// </summary>
    protected PerformanceCounterBase()
    {
      counter = new ConcurrentBag<TEntry>();
    }



    /// <summary>
    /// 创建性能报告
    /// </summary>
    /// <returns></returns>
    public virtual async Task<TReport> CreateReportAsync()
    {

      var _counter = counter;
      counter = new ConcurrentBag<TEntry>();
      Thread.MemoryBarrier();

      var begin = timestamp;
      var end = timestamp = DateTime.UtcNow;
      Thread.MemoryBarrier();



      await Task.Yield();

      return await CreateReportAsync( begin, end, _counter.ToArray() );

    }

    /// <summary>
    /// 创建性能报告
    /// </summary>
    /// <param name="begin">开始时间</param>
    /// <param name="end">结束时间</param>
    /// <param name="data">搜集到的数据</param>
    /// <returns>性能报告</returns>
    protected abstract Task<TReport> CreateReportAsync( DateTime begin, DateTime end, TEntry[] data );



    /// <summary>
    /// 进行性能计数
    /// </summary>
    /// <param name="entry"></param>
    protected void Increase( TEntry entry )
    {
      counter.Add( entry );
    }


  }
}
