using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Performance
{

  /// <summary>
  /// 性能数据搜集上下文
  /// </summary>
  public sealed class PerformanceContext
  {

    /// <summary>
    /// 性能监控服务
    /// </summary>
    IPerformanceService PerformanceService { get; }

    /// <summary>
    /// 此次性能数据搜集的时间范围
    /// </summary>
    public DateTimeRange TimeRange { get; }

  }
}
