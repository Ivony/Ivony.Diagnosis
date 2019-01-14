using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Performance.Metrics
{

  /// <summary>
  /// 性能度量值
  /// </summary>
  public struct PerformanceMetric
  {

    /// <summary>
    /// 创建性能度量值实例
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="unit">单位</param>
    public PerformanceMetric( double value, PerformanceMetricUnit unit )
    {

      if ( unit.FormatString == null )
        throw new ArgumentException( "must specify unit.", "unit" );

      Value = value;
      Unit = unit;
    }


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



  public struct PerformanceMetricUnit
  {

    public PerformanceMetricUnit( PerformanceMetricUnitType type, string format )
    {
      Type = type;
      FormatString = format;
    }

    public PerformanceMetricUnitType Type { get; }

    public string FormatString { get; }

    public string FormatValue( double value )
    {
      return string.Format( FormatString, value );
    }


    public static readonly PerformanceMetricUnit rps = new PerformanceMetricUnit( PerformanceMetricUnitType.Scale, "{0} rps" );

    public static readonly PerformanceMetricUnit ms = new PerformanceMetricUnit( PerformanceMetricUnitType.Scale, "{0} ms" );

    public static readonly PerformanceMetricUnit percent = new PerformanceMetricUnit( PerformanceMetricUnitType.Scale, "{0:P}" );

    public static readonly PerformanceMetricUnit pcs = new PerformanceMetricUnit( PerformanceMetricUnitType.Scale, "{0}" );


  }


  public enum PerformanceMetricUnitType
  {
    Scale,
    Percent,
    Boolean
  }

}
