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
    public static IDisposable Register<TReport>( this IPerformanceService service, IPerformanceSource<TReport> counter ) where TReport : IPerformanceReport
    {
      return service.Register( counter, service.ServiceProvider.GetPerformanceReportCollectors<TReport>( counter ) );
    }



    /// <summary>
    /// 从系统服务中查找性能报告搜集器
    /// </summary>
    /// <typeparam name="TReport">性能报告类型</typeparam>
    /// <param name="serviceProvider">系统服务提供程序</param>
    /// <param name="counter">性能计数器</param>
    /// <returns>所有注册的性能报告搜集器</returns>
    public static IPerformanceCollector<TReport>[] GetPerformanceReportCollectors<TReport>( this IServiceProvider serviceProvider, IPerformanceSource<TReport> counter ) where TReport : IPerformanceReport
    {

      var result = new HashSet<IPerformanceCollector<TReport>>();
      var factories = serviceProvider.GetServices<IPerformanceCollectorFactory>();

      foreach ( var item in factories )
      {
        var collector = item.GetReportCollector<TReport>( counter );
        if ( collector != null )
          result.Add( collector );
      }


      result.UnionWith( serviceProvider.GetServices<IPerformanceCollector<TReport>>().ToArray() );

      return result.ToArray();

    }




  }
}
