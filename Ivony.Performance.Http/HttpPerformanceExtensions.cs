using System;
using System.Linq;
using System.Net.Http;
using Ivony.Performance;
using Ivony.Performance.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class HttpPerformanceExtensions
  {



    /// <summary>
    /// 为当前 ASP.NET Core 管线启用性能计数器
    /// </summary>
    /// <param name="builder">ASP.NET 管线构建器</param>
    /// <param name="counter">HTTP 性能计数器</param>
    /// <returns>ASP.NET 管线构建器</returns>
    public static IApplicationBuilder UsePerformanceCounter( this IApplicationBuilder builder, HttpPerformanceCounter counter = null )
    {



      counter = new HttpPerformanceCounter();
      builder.UseMiddleware<HttpPerformanceMiddleware>( counter );
      return builder;
    }


  }
}
