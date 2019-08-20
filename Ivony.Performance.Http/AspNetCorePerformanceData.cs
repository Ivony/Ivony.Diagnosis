using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ivony.Performance.Http
{
  public class AspNetCorePerformanceData : PerformanceData
  {


    private object[] _data;

    public AspNetCorePerformanceData( string dataSource, DateTimeRange timeRange, HttpRequestEntry[] requests, HttpCompletedRequestEntry[] completed ) : base( dataSource, timeRange, requests.Cast<Object>().Concat( completed ) )
    {
      DataSource = dataSource;
      TimeRange = timeRange;

      _data = new[] { requests, completed };
    }

    public string DataSource { get; }

    public DateTimeRange TimeRange { get; }

    public IReadOnlyList<T> GetItems<T>()
    {
      throw new NotImplementedException();
    }

    public IPerformanceData GetPerformanceData( string name )
    {
      throw new NotImplementedException();
    }


  }
}
