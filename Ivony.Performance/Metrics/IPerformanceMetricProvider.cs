using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Ivony.Performance.Metrics
{
  /// <summary>
  /// 性能度量值提供程序
  /// </summary>
  public interface IPerformanceMetricProvider
  {

    /// <summary>
    /// 创建性能度量值
    /// </summary>
    /// <param name="report">性能报告对象</param>
    /// <param name="property">包含值的属性信息</param>
    /// <returns>性能度量</returns>
    PerformanceMetric CreateMetric( object report, PropertyInfo property );

  }
}
