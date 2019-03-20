using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Timer = System.Timers.Timer;

namespace Ivony.Performance
{


  /// <summary>
  /// 统一调度性能计数器和创建性能报告的性能报告服务
  /// </summary>
  public class PerformanceService : IPerformanceService
  {



    private Timer timer;


    /// <summary>
    /// 启动性能计数器服务
    /// </summary>
    /// <param name="cancellationToken">任务取消标识</param>
    /// <returns></returns>
    public Task StartAsync( CancellationToken cancellationToken = default( CancellationToken ) )
    {

      if ( ServiceProvider.GetService<AutoRegisterAllPerformanceCollector>() != null )
      {
        foreach ( var service in ServiceProvider.GetServices( typeof( IPerformanceSource<> ) ) )
        {
          var performanceType = service.GetType().GetGenericArguments()[0];
          var method = typeof( PerformanceServiceExtensions ).GetMethod( "Register" ).MakeGenericMethod( performanceType );

          method.Invoke( null, new[] { this, service } );
        }
      }

      timer = new Timer( Interval.TotalMilliseconds );
      timer.Elapsed += SendReport;
      timer.Start();

      return Task.CompletedTask;
    }


    /// <summary>
    /// 停止性能计数器服务
    /// </summary>
    /// <param name="cancellationToken">任务取消标识</param>
    /// <returns></returns>
    public Task StopAsync( CancellationToken cancellationToken = default( CancellationToken ) )
    {

      if ( timer != null )
      {
        timer.Stop();
        timer.Dispose();
      }
      return Task.CompletedTask;
    }







    /// <summary>
    /// 创建 PerformanceService 实例对象
    /// </summary>
    /// <param name="serviceProvider">系统服务提供程序</param>
    public PerformanceService( IServiceProvider serviceProvider ) : this( serviceProvider, TimeSpan.FromSeconds( 1 ) ) { }



    /// <summary>
    /// 创建 PerformanceService 实例对象
    /// </summary>
    /// <param name="serviceProvider">系统服务提供程序</param>
    /// <param name="interval">创建和推送性能报告的时间间隔</param>
    public PerformanceService( IServiceProvider serviceProvider, TimeSpan interval )
    {
      ServiceProvider = serviceProvider;
      Interval = interval;
      Logger = ServiceProvider.GetRequiredService<ILogger<PerformanceService>>();
    }


    /// <summary>
    /// 获取系统服务提供程序
    /// </summary>
    public IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// 创建和推送性能报告的时间间隔
    /// </summary>
    public TimeSpan Interval { get; }

    /// <summary>
    /// 错误日志记录器
    /// </summary>
    public ILogger<PerformanceService> Logger { get; }


    private IDictionary<object, IPerformanceSourceHost> hosts = new Dictionary<object, IPerformanceSourceHost>();




    /// <summary>
    /// 注册一个性能报告搜集器
    /// </summary>
    /// <typeparam name="TReport">性能报告类型</typeparam>
    /// <param name="source">性能报告源</param>
    /// <param name="collector">性能报告搜集器</param>
    /// <returns>返回一个 IDisposable 对象，用于取消注册性能报告搜集</returns>
    public IDisposable Register<TReport>( IPerformanceSource<TReport> source, IPerformanceCollector<TReport> collector ) where TReport : IPerformanceReport
    {
      var host = GetHost( source );
      return host.Register( collector );
    }


    private Host<TReport> GetHost<TReport>( IPerformanceSource<TReport> source ) where TReport : IPerformanceReport
    {
      lock ( sync )
      {
        if ( disposed )
          throw new ObjectDisposedException( "PerformanceSerivce" );

        if ( hosts.TryGetValue( source, out var host ) )
          return (Host<TReport>) host;

        return (Host<TReport>) (hosts[source] = new Host<TReport>( this, source ));
      }
    }



    /// <summary>
    /// 由 Timer 对象定期调用，发送性能报告。
    /// </summary>
    /// <param name="sender">事件源</param>
    /// <param name="e">时间参数</param>
    protected virtual void SendReport( object sender, ElapsedEventArgs e )
    {

      var now = DateTime.UtcNow;
      var hosts = this.hosts.Values.ToArray();
      Task.WhenAll( hosts.Select( item => Task.Run( () => item.SendReport( now ) ) ) )
        .ContinueWith( task =>
        {
          if ( task.IsFaulted )
            OnException( task.Exception );
        } );
    }

    /// <summary>
    /// 当发送性能报告出现异常时调用此方法
    /// </summary>
    /// <param name="exception">异常信息</param>
    protected virtual void OnException( AggregateException exception )
    {
      Logger.LogError( $"send performance report with error:\n{exception}" );
    }


    private readonly object sync = new object();
    private bool disposed = false;

    void IDisposable.Dispose()
    {
      lock ( sync )
      {
        disposed = true;

        StopAsync().Wait();

        foreach ( var item in hosts.Values.ToArray() )
        {
          item.Dispose();
        }
      }
    }


    private interface IPerformanceSourceHost : IDisposable
    {
      Task SendReport( DateTime timestamp );

    }

    private class Host<TReport> : IPerformanceSourceHost where TReport : IPerformanceReport
    {

      public Host( PerformanceService service, IPerformanceSource<TReport> source )
      {
        Service = service;
        Source = source;
      }

      public PerformanceService Service { get; }
      public IPerformanceSource<TReport> Source { get; }

      private const string ObjectName = "PerformanceSourceHost";
      private readonly object sync = new object();



      /// <summary>
      /// 向所有性能报告搜集器推送性能报告
      /// </summary>
      /// <returns>用于等待推送完成的 Task 对象</returns>
      public virtual async Task SendReport(  DateTime timestamp )
      {
        var report = await Source.CreateReportAsync();
        await Task.WhenAll( registrations.Select( item => item.SendReportAsync( Service, timestamp, report ) ) );
      }

      /// <summary>
      /// 注册一个性能报告搜集器
      /// </summary>
      /// <param name="collector">性能报告搜集器</param>
      /// <returns>返回一个 IDisposable 对象，调用其 Dispose 方法来取消注册</returns>
      public IDisposable Register( IPerformanceCollector<TReport> collector )
      {
        lock ( sync )
        {
          if ( disposed )
            throw new ObjectDisposedException( ObjectName );

          var registration = new Registration( this, collector );
          registrations.Add( registration );
          return registration;
        }
      }


      /// <summary>
      /// 注册多个性能报告搜集器
      /// </summary>
      /// <param name="collectors">性能报告搜集器列表</param>
      /// <returns>返回一个 IDisposable 对象，调用其 Dispose 方法来取消注册</returns>
      public IDisposable Register( params IPerformanceCollector<TReport>[] collectors )
      {
        lock ( sync )
        {
          if ( disposed )
            throw new ObjectDisposedException( ObjectName );

          var items = collectors.Select( item =>
          {
            var reg = new Registration( this, item );
            registrations.Add( reg );
            return reg;
          } ).ToArray();


          if ( items.Length == 1 )
            return items[0];

          else
            return new RegistrationSet( items );
        }
      }


      private bool disposed = false;
      private HashSet<Registration> registrations = new HashSet<Registration>();


      public void Dispose()
      {
        lock ( sync )
        {
          disposed = true;
          foreach ( var item in registrations.ToArray() )
            item.Dispose();
        }
      }


      private class RegistrationSet : IDisposable
      {
        private readonly Registration[] registrations;

        public RegistrationSet( Registration[] registrations )
        {
          this.registrations = registrations;
        }

        public void Dispose()
        {
          foreach ( var item in registrations )
            item.Dispose();
        }
      }



      private class Registration : IDisposable
      {
        public Registration( Host<TReport> host, IPerformanceCollector<TReport> collector )
        {
          Host = host;
          Collector = collector;
        }

        public Host<TReport> Host { get; }

        public IPerformanceCollector<TReport> Collector { get; }


        public void Dispose()
        {
          Host.registrations.Remove( this );
        }

        public Task SendReportAsync( IPerformanceService service, DateTime timestamp, TReport report )
        {
          return Collector.CollectReportAsync( service, timestamp, report );
        }
      }
    }


  }
}
