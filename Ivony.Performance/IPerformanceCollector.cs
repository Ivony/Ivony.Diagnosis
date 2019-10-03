using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.Performance
{

  /// <summary>
  /// 定义性能报告收集器抽象
  /// </summary>
  public interface IPerformanceCollector
  {
    /// <summary>
    /// 收集性能报告
    /// </summary>
    /// <param name="context">性能报告上下文</param>
    /// <param name="metrics">性能度量值列表</param>
    /// <returns></returns>
    Task CollectReportAsync( PerformanceContext context, PerformanceMetric[] metrics );

  }
}
