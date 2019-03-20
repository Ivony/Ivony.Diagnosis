using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Ivony.Performance
{


  /// <summary>
  /// 提供性能服务一些扩展方法
  /// </summary>
  public static class PerformanceServiceExtensions
  {


    /// <summary>
    /// 从系统服务中查找性能报告搜集器并注册
    /// </summary>
    /// <typeparam name="TReport">性能报告类型</typeparam>
    /// <param name="service">性能监控服务</param>
    /// <param name="source">性能报告源</param>
    /// <returns>返回一些 IDisposable 对象，用于取消注册性能报告搜集</returns>
    public static IDisposable Register<TReport>( this IPerformanceService service, IPerformanceSource<TReport> source ) where TReport : IPerformanceReport
    {
      var collectors = service.ServiceProvider.GetPerformanceReportCollectors<TReport>( source );
      return new DisposableHost( collectors.Select( item => service.Register( source, item ) ).ToArray() );
    }



    /// <summary>
    /// 从系统服务中查找性能报告搜集器
    /// </summary>
    /// <typeparam name="TReport">性能报告类型</typeparam>
    /// <param name="serviceProvider">系统服务提供程序</param>
    /// <param name="source">性能报告源</param>
    /// <returns>所有注册的性能报告搜集器</returns>
    public static IPerformanceCollector<TReport>[] GetPerformanceReportCollectors<TReport>( this IServiceProvider serviceProvider, IPerformanceSource<TReport> source ) where TReport : IPerformanceReport
    {

      var result = new HashSet<IPerformanceCollector<TReport>>();
      var factories = serviceProvider.GetServices<IPerformanceCollectorFactory>();

      foreach ( var item in factories )
      {
        var collector = item.GetReportCollector<TReport>( source );
        if ( collector != null )
          result.Add( collector );
      }


      result.UnionWith( serviceProvider.GetServices<IPerformanceCollector<TReport>>().ToArray() );

      return result.ToArray();

    }

    private class DisposableHost : IDisposable
    {
      private IDisposable[] _disposableObjects;

      public DisposableHost( IDisposable[] disposableObjects )
      {
        _disposableObjects = disposableObjects;
      }

      public void Dispose()
      {
        List<Exception> exceptions = new List<Exception>();

        foreach ( var item in _disposableObjects )
        {
          try
          {
            item.Dispose();
          }
          catch ( Exception e )
          {
            exceptions.Add( e );
          }
        }

        if ( exceptions.Any() )
          throw new AggregateException( exceptions );

      }
    }
  }
}
