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
  public interface IPerformanceCollector<TReport> where TReport : IPerformanceReport
  {
    /// <summary>
    /// 收集性能报告
    /// </summary>
    /// <param name="service">性能报告服务</param>
    /// <param name="timestamp">当前时间戳</param>
    /// <param name="report">性能报告</param>
    /// <returns></returns>
    Task CollectReportAsync( IPerformanceService service, DateTime timestamp, TReport report );

  }


  /// <summary>
  /// 定义全局性能报告收集器抽象
  /// </summary>
  public interface IGlobalPerformanceCollector
  {
    /// <summary>
    /// 收集性能报告
    /// </summary>
    /// <param name="service">性能报告服务</param>
    /// <param name="timestamp">当前时间戳</param>
    /// <param name="reports">性能报告</param>
    /// <returns></returns>
    Task CollectReportsAsync( IPerformanceService service, DateTime timestamp, IPerformanceReport[] reports );
  }

}
