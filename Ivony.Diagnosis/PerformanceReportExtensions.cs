using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Diagnosis
{
  public static class PerformanceReportExtensions
  {


    public static void Subscribe<TReport>( this IPerformanceCounter<TReport> counter, Action<TReport> observer )
    {
      counter.Subscribe( new Observer<TReport>( observer ) );
    }

    private class Observer<TReport> : IPerformanceReportObserver<TReport>
    {
      private Action<TReport> _action;

      public Observer( Action<TReport> action )
      {
        _action = action;
      }

      public void Next( TReport report )
      {
        _action( report );
      }
    }
  }
}
