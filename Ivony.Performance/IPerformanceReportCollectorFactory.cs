using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Performance
{
  public interface IPerformanceReportCollectorFactory
  {

    IPerformanceReportCollector<TReport> GetReportCollector<TReport>( IPerformanceCounter<TReport> counter ) where TReport : IPerformanceReport;

  }
}
