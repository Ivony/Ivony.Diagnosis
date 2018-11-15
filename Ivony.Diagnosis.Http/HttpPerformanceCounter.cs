using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Ivony.Diagnosis
{
  public class HttpPerformanceCounter
  {
    private ConcurrentBag<Entry> _counter = new ConcurrentBag<Entry>();



    private struct Entry
    {
      public long elapsed;
      public int statusCode;
    }



    public HttpPerformanceCounter()
    {

    }


    public void OnRequestCompleted( long elapsed, int statusCode )
    {
      if ( _availables == false )
        return;

      _counter.Add( new Entry { elapsed = elapsed, statusCode = statusCode } );


    }


    private bool _availables = true;

    public void Stop()
    {
      _availables = false;
    }



    public class Report
    {

      public Report( HttpPerformanceCounter counter )
      {
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
        }

        else
          HttpStatusReport = new Dictionary<int, int>();
      }

      /// <summary>
      /// 总请求数
      /// </summary>
      public int TotalRequests { get; }

      /// <summary>
      /// 平均处理时间
      /// </summary>
      public TimeSpan AverageElapse { get; }

      /// <summary>
      /// 最长处理时间
      /// </summary>
      public TimeSpan MaxElapse { get; }

      /// <summary>
      /// 最短处理时间
      /// </summary>
      public TimeSpan MinElapse { get; }


      /// <summary>
      /// HTTP 各状态码计数
      /// </summary>
      public IDictionary<int, int> HttpStatusReport { get; }


      /// <summary>
      /// 错误率
      /// </summary>
      public double ErrorRate { get; }

      public override string ToString()
      {
        return $"total: {TotalRequests}, avg elapse: {AverageElapse.TotalMilliseconds:F2}ms, error rate: {ErrorRate:P2}";
      }

    }


    public Report CreateReport()
    {

      return new Report( this );


    }
  }
}
