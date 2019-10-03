using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ivony.Performance;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Ivony.Performance.Http
{
  public class HttpPerformanceMiddleware : IPerformanceSource
  {
    private readonly RequestDelegate nextHandler;

    public HttpPerformanceMiddleware( RequestDelegate next )
    {
      nextHandler = next;
    }



    private readonly PerformanceEntryBag<HttpRequestEntry> requests = new PerformanceEntryBag<HttpRequestEntry>();
    private readonly PerformanceEntryBag<HttpCompletedRequestEntry> completedRequests = new PerformanceEntryBag<HttpCompletedRequestEntry>();



    /// <summary>
    /// 重写 Invoke 方法，进行性能计数
    /// </summary>
    /// <param name="context">HTTP 上下文信息</param>
    /// <returns></returns>
    public async Task InvokeAsync( HttpContext context )
    {

      var watch = new Stopwatch();

      Begin( context, watch );

      try
      {
        await nextHandler( context );
      }
      finally
      {
        watch.Stop();
        End( context, watch );
      }

    }

    private void End( HttpContext context, Stopwatch watch )
    {
      watch.Stop();

      completedRequests.Add( new HttpCompletedRequestEntry { Path = context.Request.Path, Duration = watch.Elapsed, StatusCode = context.Response.StatusCode } );
    }

    private void Begin( HttpContext context, Stopwatch watch )
    {

      requests.Add( new HttpRequestEntry { Path = context.Request.Path } );
      watch.Start();
    }

    public IPerformanceData CreatePerformanceData( PerformanceContext context )
    {
      var r = requests.Pack();
      var c = completedRequests.Pack();

      return new HttpPerformanceData( "aspnet core", context.TimeRange, r, c );
    }


    /// <summary>
    /// 定义一个 HTTP 请求计数项
    /// </summary>
    private struct HttpRequestEntry
    {

      /// <summary>
      /// 请求路径
      /// </summary>
      public string Path { get; set; }

    }

    /// <summary>
    /// 定义一个 HTTP 请求完成计数项
    /// </summary>
    public struct HttpCompletedRequestEntry
    {

      /// <summary>
      /// 请求路径
      /// </summary>
      public string Path { get; set; }

      /// <summary>
      /// 请求时长
      /// </summary>
      public TimeSpan Duration { get; set; }

      /// <summary>
      /// 请求响应码
      /// </summary>
      public int StatusCode { get; set; }

    }
  }
}
