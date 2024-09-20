using System.Globalization;
using System.Reflection;
using ACore.Base.Cache;
using ACore.Extensions;
using ACore.Tests.Base.Models;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.InMemory;

namespace ACore.Tests.Base;

public abstract class TestsBase
{
  private ServiceCollection _services = new();
  protected IConfigurationRoot Configuration { get; set; } = null!;
  protected IMediator Mediator = null!;
  protected TestData TestData { get; private set; } = null!;

  protected ILogger<TestsBase> Log { get; set; } = null!;
  protected InMemorySink LogInMemorySink { get; set; } = new();

  protected string RootDir { get; set; } = string.Empty;

  protected async Task RunTestAsync(MemberInfo? method, Func<Task> testCode)
  {
    if (method == null)
      throw new ArgumentException($"Method name is null {nameof(RunTestAsync)}");

    await RunTestAsync(new TestData(method), testCode);
  }

  protected Task RunTestAsync<T>(MemberInfo method, T param, Func<T, Task> testCode)
    => RunTestAsync(new TestData(method), param, testCode);

  // ReSharper disable once MemberCanBePrivate.Global
  protected async Task RunTestAsync<T>(TestData testData, T param, Func<T, Task> testCode)
  {
    Thread.CurrentThread.CurrentCulture = new CultureInfo(1033);
    Thread.CurrentThread.CurrentUICulture = new CultureInfo(1033);

    TestData = testData;
    await SettingStartTestAsync(_services);

    if (CheckTest() == false)
      return;

    Log.LogInformation("Start test Test {TestName}", TestData.TestName);

    try
    {
      await testCode(param);
    }
    catch (Exception ex)
    {
      Log.LogError("RunTestAsync-{Ex}", ex.MessageRecursive());
      throw;
    }
    finally
    {
      await FinishedTestAsync();
    }

    Log.LogInformation("End test Test {TestName}", TestData.TestName);
  }

  protected async Task RunTestAsync(TestData testData, Func<Task> testCode)
  {
    await RunTestAsync(testData, 0, async (_) => { await testCode(); });
  }

  protected void RegisterAutofacContainer(ServiceCollection services, ContainerBuilder containerBuilder)
  {
    SetContainer(containerBuilder);
    RegisterServices(services);

    var configuration = MediatRConfigurationBuilder
      .Create(typeof(TestsBase).Assembly, typeof(CacheKey).Assembly)
      .WithAllOpenGenericHandlerTypesRegistered()
      .Build();
    containerBuilder.RegisterMediatR(configuration);
  }

  protected virtual void SetContainer(ContainerBuilder containerBuilder)
  {
    
  }

  protected virtual void RegisterServices(ServiceCollection services)
  {
    RootDir = JsonSettingPath();

    var builder = new ConfigurationBuilder()
      .SetBasePath(RootDir)
      .AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: true)
      .AddJsonFile("appsettings.Test.Debug.json", optional: true, reloadOnChange: true)
      .AddEnvironmentVariables();
    Configuration = builder.Build();
    services.AddSingleton<IConfiguration>(Configuration);
    

    var logDir = Path.Combine(RootDir, "Logs");
    var serilog = new LoggerConfiguration()
      .MinimumLevel.Verbose()
      .WriteTo.File(Path.Combine(logDir, TestData.TestName) + ".txt", retainedFileTimeLimit: TimeSpan.FromDays(1), retainedFileCountLimit: 1)
      .WriteTo.Sink(LogInMemorySink)
      .WriteTo.InMemory(restrictedToMinimumLevel: LogEventLevel.Debug)
      .CreateLogger();

    services.AddLogging(logBuilder => { logBuilder.AddSerilog(logger: serilog, dispose: true); });
  }

  protected virtual async Task GetServices(IServiceProvider sp)
  {
    Log = sp.GetService<ILogger<TestsBase>>() ?? throw new ArgumentException($"{nameof(ILogger<TestsBase>)} is null.");
    Mediator = sp.GetService<IMediator>() ?? throw new ArgumentException($"{nameof(IMediator)} is null.");
    await Task.CompletedTask;
  }

  protected virtual async Task FinishedTestAsync()
  {
    await Task.CompletedTask;
  }

  protected virtual string JsonSettingPath()
  {
    var dic = Directory.GetCurrentDirectory();
    dic = dic.Replace("\\bin\\Debug\\net6.0", string.Empty);
    dic = dic.Replace("\\bin\\Release\\net6.0", string.Empty);
    dic = dic.Replace("\\bin\\Debug\\net7.0", string.Empty);
    dic = dic.Replace("\\bin\\Release\\net7.0", string.Empty);
    dic = dic.Replace("\\bin\\Debug\\net8.0", string.Empty);
    dic = dic.Replace("\\bin\\Release\\net8.0", string.Empty);
    return dic;
  }

  private bool CheckTest()
  {
    if (string.IsNullOrEmpty(TestData.TestId))
      throw new Exception("Test does not have id.");

    // if ((TestData.TestEnvironmentType & TestConfig.TestType) == 0)
    //     return false;

    return true;
  }

  private async Task SettingStartTestAsync(ServiceCollection services)
  {
    _services = [];
    // RegisterServices(services);
    //
    // var serviceProvider = services.BuildServiceProvider();
    //
    // await GetServicesAsync(serviceProvider);
    //
    //------------------


    var serviceCollection = services;

    // The Microsoft.Extensions.Logging package provides this one-liner
    // to add logging services.
    // serviceCollection.AddLogging();

    var containerBuilder = new ContainerBuilder();

    RegisterAutofacContainer(services, containerBuilder);


    // Once you've registered everything in the ServiceCollection, call
    // Populate to bring those registrations into Autofac. This is
    // just like a foreach over the list of things in the collection
    // to add them to Autofac.
    containerBuilder.Populate(serviceCollection);

    // Creating a new AutofacServiceProvider makes the container
    // available to your app using the Microsoft IServiceProvider
    // interface so you can use those abstractions rather than
    // binding directly to Autofac.
    var container = containerBuilder.Build();


    var serviceProvider = new AutofacServiceProvider(container);
    await GetServices(serviceProvider);
  }
}