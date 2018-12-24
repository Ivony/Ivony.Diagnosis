using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Diagnosis
{
  public interface IPerformanceReportObserver<TReport>
  {
    void Next( TReport report );

  }
}
