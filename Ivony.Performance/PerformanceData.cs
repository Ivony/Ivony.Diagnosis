using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Ivony.Performance
{
  public class PerformanceData : IPerformanceData
  {

    private IReadOnlyList<object> _data;


    private ConcurrentDictionary<Type, object> _cache = new ConcurrentDictionary<Type, object>();


    public PerformanceData( string dataSource, DateTimeRange timeRange, IEnumerable data )
    {
      DataSource = dataSource;
      TimeRange = timeRange;

      _data = data.Cast<object>().ToArray();
    }


    /// <summary>
    /// 数据源名称
    /// </summary>
    public string DataSource { get; }


    /// <summary>
    /// 性能数据时间范围
    /// </summary>
    public DateTimeRange TimeRange { get; }


    /// <summary>
    /// 获取指定类型的计数项
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IReadOnlyList<T> GetItems<T>()
    {
      return (IReadOnlyList<T>) _cache.GetOrAdd( typeof( T ), _ => new ReadOnlyCollection<T>( _data.OfType<T>().ToArray() ) );
    }


    /// <summary>
    /// 获取指定名称的性能数据
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IPerformanceData GetPerformanceData( string name )
    {
      return null;
    }
  }
}
