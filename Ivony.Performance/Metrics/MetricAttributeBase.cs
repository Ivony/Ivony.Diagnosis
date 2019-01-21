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
  public abstract class MetricAttributeBase : Attribute
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
  public class MetricAttribute : MetricAttributeBase
  {

    public MetricAttribute( Type providerType )
    {
      Instance = (IPerformanceMetricProvider) Activator.CreateInstance( providerType );

    }

    public IPerformanceMetricProvider Instance { get; }

    public override PerformanceMetric GetMetric( object report, PropertyInfo property )
    {
      return Instance.CreateMetric( report, property );
    }
  }



  [AttributeUsage( AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
  public class UnitAttribute : MetricAttributeBase
  {

    public UnitAttribute( PerformanceMetricUnitType type, string format )
    {
      Unit = new PerformanceMetricUnit( type, format );
    }

    public PerformanceMetricUnit Unit { get; }

    public override PerformanceMetric GetMetric( object report, PropertyInfo property )
    {
      return new PerformanceMetric( GetValue( report, property ), Unit );
    }
  }



  [AttributeUsage( AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
  public class Unit_msAttribute : MetricAttributeBase
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
  public class Unit_rpsAttribute : MetricAttributeBase
  {
    public override PerformanceMetric GetMetric( object report, PropertyInfo property )
    {
      var value = GetValue( report, property );
      return new PerformanceMetric( value, PerformanceMetricUnit.rps );
    }

  }

  [AttributeUsage( AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
  public class Unit_percentAttribute : MetricAttributeBase
  {
    public override PerformanceMetric GetMetric( object report, PropertyInfo property )
    {
      var value = GetValue( report, property );
      return new PerformanceMetric( value, PerformanceMetricUnit.percent );
    }

  }

  [AttributeUsage( AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
  public class Unit_pcsAttribute : MetricAttributeBase
  {
    public override PerformanceMetric GetMetric( object report, PropertyInfo property )
    {
      var value = GetValue( report, property );
      return new PerformanceMetric( value, PerformanceMetricUnit.pcs );
    }

  }


}
