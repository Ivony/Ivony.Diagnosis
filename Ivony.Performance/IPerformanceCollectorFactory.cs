using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Performance
{
  /// <summary>
  /// 定义 IPerformanceCollector 工厂类，负责创建 IPerformanceCollector 对象
  /// </summary>
  public interface IPerformanceCollectorFactory
  {

    /// <summary>
    /// 对指定的性能报告源创建搜集器 
    /// </summary>
    /// <param name="source">性能报告源</param>
    /// <returns>可以搜集这些性能报告的搜集器</returns>
    IPerformanceCollector[] GetReportCollectors( IPerformanceSource source );

  }
}
