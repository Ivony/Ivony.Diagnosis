using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Timer = System.Timers.Timer;

namespace Ivony.Performance
{


  /// <summary>
  /// 统一调度性能计数器和创建性能报告的性能报告服务
  /// </summary>
  public class PerformanceService : IPerformanceService
  {



    private Timer timer;

    private ConcurrentDictionary<IPerformanceSource, Host> hosts = new ConcurrentDictionary<IPerformanceSource, Host>();


    /// <summary>
    /// 启动性能计数器服务
    /// </summary>
    /// <param name="cancellationToken">任务取消标识</param>
    /// <returns></returns>
    public Task StartAsync( CancellationToken cancellationToken = default( CancellationToken ) )
    {


      foreach ( var source in ServiceProvider.GetServices<IPerformanceSource>() )
        Register( source );

      globalCollectors = ServiceProvider.GetServices<IGlobalPerformanceCollector>().ToArray();



      timer = new Timer( Interval.TotalMilliseconds );
      timer.Elapsed += SendReport;
      timer.Start();

      return Task.CompletedTask;
    }


    /// <summary>
    /// 获取所有性能报告源
    /// </summary>
    public IEnumerable<IPerformanceSource> Sources => hosts.Keys;



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
      Logger = ServiceProvider.GetService<ILogger<PerformanceService>>() ?? (ILogger) NullLogger.Instance;
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
    public ILogger Logger { get; }


    private IReadOnlyCollection<IGlobalPerformanceCollector> globalCollectors = new HashSet<IGlobalPerformanceCollector>();




    /// <summary>
    /// 注册一个性能报告搜集器
    /// </summary>
    /// <param name="source">性能报告源</param>
    /// <returns>返回一个 IDisposable 对象，用于取消注册性能报告搜集</returns>
    public IPerformanceCollectorRegistrationHost Register( IPerformanceSource source )
    {

      return hosts.GetOrAdd( source, CreateHost );

    }



    /// <summary>
    /// 注册一个性能报告搜集器
    /// </summary>
    /// <param name="source">性能报告源</param>
    /// <param name="collector">性能报告搜集器</param>
    /// <returns>返回一个 IDisposable 对象，用于取消注册性能报告搜集</returns>
    public IPerformanceCollectorRegistration Register( IPerformanceSource source, IPerformanceCollector collector )
    {
      var host = hosts.GetOrAdd( source, CreateHost );
      return host.Register( collector );
    }



    private Host CreateHost( IPerformanceSource source )
    {
      var factories = ServiceProvider.GetServices<IPerformanceCollectorFactory>();
      var collectors = factories.SelectMany( factory => factory.GetReportCollectors( source ) );

      return new Host( this, source, collectors );

    }



    /// <summary>
    /// 获取注册在指定性能报告源的所有收集器
    /// </summary>
    /// <param name="source">性能报告源</param>
    /// <returns></returns>
    public IPerformanceCollectorRegistrationHost GetRegistrations( IPerformanceSource source )
    {
      if ( hosts.TryGetValue( source, out var host ) )
        return host;

      else
        return null;

    }





    /// <summary>
    /// 由 Timer 对象定期调用，发送性能报告。
    /// </summary>
    /// <param name="sender">事件源</param>
    /// <param name="_"></param>
    protected virtual async void SendReport( object sender, ElapsedEventArgs _ )
    {

      var timestamp = DateTime.UtcNow;

      var list = hosts.Values.ToArray();

      try
      {

        var results = await Task.WhenAll( list.Select( async item => new { report = await item.CreateReportAsync(), host = item } ) );
        results = results.Where( item => item.report != null ).ToArray();


        var reports = results.Select( item => item.report ).ToArray();
        await Task.WhenAll( results.Select( item => item.host.SendReport( timestamp, item.report ) ) );


        await GlobalReportCollect( timestamp, reports );


      }
      catch ( Exception e )
      {
        OnException( e );
      }




    }

    private async Task GlobalReportCollect( DateTime timestamp, IReadOnlyList<IPerformanceReport> reports )
    {
      try
      {

        var collectors = ServiceProvider.GetServices<IGlobalPerformanceCollector>();
        await Task.WhenAll( collectors.Select( item => item.CollectReportsAsync( this, timestamp, reports.ToArray() ) ) );
      }
      catch ( Exception e )
      {
        OnException( e );
      }

    }

    /// <summary>
    /// 当发送性能报告出现异常时调用此方法
    /// </summary>
    /// <param name="exception">异常信息</param>
    protected virtual void OnException( Exception exception )
    {
      Logger.LogError( $"send performance report with error:\n{exception}" );
    }


    private readonly object sync = new object();
    private bool disposed = false;
    private IEnumerable<object> result;

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

    private class Host : IPerformanceCollectorRegistrationHost
    {


      private bool disposed = false;
      private readonly HashSet<Registration> _registrations;
      private readonly PerformanceService _service;


      private readonly object _sync = new object();
      private readonly string _name;
      private readonly ILogger _logger;

      public IPerformanceSource Source { get; }

      public IPerformanceService Service => _service;

      public Host( PerformanceService service, IPerformanceSource source, IEnumerable<IPerformanceCollector> collectors )
      {
        Source = source;

        _service = service;
        _registrations = new HashSet<Registration>( collectors.Select( item => new Registration( this, item ) ) );

        _name = $"Ivony.Performance.PerformanceHost[{source.SourceName}]";
        _logger = service.ServiceProvider.GetService<ILoggerFactory>()?.CreateLogger( _name ) ?? NullLogger.Instance;

      }




      public IReadOnlyList<IPerformanceCollectorRegistration> Registrations => _registrations.ToArray();


      /// <summary>
      /// 创建性能报告
      /// </summary>
      /// <returns>性能报告</returns>
      public async Task<IPerformanceReport> CreateReportAsync()
      {
        try
        {
          return await Source.CreateReportAsync();
        }
        catch ( Exception e )
        {
          _logger.LogError( "unhandled exception in create report:" + e );
          return null;
        }
      }



      /// <summary>
      /// 向所有性能报告搜集器推送性能报告
      /// </summary>
      /// <returns>用于等待推送完成的 Task 对象</returns>
      public virtual async Task SendReport( DateTime timestamp, IPerformanceReport report )
      {
        try
        {

          Registration[] list;

          lock ( _sync )
          {
            list = _registrations.ToArray();
          }

          await Task.WhenAll( list.Select( registration => registration.SendReportAsync( Service, timestamp, report ) ) );

        }
        catch ( Exception e )
        {
          _logger.LogError( "unhandled exception in send report:" + e );
        }
      }





      /// <summary>
      /// 注册一个性能报告搜集器
      /// </summary>
      /// <param name="collector">性能报告搜集器</param>
      /// <returns>返回一个 IDisposable 对象，调用其 Dispose 方法来取消注册</returns>
      public IPerformanceCollectorRegistration Register( IPerformanceCollector collector )
      {
        lock ( _sync )
        {
          if ( disposed )
            throw new ObjectDisposedException( _name );

          var registration = new Registration( this, collector );
          _registrations.Add( registration );
          return registration;
        }
      }



      public void Dispose()
      {
        lock ( _sync )
        {
          disposed = true;
          _service.RemoveHost( this );
          _registrations.Clear();
        }
      }



      private class Registration : IPerformanceCollectorRegistration
      {
        public Registration( Host host, IPerformanceCollector collector )
        {
          Host = host;
          Collector = collector;
        }

        public Host Host { get; }


        public IPerformanceSource Source => Host.Source;


        public IPerformanceCollector Collector { get; }


        public void Dispose()
        {
          Host._registrations.Remove( this );
        }

        public Task SendReportAsync( IPerformanceService service, DateTime timestamp, IPerformanceReport report )
        {
          return Collector.CollectReportAsync( service, timestamp, report );
        }
      }
    }

    private void RemoveHost( Host host )
    {
      hosts.TryRemove( host.Source, out _ );

    }
  }
}
