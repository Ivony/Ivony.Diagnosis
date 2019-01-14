﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Ivony.Performance
{


  /// <summary>
  /// 收集性能日志并记录到日志的性能报告收集器
  /// </summary>
  /// <typeparam name="TReport">性能报告</typeparam>
  public class PerformanceReportLogsCollector<TReport> : IPerformanceReportCollector<TReport> where TReport : IPerformanceReport
  {

    /// <summary>
    /// 创建 PerformanceReportLogsCollector 对象
    /// </summary>
    /// <param name="logger">日志记录器</param>
    public PerformanceReportLogsCollector( ILogger<PerformanceReportLogsCollector<TReport>> logger )
    {
      Logger = logger;
    }

    /// <summary>
    /// 日志记录器
    /// </summary>
    public ILogger<PerformanceReportLogsCollector<TReport>> Logger { get; }


    /// <summary>
    /// 发送性能报告
    /// </summary>
    /// <param name="report">性能报告</param>
    /// <returns></returns>
    public Task CollectReportAsync( TReport report )
    {
      Logger.LogInformation( report.ToString() );
      return Task.CompletedTask;
    }
  }
}