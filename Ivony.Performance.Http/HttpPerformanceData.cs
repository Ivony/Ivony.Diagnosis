using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ivony.Performance.Http
{

  /// <summary>
  /// 定义 ASP.NET Core 性能数据
  /// </summary>
  public class HttpPerformanceData : IPerformanceData
  {


    private object[] _data;

    /// <summary>
    /// 创建 AspNetCorePerformanceData 实例
    /// </summary>
    /// <param name="dataSource">数据源名称</param>
    /// <param name="timeRange">时间范围</param>
    /// <param name="requests">时间范围内接收到的请求</param>
    /// <param name="completed">时间范围内已完成的请求</param>
    public HttpPerformanceData( string dataSource, DateTimeRange timeRange, IReadOnlyList<HttpRequestEntry> requests, HttpCompletedRequestEntry[] completed )
    {
      DataSource = dataSource;
      TimeRange = timeRange;

      _data = new[] { requests, completed };
    }

    /// <summary>
    /// 数据源名称
    /// </summary>
    public string DataSource { get; }

    /// <summary>
    /// 时间范围
    /// </summary>
    public DateTimeRange TimeRange { get; }

    /// <summary>
    /// 获取指定类型的计数项
    /// </summary>
    /// <typeparam name="T">计数项类型</typeparam>
    /// <returns>指定类型的计数项</returns>
    public IEnumerable<T> GetItems<T>()
    {
      return _data.OfType<T>();
    }

    /// <summary>
    /// 获取指定名称的性能数据
    /// </summary>
    /// <param name="name">数据源名称</param>
    /// <returns></returns>
    public IPerformanceData GetPerformanceData( string name )
    {
      return null;
    }


  }
}
