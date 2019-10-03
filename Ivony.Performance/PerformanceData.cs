using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Performance
{



  /// <summary>
  /// IPerformanceData 的标准实现
  /// </summary>
  /// <typeparam name="TEntry">计数项类型</typeparam>
  public class PerformanceData<TEntry> : IPerformanceData
  {
    private IReadOnlyList<TEntry> _items;

    /// <summary>
    /// 创建 PerformanceData 对象
    /// </summary>
    /// <param name="dataSsource">数据源名称</param>
    /// <param name="timeRange">时间范围</param>
    /// <param name="entries">计数项</param>
    public PerformanceData( string dataSsource, DateTimeRange timeRange, IReadOnlyList<TEntry> entries )
    {
      DataSource = dataSsource;
      TimeRange = timeRange;
      _items = entries;
    }


    public string DataSource { get; }

    public DateTimeRange TimeRange { get; }

    public IReadOnlyList<T> GetEntries<T>()
    {
      if ( typeof( T ) == typeof( TEntry ) )
        return (IReadOnlyList<T>) _items;

      else
        return Empty<T>.EmptyList;
    }


    private class Empty<T>
    {
      public static T[] EmptyList { get; } = new T[0];
    }


    public IPerformanceData GetPerformanceData( string name )
    {
      return null;
    }
  }
}
