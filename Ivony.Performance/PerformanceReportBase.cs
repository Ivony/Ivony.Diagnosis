using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

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

    public DateTime BeginTime { get; }

    public DateTime EndTime { get; }

    public IEnumerable<string> Keys { get; } = Enumerable.Empty<string>();

    public double GetValue( string key )
    {
      throw new ArgumentException( "key", $"\"{key}\" is not supported." );
    }
  }
}
