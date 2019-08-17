using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ivony.Performance.Metrics;
using Microsoft.Extensions.Logging;

namespace Ivony.Performance.Http
{
  /// <summary>
  /// 定义 HTTP 性能计数器
  /// </summary>
  public class HttpPerformanceCounter : PerformanceCounterBase<(long elapsed, int statusCode)>
  {



    /// <summary>
    /// 性能报告源名称
    /// </summary>
    public override string SourceName { get; }



    /// <summary>
    /// 创建 HTTP 性能计数器实例
    /// </summary>
    /// <param name="sourceName">源名称</param>
    public HttpPerformanceCounter( string sourceName )
    {
      SourceName = sourceName;
    }



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

      public Report( IPerformanceSource source, DateTime begin, DateTime end, (long elapsed, int statusCode)[] data ) : base( source, begin, end )
      {

        TotalRequests = data.Length;
        if ( data.Length > 0 )
        {


          var sorted = data.OrderBy( item => item.elapsed ).ToArray();

          AverageElapse = TimeSpan.FromMilliseconds( sorted.Average( entry => entry.elapsed ) );
          MaxElapse = TimeSpan.FromMilliseconds( sorted.Last().elapsed );
          MinElapse = TimeSpan.FromMilliseconds( sorted.First().elapsed );


          var count = sorted.Length;

          var c95 = (int) (count * 0.05);
          var c99 = (int) (count * 0.01);

          Percent95Elapsed = TimeSpan.FromMilliseconds( sorted.Skip( c95 ).First().elapsed );
          Percent99Elapsed = TimeSpan.FromMilliseconds( sorted.Skip( c99 ).First().elapsed );



          HttpStatusReport = data.GroupBy( entry => entry.statusCode ).ToDictionary( item => item.Key, item => item.Count() );

          var errors = (double) data.Count( entry => entry.statusCode >= 300 );

          ErrorRate = errors / TotalRequests;
          var success = TotalRequests - errors;
          RequestPerSecond = success / (EndTime - BeginTime).TotalSeconds;
        }

        else
          HttpStatusReport = new Dictionary<int, int>();
      }




      [Unit_pcs]
      public int TotalRequests { get; }

      [Unit_rps]
      public double RequestPerSecond { get; }

      [Unit_pcs]
      public IDictionary<int, int> HttpStatusReport { get; }


      [Unit_ms]
      public TimeSpan AverageElapse { get; }

      [Unit_ms]
      public TimeSpan Percent99Elapsed { get; }

      [Unit_ms]
      public TimeSpan Percent95Elapsed { get; }

      [Unit_ms]
      public TimeSpan MaxElapse { get; }

      [Unit_ms]
      public TimeSpan MinElapse { get; }

      [Unit_percent]
      public double ErrorRate { get; }


      public override string ToString()
      {
        var report = $"{BeginTime:O} - {EndTime:O}\n";
        report += $"total: {TotalRequests}, rps: {RequestPerSecond:F0}, avg: {AverageElapse.TotalMilliseconds:F0}ms, max: {MaxElapse.TotalMilliseconds:F0}ms, min: {MinElapse.TotalMilliseconds:F0}ms, error rate: {ErrorRate:P2}\n";

        report += string.Join( ", ", HttpStatusReport.Select( item => $"HTTP{item.Key}: {item.Value}" ) );

        return report;

      }
    }



    /// <summary>
    /// 创建性能报告
    /// </summary>
    /// <param name="begin">开始时间</param>
    /// <param name="end">结束时间</param>
    /// <param name="data">搜集到的数据</param>
    /// <returns>性能报告</returns>
    protected override Task<IPerformanceReport> CreateReportAsync( DateTime begin, DateTime end, (long elapsed, int statusCode)[] data )
    {

      return Task.Run( () => (IPerformanceReport) new Report( this, begin, end, data ) );
    }
  }
}
