using System;
using System.Collections.Generic;
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
    /// 注册一个性能报告源
    /// </summary>
    /// <param name="source">性能报告源</param>
    /// <returns></returns>
    IPerformanceCollectorRegistrationHost Register( IPerformanceSource source );

    /// <summary>
    /// 注册一个性能报告搜集器
    /// </summary>
    /// <param name="source">性能报告源</param>
    /// <param name="collector">性能报告搜集器</param>
    /// <returns></returns>
    IPerformanceCollectorRegistration Register( IPerformanceSource source, IPerformanceCollector collector );


    /// <summary>
    /// 获取所有性能报告源
    /// </summary>
    IEnumerable<IPerformanceSource> Sources { get; }



    /// <summary>
    /// 获取性能报告源注册的所有收集器
    /// </summary>
    /// <param name="source">性能报告源</param>
    /// <returns>注册在其上的收集器</returns>
    IPerformanceCollectorRegistrationHost GetRegistrations( IPerformanceSource source );

  }
}