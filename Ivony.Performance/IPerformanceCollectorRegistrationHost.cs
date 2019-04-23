using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Performance
{
  public interface IPerformanceCollectorRegistrationHost : IDisposable
  {


    IPerformanceService Service { get; }

    IPerformanceSource Source { get; }

    IReadOnlyList<IPerformanceCollectorRegistration> Registrations { get; }

  }
}
