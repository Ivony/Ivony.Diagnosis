using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Performance
{
  /// <summary>
  /// 定义 IPerformanceReportCollector 工厂类，负责创建 IPerformanceReportCollector 对象
  /// </summary>
  public interface IPerformanceCollectorFactory
  {

    IPerformanceCollector<TReport> GetReportCollector<TReport>( IPerformanceSource<TReport> counter ) where TReport : IPerformanceReport;

  }
}
