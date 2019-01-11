using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace Ivony.Performance.Http
{
  public class HttpPerformanceDelegatingHandler : DelegatingHandler
  {

    private readonly HttpPerformanceCounter counter;



    public HttpPerformanceDelegatingHandler( ILogger logger ) : this( logger, null )
    {
    }

    public HttpPerformanceDelegatingHandler( ILogger logger, HttpMessageHandler handler ) : base( handler )
    {

      counter = new HttpPerformanceCounter();
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

    private class Observer : IObserver<IHttpPerformanceReport>
    {

      public void OnCompleted()
      {
      }

      public void OnError( Exception error )
      {
      }

      public void OnNext( IHttpPerformanceReport value )
      {
      }
    }
  }
}
