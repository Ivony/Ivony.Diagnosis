using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Ivony.Performance.Http;
using Microsoft.Extensions.Logging;

namespace Ivony.Diagnosis.LoadTestConsole
{
  class Program
  {
    public static async Task Main( string[] args )
    {
      if ( args.Length < 1 )
      {
        Console.WriteLine( "url is required" );
        return;
      }

      var url = args[0];

      var size = 100000;
      if ( args.Length > 1 )
        size = int.Parse( args[1] );


      var degree = 10;
      if ( args.Length > 2 )
        degree = int.Parse( args[2] );

      var client = new HttpClient( new HttpPerformanceDelegatingHandler( new ConsoleLogger(), new HttpClientHandler() ) );


      var block = new ActionBlock<int>( async i => await client.GetAsync( string.Format( url, i ) ), new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = degree } );
      foreach ( var i in Enumerable.Range( 0, size ) )
        block.Post( i );

      block.Complete();
      await block.Completion;
    }

    private class ConsoleLogger : ILogger
    {
      public IDisposable BeginScope<TState>( TState state )
      {
        return null;
      }

      public bool IsEnabled( LogLevel logLevel )
      {
        return true;
      }

      public void Log<TState>( LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter )
      {
        Console.WriteLine( formatter( state, exception ) );
      }
    }
  }
}
