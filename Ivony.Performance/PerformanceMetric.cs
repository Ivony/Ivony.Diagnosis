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
    /// <param name="category">指标所属类型</param>
    /// <param name="metadata">指标元数据信息</param>
    /// <param name="value">指标值</param>
    /// <param name="unit">单位</param>
    public PerformanceMetric( string category, IReadOnlyDictionary<string, string> metadata, double value, PerformanceMetricUnit unit )
    {

      if ( unit.FormatString == null )
        throw new ArgumentException( "must specify unit.", "unit" );


      Category = category ?? throw new ArgumentNullException( nameof( category ) );
      Metadata = metadata ?? throw new ArgumentNullException( nameof( metadata ) );
      Value = value;
      Unit = unit;
    }



    /// <summary>
    /// 指标所属类型
    /// </summary>
    public string Category { get; }

    /// <summary>
    /// 指标元数据信息
    /// </summary>
    public IReadOnlyDictionary<string, string> Metadata { get; }


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
