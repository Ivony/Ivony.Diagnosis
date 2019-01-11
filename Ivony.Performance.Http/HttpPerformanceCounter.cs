using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Ivony.Performance.Http
{
  /// <summary>
  /// 定义 HTTP 性能计数器
  /// </summary>
  public class HttpPerformanceCounter : PerformanceCounterBase<(long elapsed, int statusCode), IHttpPerformanceReport>
  {


    /// <summary>
    /// 当请求完成时调用此方法记录
    /// </summary>
    /// <param name="elapsed">响应时间（毫秒）</param>
    /// <param name="statusCode">响应状态码</param>
    public void OnRequestCompleted( long elapsed, int statusCode )
    {
      Increase( (elapsed, statusCode) );
    }

    private class Report : PerformanceReportBase, IHttpPerformanceReport
    {

      public Report( DateTime begin, DateTime end, (long elapsed, int statusCode)[] data ) : base( begin, end )
      {

        TotalRequests = data.Length;
        if ( data.Length > 0 )
        {
          AverageElapse = TimeSpan.FromMilliseconds( data.Average( entry => entry.elapsed ) );
          MaxElapse = TimeSpan.FromMilliseconds( data.Max( entry => entry.elapsed ) );
          MinElapse = TimeSpan.FromMilliseconds( data.Min( entry => entry.elapsed ) );

          HttpStatusReport = data.GroupBy( entry => entry.statusCode ).ToDictionary( item => item.Key, item => item.Count() );

          var errors = (double) data.Count( entry => entry.statusCode >= 300 );

          ErrorRate = errors / TotalRequests;
          var success = TotalRequests - errors;
          RequestPerSecond = success / (EndTime - BeginTime).TotalSeconds;
        }

        else
          HttpStatusReport = new Dictionary<int, int>();
      }




      public int TotalRequests { get; }

      public double RequestPerSecond { get; }

      public IDictionary<int, int> HttpStatusReport { get; }


      public TimeSpan AverageElapse { get; }

      public TimeSpan MaxElapse { get; }

      public TimeSpan MinElapse { get; }

      public double ErrorRate { get; }


      public override string ToString()
      {
        var report = $"{BeginTime:O} - {EndTime:O}\n";
        report += $"total: {TotalRequests}, rps: {RequestPerSecond:F0}, avg: {AverageElapse.TotalMilliseconds:F0}ms, max: {MaxElapse.TotalMilliseconds:F0}ms, min: {MinElapse.TotalMilliseconds:F0}ms, error rate: {ErrorRate:P2}\n";

        report += string.Join( ", ", HttpStatusReport.Select( item => $"HTTP{item.Key}: {item.Value}" ) );

        return report;

      }
    }


    protected override Task<IHttpPerformanceReport> CreateReportAsync( DateTime begin, DateTime end, (long elapsed, int statusCode)[] data )
    {

      return Task.Run( () => (IHttpPerformanceReport) new Report( begin, end, data ) );
    }
  }
}
