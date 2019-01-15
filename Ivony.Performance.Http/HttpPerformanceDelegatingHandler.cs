using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace Ivony.Performance.Http
{
  /// <summary>
  /// 用于监控 HTTP 性能的 HTTP 消息处理器
  /// </summary>
  public class HttpPerformanceDelegatingHandler : DelegatingHandler
  {


    /// <summary>
    /// 创建 HttpPerformanceDelegatingHandler 对象
    /// </summary>
    /// <param name="handler"></param>
    public HttpPerformanceDelegatingHandler( HttpMessageHandler handler ) : base( handler )
    {
      Counter = new HttpPerformanceCounter();
    }

    /// <summary>
    /// 性能计数器
    /// </summary>
    public HttpPerformanceCounter Counter { get; }


    /// <summary>
    /// 重写此方法发送 HTTP 请求
    /// </summary>
    /// <param name="request">请求消息</param>
    /// <param name="cancellationToken">取消标识</param>
    /// <returns>用于等待的 Task 对象</returns>
    protected override async Task<HttpResponseMessage> SendAsync( HttpRequestMessage request, CancellationToken cancellationToken )
    {


      var watch = new Stopwatch();
      watch.Start();
      var response = await base.SendAsync( request, cancellationToken );
      watch.Stop();


      Counter.OnRequestCompleted( watch.ElapsedMilliseconds, (int) response.StatusCode );

      return response;

    }
  }
}
