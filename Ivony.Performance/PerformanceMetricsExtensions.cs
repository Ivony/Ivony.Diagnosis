using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Ivony.Performance
{
  /// <summary>
  /// 提供一系列方法辅助创建 PerformanceMetric 对象
  /// </summary>
  public static class PerformanceMetricsExtensions
  {

    private static readonly IReadOnlyDictionary<string, string> _empty = new ReadOnlyDictionary<string, string>( new Dictionary<string, string>() );

    /// <summary>
    /// 得到一个计数指标值
    /// </summary>
    /// <typeparam name="TEntry">计数项类型</typeparam>
    /// <param name="entries">性能计数项</param>
    /// <returns></returns>
    public static PerformanceMetric[] Count<TEntry>( PerformanceEntries<TEntry> entries )
    {
      return new[] { new PerformanceMetric( "count", _empty, entries.Count, PerformanceMetricUnit.pcs ) };
    }

    /// <summary>
    /// 得到一个计时指标值
    /// </summary>
    /// <typeparam name="TEntry">计数项类型</typeparam>
    /// <param name="entries">性能计数项</param>
    /// <param name="elapseProvider">从性能计数项中提取时延的方法</param>
    /// <returns></returns>
    public static PerformanceMetric[] Elapsed<TEntry>( PerformanceEntries<TEntry> entries, Func<TEntry, TimeSpan> elapseProvider )
    {
      var elapsed = entries.Select( item => elapseProvider( item ).TotalMilliseconds ).OrderBy( item => item );

      return new[]
      {
        new PerformanceMetric( "max", _empty, elapsed.Last(), PerformanceMetricUnit.ms ),
        new PerformanceMetric( "min", _empty, elapsed.First(), PerformanceMetricUnit.ms ),
        new PerformanceMetric( "avg", _empty, elapsed.Average(), PerformanceMetricUnit.ms ),
      };
    }

    /// <summary>
    /// 得到一个计数指标值
    /// </summary>
    /// <typeparam name="TEntry">计数项类型</typeparam>
    /// <param name="entries">性能计数项</param>
    /// <param name="category">指标类型（默认为count）</param>
    /// <returns></returns>
    public static PerformanceMetric Count<TEntry>( PerformanceEntries<TEntry> entries, string category = "count" )
    {
      return new PerformanceMetric( "count", _empty, entries.Count, PerformanceMetricUnit.pcs );
    }

  }
}
