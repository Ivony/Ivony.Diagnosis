using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Performance
{

  /// <summary>
  /// 性能度量值
  /// </summary>
  public struct PerformanceMetric
  {

    /// <summary>
    /// 创建性能度量值实例
    /// </summary>
    /// <param name="datasource">数据源</param>
    /// <param name="aggregation">数据聚合方式</param>
    /// <param name="value">指标值</param>
    /// <param name="unit">单位</param>
    public PerformanceMetric( string datasource, string aggregation, double value, PerformanceMetricUnit unit )
    {

      if ( unit.FormatString == null )
        throw new ArgumentException( "must specify unit.", "unit" );

      Datasource = datasource ?? throw new ArgumentNullException( nameof( datasource ) );
      Aggregation = aggregation ?? throw new ArgumentNullException( nameof( aggregation ) );

      Value = value;
      Unit = unit;
    }

    /// <summary>
    /// 数据源
    /// </summary>
    public string Datasource { get; }

    /// <summary>
    /// 数据聚合方式
    /// </summary>
    public string Aggregation { get; }




    /// <summary>
    /// 度量值
    /// </summary>
    public double Value { get; }


    /// <summary>
    /// 度量单位
    /// </summary>
    public PerformanceMetricUnit Unit { get; }


    /// <summary>
    /// 获取度量值的字符串表达形式
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      return Unit.FormatValue( Value );
    }

  }


  /// <summary>
  /// 定义度量单位
  /// </summary>

  public struct PerformanceMetricUnit
  {

    /// <summary>
    /// 创建 PerformanceMetricUnit 实例
    /// </summary>
    /// <param name="type">单位类型</param>
    /// <param name="format">值样式</param>
    public PerformanceMetricUnit( PerformanceMetricUnitType type, string format )
    {
      Type = type;
      FormatString = format;
    }

    /// <summary>
    /// 单位类型
    /// </summary>
    public PerformanceMetricUnitType Type { get; }

    /// <summary>
    /// 值样式字符串
    /// </summary>
    public string FormatString { get; }

    /// <summary>
    /// 格式化值
    /// </summary>
    /// <param name="value">度量值</param>
    /// <returns></returns>
    public string FormatValue( double value )
    {
      return string.Format( FormatString, value );
    }


    /// <summary>
    /// 获取 rps 度量单位，每秒请求数
    /// </summary>
    public static readonly PerformanceMetricUnit rps = new PerformanceMetricUnit( PerformanceMetricUnitType.Scale, "{0} rps" );

    /// <summary>
    /// 获取 ms 度量单位，毫秒
    /// </summary>
    public static readonly PerformanceMetricUnit ms = new PerformanceMetricUnit( PerformanceMetricUnitType.Scale, "{0} ms" );

    /// <summary>
    /// 获取百分比度量单位
    /// </summary>
    public static readonly PerformanceMetricUnit percent = new PerformanceMetricUnit( PerformanceMetricUnitType.Percent, "{0:P}" );

    /// <summary>
    /// 获取 pieces 单位，代表一个计数项
    /// </summary>
    public static readonly PerformanceMetricUnit pcs = new PerformanceMetricUnit( PerformanceMetricUnitType.Scale, "{0}" );
  }


  /// <summary>
  /// 单位值类型
  /// </summary>
  public enum PerformanceMetricUnitType
  {
    /// <summary>
    /// 标量值
    /// </summary>
    Scale,

    /// <summary>
    /// 百分比
    /// </summary>
    Percent,

    /// <summary>
    /// 布尔值
    /// </summary>
    Boolean
  }

}
