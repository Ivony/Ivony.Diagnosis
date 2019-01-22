using Ivony.Performance;

using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class ServicesExtensions
  {

    /// <summary>
    /// 添加性能监控相关服务
    /// </summary>
    /// <param name="services">服务注册容器</param>
    public static void AddPerformanceMonitor( this IServiceCollection services )
    {
      services.AddSingleton<IPerformanceService, PerformanceService>();
      services.AddSingleton<IHostedService>( serviceProvider => serviceProvider.GetRequiredService<IPerformanceService>() );
    }

  }
}
