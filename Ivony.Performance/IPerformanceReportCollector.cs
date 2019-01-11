using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Performance
{

  /// <summary>
  /// 定义性能报告收集器抽象
  /// </summary>
  /// <typeparam name="TReport">性能报告类型</typeparam>
  public interface IPerformanceReportCollector<TReport> where TReport : IPerformanceReport
  {
    /// <summary>
    /// 收集性能报告
    /// </summary>
    /// <param name="report">性能报告</param>
    /// <returns></returns>
    Task CollectReportAsync( TReport report );

  }
}
