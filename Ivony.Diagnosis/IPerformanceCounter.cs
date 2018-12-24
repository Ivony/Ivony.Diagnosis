using System;

namespace Ivony.Diagnosis
{

  /// <summary>
  /// 性能计数器抽象
  /// </summary>
  /// <typeparam name="TReport">性能报告格式</typeparam>
  public interface IPerformanceCounter<TReport>
  {

    /// <summary>
    /// 开始进行性能计数
    /// </summary>
    void Start();

    /// <summary>
    /// 停止性能计数
    /// </summary>
    void Stop();

    /// <summary>
    /// 订阅性能报告
    /// </summary>
    /// <param name="observer"></param>
    /// <returns></returns>
    IDisposable Subscribe( IPerformanceReportObserver<TReport> observer );
  }
}