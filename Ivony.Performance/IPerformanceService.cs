﻿using System;
using Microsoft.Extensions.Hosting;

namespace Ivony.Performance
{
  public interface IPerformanceService : IHostedService, IDisposable
  {


    IServiceProvider ServiceProvider { get; }

    IDisposable Register<TReport>( IPerformanceCounter<TReport> counter, IPerformanceReportCollector<TReport> collector ) where TReport : IPerformanceReport;
    IDisposable Register<TReport>( IPerformanceCounter<TReport> counter, IPerformanceReportCollector<TReport>[] collectors ) where TReport : IPerformanceReport;
  }
}