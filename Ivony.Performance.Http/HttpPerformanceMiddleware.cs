using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Ivony.Performance;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Ivony.Performance.Http
{
  public class HttpPerformanceMiddleware : IDisposable
  {
    private readonly RequestDelegate nextHandler;

    public HttpPerformanceMiddleware( IPerformanceService service, RequestDelegate next, HttpPerformanceCounter counter )
    {
      _counter = counter;
      _registry = service.Register( _counter );
      nextHandler = next;
    }

    private readonly HttpPerformanceCounter _counter;

    private readonly IDisposable _registry;

    void IDisposable.Dispose()
    {
      _registry.Dispose();
    }



    /// <summary>
    /// 重写 Invoke 方法，进行性能计数
    /// </summary>
    /// <param name="context">HTTP 上下文信息</param>
    /// <returns></returns>
    public async Task InvokeAsync( HttpContext context )
    {

      var watch = new Stopwatch();
      watch.Start();
      await nextHandler( context );
      watch.Stop();


      _counter.OnRequestCompleted( watch.ElapsedMilliseconds, context.Response.StatusCode );
    }
  }
}
