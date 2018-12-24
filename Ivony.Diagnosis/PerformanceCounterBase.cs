using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Ivony.Diagnosis
{

  /// <summary>
  /// 定义性能计数器基类型
  /// </summary>
  public abstract class PerformanceCounterBase<TEntry, TReport> : IPerformanceCounter<TReport>
  {

    private ConcurrentBag<TEntry> counter;
    private Timer timer;
    private DateTime timestamp;
    private bool availables;


    public PerformanceCounterBase( TimeSpan interval )
    {
      timer = new Timer( interval.TotalMilliseconds );
      counter = new ConcurrentBag<TEntry>();

      timer.Elapsed += Report;
    }

    private void Report( object sender, ElapsedEventArgs e )
    {
      var _counter = counter;
      counter = new ConcurrentBag<TEntry>();

      var begin = timestamp;
      var end = timestamp = DateTime.UtcNow;

      System.Threading.Thread.MemoryBarrier();



      var report = CreateReport( begin, end, _counter.ToArray() );
      //foreach ( var item in observerRegistrations )
      //  PushReport( item, report );

      observerRegistrations.AsParallel().ForAll( registration => PushReport( registration, report ) );
    }

    protected virtual void PushReport( Registration registration, TReport report )
    {
      registration.Observer?.Next( report );
    }

    public void Start()
    {
      availables = true;
      timestamp = DateTime.UtcNow;
      timer.Start();

    }


    public void Stop()
    {
      availables = false;
      timer.Stop();
    }



    protected abstract TReport CreateReport( DateTime start, DateTime end, TEntry[] data );

    protected void Increase( TEntry entry )
    {
      if ( availables )
        counter.Add( entry );
    }


    private HashSet<Registration> observerRegistrations = new HashSet<Registration>();

    public IDisposable Subscribe( IPerformanceReportObserver<TReport> observer )
    {
      var registration = new Registration( observer );
      observerRegistrations.Add( registration );
      return registration;
    }



    protected class Registration : IDisposable
    {
      private IPerformanceReportObserver<TReport> reference;

      public Registration( IPerformanceReportObserver<TReport> observer )
      {
        if ( observer == null )
          throw new ArgumentNullException( nameof( observer ) );

        reference = observer;
      }

      public IPerformanceReportObserver<TReport> Observer => reference;

      public void Dispose()
      {
        reference = null;
      }
    }

  }
}
