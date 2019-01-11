using System;
using System.Collections.Generic;

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
    DateTime StartTime { get; }

    /// <summary>
    /// 性能报告结束时间
    /// </summary>
    DateTime StopTime { get; }


    /// <summary>
    /// 可用的指标列表
    /// </summary>
    IEnumerable<string> Keys { get; }


    /// <summary>
    /// 获取指标值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    double GetValue( string key );

  }
}