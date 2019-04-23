using Ivony.Performance;

using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{


  /// <summary>
  /// 依赖注入服务相关扩展方法
  /// </summary>
  public static class ServicesExtensions
  {

    /// <summary>
    /// 添加性能监控相关服务
    /// </summary>
    /// <param name="services">服务注册容器</param>
    public static IServiceCollection AddPerformanceMonitor( this IServiceCollection services )
    {
      services.AddSingleton<IPerformanceService, PerformanceService>();
      services.AddSingleton<IHostedService>( serviceProvider => serviceProvider.GetRequiredService<IPerformanceService>() );

      return services;
    }

  }
}
