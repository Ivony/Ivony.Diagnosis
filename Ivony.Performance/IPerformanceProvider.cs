using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Ivony.Performance
{
  /// <summary>
  /// 性能度量提供程序
  /// </summary>
  public interface IPerformanceProvider
  {

    /// <summary>
    /// 获取性能度量值
    /// </summary>
    /// <param name="data">性能数据</param>
    /// <returns>性能度量值</returns>
    PerformanceMetric[] GetMetrics( IPerformanceData data );

  }
}
