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
    /// 统计计数项个数
    /// </summary>
    /// <typeparam name="T">计数项类型</typeparam>
    /// <param name="data">性能数据</param>
    /// <returns></returns>
    public static PerformanceMetric[] Count<T>( this IPerformanceData data, string name )
    {
      return new[] { new PerformanceMetric( $"{data.DataSource}.{name}", Aggregation.Count, data.GetEntries<T>().Count(), PerformanceMetricUnit.pcs ) };
    }

    /// <summary>
    /// 计算平均每秒计数项
    /// </summary>
    /// <typeparam name="T">计数项类型</typeparam>
    /// <param name="data">性能数据</param>
    /// <returns></returns>
    public static PerformanceMetric[] CountPerSecond<T>( this IPerformanceData data, string name )
    {
      return new[] { new PerformanceMetric( $"{data.DataSource}.{name}", Aggregation.Count, (double) data.GetEntries<T>().Count() / data.TimeRange.TimeSpan.TotalSeconds, PerformanceMetricUnit.rps ) };
    }

    /// <summary>
    /// 最大最小和平均时延指标
    /// </summary>
    /// <typeparam name="T">计数项类型</typeparam>
    /// <param name="data">性能数据</param>
    /// <param name="elapseProvider">从性能计数项中提取时延的方法</param>
    /// <returns></returns>
    public static PerformanceMetric[] Elapsed<T>( this IPerformanceData data, string name, Func<T, TimeSpan> elapseProvider )
    {
      var elapsed = data.GetEntries<T>().Select( item => elapseProvider( item ).TotalMilliseconds ).OrderBy( item => item );

      if ( elapsed.Any() )
      {
        return new[]
        {
          new PerformanceMetric( $"{data.DataSource}.{name}", Aggregation.Max, elapsed.Last(), PerformanceMetricUnit.ms ),
          new PerformanceMetric( $"{data.DataSource}.{name}", Aggregation.Min, elapsed.First(), PerformanceMetricUnit.ms ),
          new PerformanceMetric( $"{data.DataSource}.{name}", Aggregation.Avg, elapsed.Average(), PerformanceMetricUnit.ms ),
        };
      }

      else
        return new PerformanceMetric[0];
    }


    /// <summary>
    /// 得到指定百分比的基线值
    /// </summary>
    /// <typeparam name="TEntry">计数项类型</typeparam>
    /// <typeparam name="TValue">计数值类型</typeparam>
    /// <param name="data">性能数据</param>
    /// <param name="baselines">要获取的基线值</param>
    /// <param name="valueProvider">计数值提供程序</param>
    /// <param name="comparer">用于比较两个计数值的比较器</param>
    /// <returns></returns>
    public static IReadOnlyDictionary<int, TValue> Baseline<TEntry, TValue>( this IPerformanceData data, int[] baselines, Func<TEntry, TValue> valueProvider, IComparer<TValue> comparer )
    {

      var values = data.GetEntries<TEntry>().Select( item => valueProvider( item ) ).OrderBy( item => item, comparer );


      var result = new Dictionary<int, TValue>();

      foreach ( var item in baselines )
      {
        if ( item >= 100 || item <= 0 )
          throw new ArgumentOutOfRangeException( "percent must greater than 0 and less than 100", nameof( item ) );

        var index = data.GetEntries<TEntry>().Count() * item / 100;

        result.Add( item, values.ElementAt( index ) );
      }

      return result;
    }
  }
}
