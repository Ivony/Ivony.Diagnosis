﻿using System.Net.Http;
using Ivony.Performance;
using Ivony.Performance.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class HttpPerformanceExtensions
  {



    /// <summary>
    /// 为 HttpMessageHandler 添加性能计数器
    /// </summary>
    /// <param name="handler">要添加性能计数器的 HttpMessageHandler</param>
    /// <returns></returns>
    public static HttpPerformanceDelegatingHandler WithPerformanceCounter( this HttpMessageHandler handler )
    {
      return new HttpPerformanceDelegatingHandler( handler );
    }



    /// <summary>
    /// 为当前 ASP.NET Core 管线启用性能计数器
    /// </summary>
    /// <param name="builder">ASP.NET 管线构建器</param>
    /// <returns>ASP.NET 管线构建器</returns>
    public static IApplicationBuilder UsePerformanceCounter( this IApplicationBuilder builder )
    {

      builder.UseMiddleware<HttpPerformanceMiddleware>();
      return builder;

    }
  }
}
