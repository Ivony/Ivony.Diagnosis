﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Ivony.Diagnosis
{
  /// <summary>
  /// 定义 HTTP 性能计数器
  /// </summary>
  public class HttpPerformanceCounter
  {
    private ConcurrentBag<Entry> _counter = new ConcurrentBag<Entry>();



    private struct Entry
    {
      public long elapsed;
      public int statusCode;
    }



    private DateTime _startTime;

    private DateTime _stopTime;

    /// <summary>
    /// 创建 HttpPerformanceCounter 实例
    /// </summary>
    public HttpPerformanceCounter()
    {
      _startTime = DateTime.UtcNow;
    }


    /// <summary>
    /// 当请求完成时调用此方法记录
    /// </summary>
    /// <param name="elapsed">响应时间（毫秒）</param>
    /// <param name="statusCode">响应状态码</param>
    public void OnRequestCompleted( long elapsed, int statusCode )
    {
      if ( _availables == false )
        return;

      _counter.Add( new Entry { elapsed = elapsed, statusCode = statusCode } );


    }


    private bool _availables = true;

    /// <summary>
    /// 停止记录
    /// </summary>
    public void Stop()
    {
      if ( _availables )
      {
        lock ( this )
        {
          if ( _availables )
          {
            _stopTime = DateTime.UtcNow;
            _availables = false;
          }
        }
      }
    }



    public class Report : IHttpPerformanceReport
    {

      public Report( HttpPerformanceCounter counter )
      {

        StartTime = counter._startTime;
        StopTime = counter._stopTime;

        var data = counter._counter.ToArray();

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
          RequestPerSecond = success / (StopTime - StartTime).TotalSeconds;
        }

        else
          HttpStatusReport = new Dictionary<int, int>();
      }




      public int TotalRequests { get; }

      public double RequestPerSecond { get; }

      public IDictionary<int, int> HttpStatusReport { get; }



      public DateTime StartTime { get; }

      public DateTime StopTime { get; }


      public TimeSpan AverageElapse { get; }

      public TimeSpan MaxElapse { get; }

      public TimeSpan MinElapse { get; }

      public double ErrorRate { get; }


      public override string ToString()
      {
        var report = $"total: {TotalRequests}, rps: {RequestPerSecond:F0}, avg: {AverageElapse.TotalMilliseconds:F0}ms, max: {MaxElapse.TotalMilliseconds:F0}ms, min: {MinElapse.TotalMilliseconds:F2}ms, error rate: {ErrorRate:P2}\n";

        report += string.Join( ", ", HttpStatusReport.Select( item => $"HTTP{item.Key}: {item.Value}" ) );

        return report;

      }
    }


    public Report CreateReport()
    {

      Stop();

      return new Report( this );


    }
  }
}
