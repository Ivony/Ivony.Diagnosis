using System;
using System.Collections.Generic;
using Ivony.Performance.Metrics;

namespace Ivony.Performance.Http
{
  /// <summary>
  /// HTTP 性能报告
  /// </summary>
  public interface IHttpPerformanceReport : IPerformanceReport
  {

    [Unit_pcs]
    /// <summary>
    /// 总请求数
    /// </summary>
    int TotalRequests { get; }


    /// <summary>
    /// HTTP 各状态码计数
    /// </summary>
    IDictionary<int, int> HttpStatusReport { get; }


    [Unit_ms]
    /// <summary>
    /// 平均处理时间
    /// </summary>
    TimeSpan AverageElapse { get; }

    [Unit_ms]
    /// <summary>
    /// 最长处理时间
    /// </summary>
    TimeSpan MaxElapse { get; }

    [Unit_ms]
    /// <summary>
    /// 最短处理时间
    /// </summary>
    TimeSpan MinElapse { get; }

    [Unit_percent]
    /// <summary>
    /// 错误率
    /// </summary>
    double ErrorRate { get; }

  }
}