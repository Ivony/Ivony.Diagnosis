using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.Performance
{
  public class PerformanceService : IPerformanceService, IHostedService
  {
    private readonly IPerformanceSource[] _sources;
    private readonly IPerformanceCollector[] _collectors;
    private readonly IPerformanceProvider[] _providers;

    private readonly System.Timers.Timer _timer;

    public PerformanceService( IEnumerable<IPerformanceSource> sources, IEnumerable<IPerformanceProvider> providers, IEnumerable<IPerformanceCollector> collectors )
    {
      _sources = sources.ToArray();
      _collectors = collectors.ToArray();
      _providers = providers.ToArray();

      _timer = new System.Timers.Timer( TimeSpan.FromSeconds( 1 ).TotalMilliseconds );
      _timer.Elapsed += OnClock;
    }

    private void OnClock( object sender, System.Timers.ElapsedEventArgs e )
    {

      var context = new PerformanceContext{ 
      _sources.Select ( item => item.CreatePerformanceData( 

    }

    public IPerformanceSource[] PerformanceSources => _sources;

    public IPerformanceCollector[] PerformanceCollectors => _collectors;



    Task IHostedService.StartAsync( CancellationToken cancellationToken )
    {
      _timer.Start();
      return Task.CompletedTask;
    }

    Task IHostedService.StopAsync( CancellationToken cancellationToken )
    {
      _timer.Stop();
      return Task.CompletedTask;
    }
  }
}
