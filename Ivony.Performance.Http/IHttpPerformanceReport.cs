using System;
using System.Collections.Generic;

namespace Ivony.Performance.Http
{
  /// <summary>
  /// HTTP 性能报告
  /// </summary>
  public interface IHttpPerformanceReport : IPerformanceReport
  {

    /// <summary>
    /// 总请求数
    /// </summary>
    int TotalRequests { get; }


    /// <summary>
    /// HTTP 各状态码计数
    /// </summary>
    IDictionary<int, int> HttpStatusReport { get; }


    /// <summary>
    /// 平均处理时间
    /// </summary>
    TimeSpan AverageElapse { get; }

    /// <summary>
    /// 最长处理时间
    /// </summary>
    TimeSpan MaxElapse { get; }

    /// <summary>
    /// 最短处理时间
    /// </summary>
    TimeSpan MinElapse { get; }

    /// <summary>
    /// 错误率
    /// </summary>
    double ErrorRate { get; }

  }
}