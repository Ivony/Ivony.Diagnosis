using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Ivony.Diagnosis
{
  public class HttpPerformanceMiddleware : IMiddleware
  {
    private static HttpPerformanceCounter counter = new HttpPerformanceCounter();

    private ILogger logger;


    public HttpPerformanceMiddleware( ILogger<HttpPerformanceMiddleware> logger )
    {
      this.logger = logger;

      var timer = new System.Timers.Timer( 1000 );
      timer.Elapsed += ( sender, args ) =>
      {

        var old = counter;
        counter = new HttpPerformanceCounter();
        logger.LogInformation( old.CreateReport().ToString() );
      };

      timer.Start();

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
