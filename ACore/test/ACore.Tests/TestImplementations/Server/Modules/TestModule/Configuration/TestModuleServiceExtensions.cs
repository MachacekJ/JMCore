using ACore.Server.Modules.AuditModule.CQRS.AuditGet;
using ACore.Server.Storages.Configuration;
using ACore.Tests.TestImplementations.Server.Configuration;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Delete;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Get;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Save;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.Mongo;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Memory;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.PG;
using Autofac;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.Configuration;

public static class TestModuleServiceExtensions
{
  public static void AddTestModule(this IServiceCollection services, TestModuleOptions options)
  {
   // services.AddMediatR(c => { c.RegisterServicesFromAssemblyContaining(typeof(ITestStorageModule)); });
    services.TryAddTransient(typeof(IPipelineBehavior<,>), typeof(TestModulePipelineBehavior<,>));
    
    if (options.Storages == null)
      throw new ArgumentException($"{nameof(options.Storages)} is null.");
    
    services.AddDbMongoStorage<TestModuleMongoStorageImpl>(options.Storages);
    services.AddDbPGStorage<TestModulePGStorageImpl>(options.Storages);
    services.AddDbMemoryStorage<TestModuleMemoryStorageImpl>(options.Storages, nameof(ITestStorageModule));
    
    TestNoAuditData.MapConfig();
  }


  public static void ConfigureAutofacTestModule(this ContainerBuilder containerBuilder)
  {
    containerBuilder.RegisterGeneric(typeof(TestAuditGetHandler<>)).AsImplementedInterfaces();
    containerBuilder.RegisterGeneric(typeof(TestAuditSaveHandler<>)).AsImplementedInterfaces();
    containerBuilder.RegisterGeneric(typeof(TestAuditDeleteHandler<>)).AsImplementedInterfaces();
  }

  public static async Task UseTestModule(this IServiceProvider provider)
  {
    var opt = provider.GetService<IOptions<ACoreTestOptions>>()?.Value
              ?? throw new ArgumentException($"{nameof(TestModuleOptions)} is not configured.");

    StorageOptions? storageOptions = null;
    if (opt.ACoreServerOptions.DefaultStorages != null)
      storageOptions = opt.ACoreServerOptions.DefaultStorages;
    if (opt.TestModuleOptions.Storages != null)
      storageOptions = opt.TestModuleOptions.Storages;
    if (storageOptions == null)
      throw new ArgumentException($"{nameof(opt.TestModuleOptions)} is null. You can also use {nameof(opt.ACoreServerOptions.DefaultStorages)}.");


    await provider.ConfigureMongoStorage<ITestStorageModule, TestModuleMongoStorageImpl>(storageOptions);
    await provider.ConfigurePGStorage<ITestStorageModule, TestModulePGStorageImpl>(storageOptions);
    await provider.ConfigureMemoryStorage<ITestStorageModule, TestModuleMemoryStorageImpl>(storageOptions);
  }
}