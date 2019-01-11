using System;
using System.Collections.Generic;
using System.Text;
using Ivony.Performance;
using Ivony.Performance.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class HttpPerformanceExtensions
  {

    public static void AddPerformanceMonitor( this IServiceCollection services )
    {
      services.AddSingleton<PerformanceService, PerformanceService>();
      services.AddSingleton<IHostedService>( serviceProvider => serviceProvider.GetRequiredService<PerformanceService>() );

      services.AddSingleton<HttpPerformanceMiddleware, HttpPerformanceMiddleware>();


    }
  }
}
