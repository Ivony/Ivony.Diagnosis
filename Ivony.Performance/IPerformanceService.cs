using System;
using Microsoft.Extensions.Hosting;

namespace Ivony.Performance
{
  public interface IPerformanceService : IHostedService, IDisposable
  {


    IServiceProvider ServiceProvider { get; }

    IDisposable Register<TReport>( IPerformanceSource<TReport> counter, IPerformanceCollector<TReport> collector ) where TReport : IPerformanceReport;
    IDisposable Register<TReport>( IPerformanceSource<TReport> counter, IPerformanceCollector<TReport>[] collectors ) where TReport : IPerformanceReport;
  }
}