using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Ivony.Performance
{
  public class PerformanceEntryBag<T> : ICollection<T>
  {

    private ConcurrentBag<T> collection = new ConcurrentBag<T>();

    public int Count => collection.Count;

    public bool IsReadOnly => false;

    public void Add( T item ) => collection.Add( item );

    public void Clear() => throw new NotSupportedException();


    public bool Contains( T item ) => collection.Contains( item );

    public void CopyTo( T[] array, int arrayIndex ) => collection.CopyTo( array, arrayIndex );

    public IEnumerator<T> GetEnumerator() => collection.GetEnumerator();

    public bool Remove( T item ) => throw new NotSupportedException();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


    /// <summary>
    /// 返回目前已经搜集到的所有数据并清空容器
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<T> Dump()
    {
      return Interlocked.Exchange( ref collection, new ConcurrentBag<T>() ).ToArray();
    }
  }
}
