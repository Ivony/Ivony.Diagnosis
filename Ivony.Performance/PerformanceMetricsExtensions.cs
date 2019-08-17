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
    public static PerformanceMetric[] Count<TEntry>( PerformanceData<TEntry> data )
    {
      return new[] { new PerformanceMetric( data.DataSource, PerformanceAggregation.Count, data.Count, PerformanceMetricUnit.pcs ) };
    }

    /// <summary>
    /// 得到一个计时指标值
    /// </summary>
    /// <typeparam name="TEntry">计数项类型</typeparam>
    /// <param name="data">性能计数项</param>
    /// <param name="elapseProvider">从性能计数项中提取时延的方法</param>
    /// <returns></returns>
    public static PerformanceMetric[] Elapsed<TEntry>( PerformanceData<TEntry> data, Func<TEntry, TimeSpan> elapseProvider )
    {
      var elapsed = data.Select( item => elapseProvider( item ).TotalMilliseconds ).OrderBy( item => item );

      return new[]
      {
        new PerformanceMetric( data.DataSource, PerformanceAggregation.Max, elapsed.Last(), PerformanceMetricUnit.ms ),
        new PerformanceMetric( data.DataSource, PerformanceAggregation.Min, elapsed.First(), PerformanceMetricUnit.ms ),
        new PerformanceMetric( data.DataSource, PerformanceAggregation.Avg, elapsed.Average(), PerformanceMetricUnit.ms ),
      };
    }


    /// <summary>
    /// 得到指定百分比的基线值
    /// </summary>
    /// <typeparam name="TEntry">计数项类型</typeparam>
    /// <typeparam name="TValue">计数值类型</typeparam>
    /// <param name="data">性能计数数据</param>
    /// <param name="baselines">要获取的基线值</param>
    /// <param name="valueProvider">计数值提供程序</param>
    /// <param name="comparer">用于比较两个计数值的比较器</param>
    /// <returns></returns>
    public static IReadOnlyDictionary<int, TValue> Baseline<TEntry, TValue>( PerformanceData<TEntry> data, int[] baselines, Func<TEntry, TValue> valueProvider, IComparer<TValue> comparer )
    {

      var values = data.Select( item => valueProvider( item ) ).OrderBy( item => item, comparer );


      var result = new Dictionary<int, TValue>();

      foreach ( var item in baselines )
      {
        if ( item >= 100 || item <= 0 )
          throw new ArgumentOutOfRangeException( "percent must greater than 0 and less than 100", nameof( item ) );

        var index = data.Count * item / 100;

        result.Add( item, values.ElementAt( index ) );
      }

      return result;
    }
  }
}
