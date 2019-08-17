using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Ivony.Performance
{
  /// <summary>
  /// 性能度量提供程序
  /// </summary>
  public interface IPerformanceProvider<TEntry>
  {

    /// <summary>
    /// 获取性能度量值
    /// </summary>
    /// <param name="entries">性能计数项</param>
    /// <returns>性能度量值</returns>
    PerformanceMetric[] GetMetrics( TEntry[] entries );

  }
}
