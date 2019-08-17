using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Performance.Http
{
  public class HttpPerformanceProvider : IPerformanceProvider<HttpRequestEntry>
  {
    public PerformanceMetric[] GetMetrics( HttpRequestEntry[] entries )
    {

      List<PerformanceMetric> metrics = new List<PerformanceMetric>( 10 );

      metrics.Add( Count( entries ) );
      metrics.Add( Eplased( entries, item => item.Duration ) );
      metrics.Add( ErrorRate( entries, item => item. ) );


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

    private PerformanceMetric ErrorRate( HttpRequestEntry[] entries, Func<HttpRequestEntry, TimeSpan> elapseProvider )
    {
      throw new NotImplementedException();
    }



    private PerformanceMetric Eplased( HttpRequestEntry[] entries, Func<HttpRequestEntry, TimeSpan> elapseProvider )
    {
      throw new NotImplementedException();
    }

    private PerformanceMetric Count( HttpRequestEntry[] entries )
    {
      throw new NotImplementedException();
    }
  }
}
