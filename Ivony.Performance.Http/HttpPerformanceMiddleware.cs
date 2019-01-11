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
  public class HttpPerformanceMiddleware : IMiddleware
  {



    public HttpPerformanceMiddleware( PerformanceService service )
    {
      Counter = new HttpPerformanceCounter();
      service.Register( Counter );
    }

    public HttpPerformanceCounter Counter { get; }

    public async Task InvokeAsync( HttpContext context, RequestDelegate next )
    {

      var watch = new Stopwatch();
      watch.Start();
      await next( context );
      watch.Stop();


      Counter.OnRequestCompleted( watch.ElapsedMilliseconds, context.Response.StatusCode );
    }
  }
}
