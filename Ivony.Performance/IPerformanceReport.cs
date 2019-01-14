using System;
using System.Collections.Generic;
using Ivony.Performance.Metrics;

namespace Ivony.Performance
{

  /// <summary>
  /// 定义性能报告抽象
  /// </summary>
  public interface IPerformanceReport
  {
    /// <summary>
    /// 性能报告开始时间
    /// </summary>
    DateTime BeginTime { get; }

    /// <summary>
    /// 性能报告结束时间
    /// </summary>
    DateTime EndTime { get; }



    /// <summary>
    /// 获取所有指标值
    /// </summary>
    /// <returns></returns>
    IReadOnlyDictionary<string, PerformanceMetric> GetMetrics();

  }
}