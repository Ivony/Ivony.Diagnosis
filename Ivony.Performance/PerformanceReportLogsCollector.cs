using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Ivony.Performance
{
  public class PerformanceReportLogsCollector<TReport> : IPerformanceReportCollector<TReport> where TReport : IPerformanceReport
  {
    public PerformanceReportLogsCollector( ILogger<PerformanceReportLogsCollector<TReport>> logger )
    {
      Logger = logger;
    }

    public ILogger<PerformanceReportLogsCollector<TReport>> Logger { get; }

    public Task SendReportAsync( TReport report )
    {
      Logger.LogInformation( report.ToString() );
      return Task.CompletedTask;
    }
  }
}
