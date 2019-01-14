using Ivony.Performance;

using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class HttpPerformanceExtensions
  {

    public static void AddPerformanceMonitor( this IServiceCollection services )
    {
      services.AddSingleton<IPerformanceService, PerformanceService>();
      services.AddSingleton<IHostedService>( serviceProvider => serviceProvider.GetRequiredService<IPerformanceService>() );



    }
  }
}
