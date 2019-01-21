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

    protected PerformanceReportBase( DateTime begin, DateTime end )
    {
      BeginTime = begin;
      EndTime = end;


      GetType().GetProperties( BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty );

    }

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
