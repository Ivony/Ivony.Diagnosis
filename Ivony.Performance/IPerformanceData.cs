using System;
using System.Collections;
using System.Collections.Generic;

namespace Ivony.Performance
{
  /// <summary>
  /// 性能数据
  /// </summary>
  public interface IPerformanceData
  {

    /// <summary>
    /// 数据源名称
    /// </summary>
    string DataSource { get; }


    /// <summary>
    /// 性能数据时间范围
    /// </summary>
    DateTimeRange TimeRange { get; }

    /// <summary>
    /// 获取指定名称的性能数据
    /// </summary>
    /// <param name="name">性能数据名称</param>
    /// <returns>指定名称的性能数据</returns>
    IPerformanceData GetPerformanceData( string name );


    /// <summary>
    /// 获取指定类型的计数项
    /// </summary>
    IReadOnlyList<T> GetEntries<T>();

  }
}