using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Collections.Concurrent;



using Timer = System.Timers.Timer;
using System.Timers;
using Microsoft.Extensions.Logging;

namespace Ivony.Performance
{


  /// <summary>
  /// 统一调度性能计数器和创建性能报告的性能报告服务
  /// </summary>
  public class PerformanceService : IHostedService
  {



    private Timer timer;

    public async Task StartAsync( CancellationToken cancellationToken )
    {
      timer = new Timer( Interval.TotalMilliseconds );
      timer.Elapsed += SendReport;
      timer.Start();
    }

    public async Task StopAsync( CancellationToken cancellationToken )
    {
      timer.Stop();
      timer.Dispose();
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


    private ConcurrentDictionary<object, IPerformanceCounterHost> counters = new ConcurrentDictionary<object, IPerformanceCounterHost>();






    /// <summary>
    /// 从系统服务中查找性能报告搜集器并注册
    /// </summary>
    /// <typeparam name="TReport">性能报告类型</typeparam>
    /// <param name="counter">性能计数器</param>
    /// <param name="collector">性能报告搜集器</param>
    /// <returns>返回一个 IDisposable 对象，用于取消注册性能报告搜集</returns>
    public IDisposable Register<TReport>( IPerformanceCounter<TReport> counter ) where TReport : IPerformanceReport
    {
      return Register( counter, ServiceProvider.GetServices<IPerformanceReportCollector<TReport>>().ToArray() );
    }



    /// <summary>
    /// 注册多个性能报告搜集器
    /// </summary>
    /// <typeparam name="TReport">性能报告类型</typeparam>
    /// <param name="counter">性能计数器</param>
    /// <param name="collector">性能报告搜集器</param>
    /// <returns>返回一个 IDisposable 对象，用于取消注册性能报告搜集</returns>
    public IDisposable Register<TReport>( IPerformanceCounter<TReport> counter, params IPerformanceReportCollector<TReport>[] collectors ) where TReport : IPerformanceReport
    {
      var host = (Host<TReport>) counters.GetOrAdd( counter, new Host<TReport>( counter ) );
      return host.Register( collectors );
    }


    /// <summary>
    /// 注册一个性能报告搜集器
    /// </summary>
    /// <typeparam name="TReport">性能报告类型</typeparam>
    /// <param name="counter">性能计数器</param>
    /// <param name="collector">性能报告搜集器</param>
    /// <returns>返回一个 IDisposable 对象，用于取消注册性能报告搜集</returns>
    public IDisposable Register<TReport>( IPerformanceCounter<TReport> counter, IPerformanceReportCollector<TReport> collector ) where TReport : IPerformanceReport
    {
      var host = (Host<TReport>) counters.GetOrAdd( counter, new Host<TReport>( counter ) );
      return host.Register( collector );
    }




    protected virtual void SendReport( object sender, ElapsedEventArgs e )
    {
      var hosts = counters.Values.ToArray();
      Task.WhenAll( hosts.Select( item => Task.Run( () => item.SendReport() ) ) )
        .ContinueWith( task =>
        {
          if ( task.IsFaulted )
            OnException( task.Exception );
        } );
    }

    protected virtual void OnException( AggregateException exception )
    {
      Logger.LogError( $"send performance report with error:\n{exception}" );
    }

    private interface IPerformanceCounterHost
    {
      Task SendReport();

    }

    private class Host<TReport> : IPerformanceCounterHost where TReport : IPerformanceReport
    {

      public Host( IPerformanceCounter<TReport> counter )
      {
        Counter = counter;
      }

      public IPerformanceCounter<TReport> Counter { get; }



      private object sync = new object();



      /// <summary>
      /// 向所有性能报告搜集器推送性能报告
      /// </summary>
      /// <returns>用于等待推送完成的 Task 对象</returns>
      public virtual async Task SendReport()
      {
        var report = await Counter.CreateReportAsync();
        await Task.WhenAll( registrations.Select( item => item.SendReportAsync( report ) ) );
      }

      /// <summary>
      /// 注册一个性能报告搜集器
      /// </summary>
      /// <param name="collector">性能报告搜集器</param>
      /// <returns>返回一个 IDisposable 对象，调用其 Dispose 方法来取消注册</returns>
      public IDisposable Register( IPerformanceReportCollector<TReport> collector )
      {
        lock ( sync )
        {
          var registration = new Registration( this, collector );
          registrations.Add( registration );
          return registration;
        }
      }


      /// <summary>
      /// 注册一个性能报告搜集器
      /// </summary>
      /// <param name="collector">性能报告搜集器</param>
      /// <returns>返回一个 IDisposable 对象，调用其 Dispose 方法来取消注册</returns>
      public IDisposable Register( params IPerformanceReportCollector<TReport>[] collectors )
      {
        lock ( sync )
        {
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


      private HashSet<Registration> registrations = new HashSet<Registration>();


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
        public Registration( Host<TReport> host, IPerformanceReportCollector<TReport> collector )
        {
          Host = host;
          Collector = collector;
        }

        public Host<TReport> Host { get; }

        public IPerformanceReportCollector<TReport> Collector { get; }


        public void Dispose()
        {
          Host.registrations.Remove( this );
        }

        public Task SendReportAsync( TReport report )
        {
          return Collector.SendReportAsync( report );
        }
      }
    }


  }
}
