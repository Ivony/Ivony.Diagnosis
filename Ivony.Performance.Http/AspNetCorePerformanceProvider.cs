using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Performance.Http
{

  /// <summary>
  /// ASP.NET Core 性能指标提供程序
  /// </summary>
  public class AspNetCorePerformanceProvider : IPerformanceProvider<AspNetCorePerformanceData>
  {
    public PerformanceMetric[] GetMetrics( AspNetCorePerformanceData data )
    {
      throw new NotImplementedException();
    }
  }
}
