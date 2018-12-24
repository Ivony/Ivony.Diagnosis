using System;

namespace Ivony.Diagnosis
{

  /// <summary>
  /// 定义性能报告抽象
  /// </summary>
  public interface IPerformanceReport
  {
    /// <summary>
    /// 性能报告开始时间
    /// </summary>
    DateTime StartTime { get; }

    /// <summary>
    /// 性能报告结束时间
    /// </summary>
    DateTime StopTime { get; }
  }
}