using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace Ivony.Diagnosis
{
  public class HttpPerformanceDelegatingHandler : DelegatingHandler
  {

    private static HttpPerformanceCounter counter = new HttpPerformanceCounter();



    public HttpPerformanceDelegatingHandler( ILogger logger ) : this( logger, null )
    {
    }

    public HttpPerformanceDelegatingHandler( ILogger logger, HttpMessageHandler handler ) : base( handler )
    {

      var timer = new System.Timers.Timer( 5000 );
      timer.Elapsed += ( sender, args ) =>
      {

        var old = counter;
        counter = new HttpPerformanceCounter();
        logger.LogInformation( old.CreateReport().ToString() );
      };

      timer.Start();

    }


    protected override async Task<HttpResponseMessage> SendAsync( HttpRequestMessage request, CancellationToken cancellationToken )
    {


      var watch = new Stopwatch();
      watch.Start();
      var response = await base.SendAsync( request, cancellationToken );
      watch.Stop();


      counter.OnRequestCompleted( watch.ElapsedMilliseconds, (int) response.StatusCode );

      return response;

    }


  }
}
