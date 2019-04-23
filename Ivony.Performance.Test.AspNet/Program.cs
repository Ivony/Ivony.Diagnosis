using System;
using Ivony.Performance.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ivony.Performance.Test.AspNet
{
  class Program
  {
    static void Main( string[] args )
    {
      WebHost
        .CreateDefaultBuilder( args )
        .UseKestrel()
        .ConfigureLogging( ConfgureLogging )
        .UseStartup<Startup>()
        .Build()
        .RunAsync().Wait();
    }

    private static void ConfgureLogging( ILoggingBuilder builder )
    {
      builder.AddConsole().SetMinimumLevel( LogLevel.Debug );
    }

    private class Startup : IStartup
    {
      public void Configure( IApplicationBuilder app )
      {

        app.UsePerformanceCounter();

        app.Run( async context =>
        {
          await context.Response.WriteAsync( "Hello" );

        } );

      }

      public IServiceProvider ConfigureServices( IServiceCollection services )
      {
        services.AddPerformanceMonitor();
        services.AddSingleton<IGlobalPerformanceCollector, PerformanceLogsCollector>();
        return services.BuildServiceProvider();
      }
    }
  }
}
