using System;

namespace Ivony.Performance
{

  /// <summary>
  /// 定义一个性能报告收集器注册信息
  /// </summary>
  public interface IPerformanceCollectorRegistration : IDisposable
  {
    /// <summary>
    /// 性能报告收集器
    /// </summary>
    IPerformanceCollector Collector { get; }

    /// <summary>
    /// 性能报告源
    /// </summary>
    IPerformanceSource Source { get; }
  }
}