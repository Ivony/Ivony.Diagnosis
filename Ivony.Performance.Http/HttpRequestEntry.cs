using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Ivony.Performance.Http
{

  /// <summary>
  /// 定义一个 HTTP 请求计数项
  /// </summary>
  public class HttpRequestEntry
  {

    /// <summary>
    /// 请求路径
    /// </summary>
    public string Path { get; }


    /// <summary>
    /// 请求当前是否已经完成
    /// </summary>
    public bool IsCompleted { get; }


  }

  /// <summary>
  /// 定义一个 HTTP 请求完成计数项
  /// </summary>
  public class HttpCompletedRequestEntry : HttpRequestEntry
  {
    /// <summary>
    /// 请求时长
    /// </summary>
    public TimeSpan Duration { get; }

    /// <summary>
    /// 请求响应码
    /// </summary>
    public HttpStatusCode StatusCode { get; }
  }

}
