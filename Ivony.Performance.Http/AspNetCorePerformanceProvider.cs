using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Performance.Http
{

  /// <summary>
  /// ASP.NET Core 性能指标提供程序
  /// </summary>
  public class AspNetCorePerformanceProvider : IPerformanceProvider<AspNetCorePerformanceData>
  {
    public PerformanceMetric[] GetMetrics( AspNetCorePerformanceData data )
    {

      var metrics = new List<PerformanceMetric>();

      metrics.AddRange( data.CountPerSecond<HttpRequestEntry>( "rps" ) );
      metrics.AddRange( data.Elapsed<HttpCompletedRequestEntry>( "elapsed", entry => entry.Duration ) );
      var baseline = data.Baseline<HttpCompletedRequestEntry, TimeSpan>( new[] { 99, 95, 90 }, entry => entry.Duration, Comparer<TimeSpan>.Default );



      metrics.AddRange( data.GetPerformanceData( "processing" ).Count<HttpRequestEntry>( "count" ) );

      throw new NotImplementedException();
    }
  }
}
