using System;

namespace Ivony.Performance
{


  /// <summary>
  /// 定义性能计数器
  /// </summary>
  public interface IPerformanceSource
  {

    /// <summary>
    /// 搜集目前已经产生的性能数据
    /// </summary>
    /// <param name="context">性能报告上下文</param>
    /// <returns>性能数据</returns>
    IPerformanceData CreatePerformanceData( PerformanceContext context );
  }
}