using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceCounterTest
{
  class Program
  {
    static void Main( string[] args )
    {

      foreach ( var item in PerformanceCounterCategory.GetCategories() )
      {
        Console.WriteLine( item.CategoryName );
      }

    }
  }
}
