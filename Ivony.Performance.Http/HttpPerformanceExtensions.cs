using System.Net.Http;
using Ivony.Performance;
using Ivony.Performance.Http;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class HttpPerformanceExtensions
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


    /// <summary>
    /// 为 HttpMessageHandler 添加性能计数器
    /// </summary>
    /// <param name="handler">要添加性能计数器的 HttpMessageHandler</param>
    /// <returns></returns>
    public static HttpPerformanceDelegatingHandler WithPerformanceCounter( this HttpMessageHandler handler )
    {
      return new HttpPerformanceDelegatingHandler( handler );
    }

  }
}
