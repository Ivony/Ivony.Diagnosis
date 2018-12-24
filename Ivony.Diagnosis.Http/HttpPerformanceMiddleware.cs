using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Ivony.Diagnosis.Http
{
  public class HttpPerformanceMiddleware : IMiddleware
  {
    private static HttpPerformanceCounter counter = new HttpPerformanceCounter();

    private readonly ILogger logger;


    public HttpPerformanceMiddleware( ILogger<HttpPerformanceMiddleware> logger )
    {
      this.logger = logger;

      counter.Subscribe( report => logger.LogInformation( report.ToString() ) );
      counter.Start();

    }

    public async Task InvokeAsync( HttpContext context, RequestDelegate next )
    {

      var watch = new Stopwatch();
      watch.Start();
      await next( context );
      watch.Stop();


      counter.OnRequestCompleted( watch.ElapsedMilliseconds, context.Response.StatusCode );
    }
  }
}
