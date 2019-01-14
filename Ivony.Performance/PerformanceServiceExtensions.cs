using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Ivony.Performance
{
  public static class PerformanceServiceExtensions
  {


    /// <summary>
    /// 从系统服务中查找性能报告搜集器并注册
    /// </summary>
    /// <typeparam name="TReport">性能报告类型</typeparam>
    /// <param name="counter">性能计数器</param>
    /// <param name="collector">性能报告搜集器</param>
    /// <returns>返回一个 IDisposable 对象，用于取消注册性能报告搜集</returns>
    public static IDisposable Register<TReport>( IPerformanceService service, IPerformanceCounter<TReport> counter ) where TReport : IPerformanceReport
    {
      return service.Register( counter, service.ServiceProvider.GetServices<IPerformanceReportCollector<TReport>>().ToArray() );
    }

  }
}
