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
  public class HttpPerformanceMiddleware
  {
    private readonly RequestDelegate nextHandler;

    public HttpPerformanceMiddleware( IPerformanceService service, RequestDelegate next )
    {
      Counter = new HttpPerformanceCounter();
      service.Register( Counter );
      nextHandler = next;
    }

    public HttpPerformanceCounter Counter { get; }

    public async Task InvokeAsync( HttpContext context )
    {

      var watch = new Stopwatch();
      watch.Start();
      await nextHandler( context );
      watch.Stop();


      Counter.OnRequestCompleted( watch.ElapsedMilliseconds, context.Response.StatusCode );
    }
  }
}
