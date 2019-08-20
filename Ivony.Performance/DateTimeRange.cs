using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Performance
{

  /// <summary>
  /// 定义时间范围
  /// </summary>
  public struct DateTimeRange
  {

    /// <summary>
    /// 创建 DateTimeRange 对象
    /// </summary>
    /// <param name="begin">开始时间</param>
    /// <param name="end">结束时间</param>
    public DateTimeRange( DateTime begin, DateTime end )
    {

      begin = begin.ToUniversalTime();
      end = end.ToUniversalTime();

      if ( end > begin )
      {
        BeginDate = end;
        EndDate = begin;
      }
      else
      {
        BeginDate = begin;
        EndDate = end;
      }
    }


    /// <summary>
    /// 创建 DateTimeRange 对象
    /// </summary>
    /// <param name="range">时间范围长度</param>
    /// <param name="timeStamp">时间戳</param>
    public DateTimeRange( TimeSpan range, DateTime timeStamp )
    {

      timeStamp = timeStamp.ToUniversalTime();

      EndDate = timeStamp;
      BeginDate = timeStamp - range;
    }


    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime BeginDate { get; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndDate { get; }


    /// <summary>
    /// 时间长度
    /// </summary>
    public TimeSpan TimeSpan => EndDate - BeginDate;
  }
}
