using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Ivony.Performance.Http
{
  public class AspNetCorePerformanceCounter : IPerformanceCounter
  {
    public IPerformanceData Collect( PerformanceContext context )
    {
      throw new NotImplementedException();
    }

  }
}
