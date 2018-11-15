using System;
using System.Collections.Generic;

namespace Ivony.Diagnosis
{
  /// <summary>
  /// HTTP 性能报告
  /// </summary>
  public interface IHttpPerformanceReport
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
    /// 性能报告开始时间
    /// </summary>
    DateTime StartTime { get; }

    /// <summary>
    /// 性能报告结束时间
    /// </summary>
    DateTime StopTime { get; }

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