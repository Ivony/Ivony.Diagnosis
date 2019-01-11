using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Ivony.Performance
{

  /// <summary>
  /// 性能计数器抽象
  /// </summary>
  /// <typeparam name="TReport">性能报告格式</typeparam>
  public interface IPerformanceCounter<TReport> where TReport : IPerformanceReport
  {

    Task<TReport> CreateReportAsync();

  }
}