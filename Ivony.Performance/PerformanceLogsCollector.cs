using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Ivony.Performance
{


  /// <summary>
  /// 收集性能日志并记录到日志的性能报告收集器
  /// </summary>
  public class PerformanceLogsCollector : IPerformanceCollector
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
    /// 搜集和记录性能指标
    /// </summary>
    /// <param name="context"></param>
    /// <param name="metrics"></param>
    /// <returns></returns>
    public Task CollectReportAsync( PerformanceContext context, PerformanceMetric[] metrics )
    {

      using ( var writer = new StringWriter() )
      {
        foreach ( var item in metrics )
          writer.WriteLine( item );

        Logger.LogInformation( writer.ToString() );
      }

      return Task.CompletedTask;
    }

  }
}
