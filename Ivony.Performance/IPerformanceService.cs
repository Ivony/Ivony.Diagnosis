using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Performance
{
  public interface IPerformanceService
  {

    /// <summary>
    /// 获取所有性能数据源
    /// </summary>
    IPerformanceSource[] PerformanceSources { get; }
    
    /// <summary>
    /// 获取所有性能指标搜集器
    /// </summary>
    IPerformanceCollector[] PerformanceCollectors { get; }


  }
}
