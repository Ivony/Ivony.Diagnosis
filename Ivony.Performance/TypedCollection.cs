using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Performance
{
  public sealed class TypedCollection
  {

    private Dictionary<Type, object> _dictionary = new Dictionary<Type, object>();


    public void AddList<T>( IReadOnlyList<T> list )
    { 
    
    }

  }
}
