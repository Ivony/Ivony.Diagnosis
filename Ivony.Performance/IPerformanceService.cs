using System;
using Microsoft.Extensions.Hosting;

namespace Ivony.Performance
{

  /// <summary>
  /// 定义性能监控服务
  /// </summary>
  public interface IPerformanceService : IHostedService, IDisposable
  {


    /// <summary>
    /// 系统服务提供程序
    /// </summary>
    IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// 注册一个性能报告搜集器
    /// </summary>
    /// <typeparam name="TReport">性能报告类型</typeparam>
    /// <param name="source">性能报告源</param>
    /// <param name="collector">性能报告搜集器</param>
    /// <returns></returns>
    IDisposable Register<TReport>( IPerformanceSource<TReport> source, IPerformanceCollector<TReport> collector ) where TReport : IPerformanceReport;
  }
}