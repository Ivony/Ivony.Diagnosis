using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Performance
{

  /// <summary>
  /// 定义一个性能计数器
  /// </summary>
  /// <typeparam name="T">性能计数项类型</typeparam>
  public interface IPerformanceCounter<T>
  {

    /// <summary>
    /// 记录一个性能计数项
    /// </summary>
    /// <param name="entry">性能计数项</param>
    void Collect( T entry );
  }
}
