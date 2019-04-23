using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Ivony.Performance
{


  /// <summary>
  /// 收集性能日志并记录到日志的性能报告收集器
  /// </summary>
  public class PerformanceLogsCollector : IPerformanceCollector , IGlobalPerformanceCollector
  {

    /// <summary>
    /// 创建 PerformanceReportLogsCollector 对象
    /// </summary>
    /// <param name="logger">日志记录器</param>
    public PerformanceLogsCollector( ILogger<PerformanceLogsCollector> logger )
    {
      Logger = logger;
    }

    /// <summary>
    /// 日志记录器
    /// </summary>
    public ILogger<PerformanceLogsCollector> Logger { get; }


    /// <summary>
    /// 发送性能报告
    /// </summary>
    /// <param name="service">性能报告服务</param>
    /// <param name="timestamp">时间戳</param>
    /// <param name="report">性能报告</param>
    /// <returns></returns>
    public Task CollectReportAsync( IPerformanceService service, DateTime timestamp, IPerformanceReport report )
    {
      Logger.LogInformation( report.ToString() );
      return Task.CompletedTask;
    }

    public Task CollectReportsAsync( IPerformanceService service, DateTime timestamp, IPerformanceReport[] reports )
    {
      foreach ( var item in reports )
      {
        Logger.LogInformation( item.ToString() );
      }
      return Task.CompletedTask;
    }
  }
}
