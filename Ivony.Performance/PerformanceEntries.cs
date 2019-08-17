using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Performance
{
  /// <summary>
  /// 定义性能计数项集合
  /// </summary>
  /// <typeparam name="TEntry">性能计数项类型</typeparam>
  public class PerformanceEntries<TEntry> : IReadOnlyList<TEntry>
  {
    private readonly TEntry[] _entries;


    internal PerformanceEntries( TEntry[] entries )
    {
      _entries = entries;
    }

    /// <summary>
    /// 获取指定索引的项
    /// </summary>
    /// <param name="index">索引位置</param>
    /// <returns></returns>
    public TEntry this[int index] => ((IReadOnlyList<TEntry>) _entries)[index];

    /// <summary>
    /// 计数项数量
    /// </summary>
    public int Count => ((IReadOnlyList<TEntry>) _entries).Count;

    /// <summary>
    /// 获取计数项枚举器
    /// </summary>
    /// <returns></returns>
    public IEnumerator<TEntry> GetEnumerator()
    {
      return ((IReadOnlyList<TEntry>) _entries).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((IReadOnlyList<TEntry>) _entries).GetEnumerator();
    }
  }
}
