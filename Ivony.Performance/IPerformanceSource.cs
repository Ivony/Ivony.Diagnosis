﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Ivony.Performance
{



  /// <summary>
  /// 定义性能报告源抽象
  /// </summary>
  public interface IPerformanceSource
  {
    /// <summary>
    /// 源名称
    /// </summary>
    string SourceName { get; }

  }


  /// <summary>
  /// 定义性能报告源抽象
  /// </summary>
  /// <typeparam name="TReport">性能报告格式</typeparam>
  public interface IPerformanceSource<TReport> : IPerformanceSource where TReport : IPerformanceReport
  {

    /// <summary>
    /// 创建性能报告
    /// </summary>
    /// <returns></returns>
    Task<TReport> CreateReportAsync();

  }
}