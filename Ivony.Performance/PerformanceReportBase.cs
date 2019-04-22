using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Ivony.Performance.Metrics;

namespace Ivony.Performance
{
  public class PerformanceReportBase : IPerformanceReport
  {

    /// <summary>
    /// 创建 PerformanceReportBase 实例
    /// </summary>
    /// <param name="source">性能报告源</param>
    /// <param name="begin">计数开始时间</param>
    /// <param name="end">计数结束时间</param>
    protected PerformanceReportBase( IPerformanceSource source, DateTime begin, DateTime end )
    {
      Source = source;
      BeginTime = begin;
      EndTime = end;


      GetType().GetProperties( BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty );

    }

    /// <summary>
    /// 创建此性能报告的性能报告源
    /// </summary>
    public IPerformanceSource Source { get; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime BeginTime { get; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndTime { get; }



    private readonly object sync = new object();
    private IReadOnlyDictionary<string, PerformanceMetric> metrics;



    /// <summary>
    /// 获取性能度量值
    /// </summary>
    /// <returns>性能度量值</returns>
    public virtual IReadOnlyDictionary<string, PerformanceMetric> GetMetrics()
    {
      lock ( sync )
      {
        if ( metrics != null )
          return metrics;

        var list =
          from property in GetType().GetProperties( BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty )
          let attribute = property.GetCustomAttributes().Select( attribute => attribute as MetricAttributeBase ).FirstOrDefault( attribute => attribute != null )
          where attribute != null
          select (property.Name, attribute.GetMetric( this, property ));

        return metrics = new ReadOnlyDictionary<string, PerformanceMetric>( list.ToDictionary( item => item.Item1, item => item.Item2 ) );
      }
    }
  }
}
