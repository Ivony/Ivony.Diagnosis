using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Performance
{
  public interface IPerformanceReportCollector<TReport> where TReport : IPerformanceReport
  {
    Task SendReportAsync( TReport report );

  }
}
