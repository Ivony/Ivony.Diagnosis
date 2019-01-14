using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Ivony.Performance.Metrics
{

  /// <summary>
  /// 定义指明度量值单位和类型的 Attribute 的基类型
  /// </summary>
  public abstract class MetricAttribute : Attribute
  {
    public abstract PerformanceMetric GetMetric( object report, PropertyInfo property );


    protected virtual T GetValue<T>( object report, PropertyInfo property )
    {
      return (T) property.GetValue( report );
    }

    protected virtual double GetValue( object report, PropertyInfo property )
    {
      var value = GetValue<IConvertible>( report, property );

      return value.ToDouble( CultureInfo.InvariantCulture );

    }

  }


  [AttributeUsage( AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
  public class Unit_msAttribute : MetricAttribute
  {
    public override PerformanceMetric GetMetric( object report, PropertyInfo property )
    {
      double value;

      if ( property.PropertyType == typeof( TimeSpan ) )
        value = GetValue<TimeSpan>( report, property ).TotalMilliseconds;

      else
        value = GetValue( report, property );

      return new PerformanceMetric( value, PerformanceMetricUnit.ms );
    }

  }


  [AttributeUsage( AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
  public class Unit_rpsAttribute : MetricAttribute
  {
    public override PerformanceMetric GetMetric( object report, PropertyInfo property )
    {
      var value = GetValue( report, property );
      return new PerformanceMetric( value, PerformanceMetricUnit.rps );
    }

  }

  [AttributeUsage( AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
  public class Unit_percentAttribute : MetricAttribute
  {
    public override PerformanceMetric GetMetric( object report, PropertyInfo property )
    {
      var value = GetValue( report, property );
      return new PerformanceMetric( value, PerformanceMetricUnit.percent );
    }

  }

  [AttributeUsage( AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
  public class Unit_pcsAttribute : MetricAttribute
  {
    public override PerformanceMetric GetMetric( object report, PropertyInfo property )
    {
      var value = GetValue( report, property );
      return new PerformanceMetric( value, PerformanceMetricUnit.pcs );
    }

  }


}
