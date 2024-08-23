using ACore.AppTest.Modules.TestModule.Configuration.Options;
using ACore.AppTest.Modules.TestModule.CQRS;
using ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit;
using ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit.Delete;
using ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit.Get;
using ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit.Save;
using ACore.AppTest.Modules.TestModule.Storages;
using ACore.AppTest.Modules.TestModule.Storages.Mongo;
using ACore.AppTest.Modules.TestModule.Storages.SQL.Memory;
using ACore.AppTest.Modules.TestModule.Storages.SQL.PG;
using ACore.Server.Modules.AuditModule;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.AuditModule.CQRS.Audit.AuditGet;
using ACore.Server.Modules.AuditModule.Extensions;
using ACore.Server.Storages;
using Autofac;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ACore.AppTest.Modules.TestModule.Configuration;

public static class TestModuleServiceExtensions
{
  private const string MemoryConnectionStringPrefix = "memory";

  public static void AddTestServiceModule(this IServiceCollection services, TestModuleOptions moduleOptions)
  {
    var testModuleConfiguration = moduleOptions;
    services.TryAddSingleton<IStorageResolver>(new DefaultStorageResolver());
    services.AddAuditServiceModule(testModuleConfiguration);

    if (testModuleConfiguration.MongoDb != null)
    {
      services.AddDbContext<TestModuleMongoStorageImpl>(dbContextOptionsBuilder => dbContextOptionsBuilder.UseMongoDB(testModuleConfiguration.MongoDb.ReadWrite, testModuleConfiguration.MongoDb.DbName));
    }

    if (testModuleConfiguration.PGDb != null)
    {
      services.AddDbContext<TestModulePGStorageImpl>(dbContextOptionsBuilder => dbContextOptionsBuilder.UseNpgsql(testModuleConfiguration.PGDb.ReadWriteConnectionString));
    }

    if (testModuleConfiguration.UseMemoryStorage)
    {
      services.AddDbContext<TestModuleMemoryStorageImpl>(dbContextOptionsBuilder => dbContextOptionsBuilder.UseInMemoryDatabase(MemoryConnectionStringPrefix + nameof(ITestStorageModule) + Guid.NewGuid()));
    }

    services.AddMediatR(c => { c.RegisterServicesFromAssemblyContaining(typeof(ITestStorageModule)); });
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TestModuleBehavior<,>));
  }


  public static void RegisterAutofacTestService(this ContainerBuilder containerBuilder)
  {
    containerBuilder.RegisterGeneric(typeof(TestAttributeAuditGetHandler<>)).AsImplementedInterfaces();
    containerBuilder.RegisterGeneric(typeof(TestAttributeAuditSaveHandler<>)).AsImplementedInterfaces();
    containerBuilder.RegisterGeneric(typeof(TestAttributeAuditDeleteHandler<>)).AsImplementedInterfaces();
    containerBuilder.RegisterGeneric(typeof(AuditGetHandler<>)).AsImplementedInterfaces();
  }

  public static async Task UseTestServiceModule(this IServiceProvider provider, TestModuleOptions opt)
  {
    await provider.UseAuditServiceModule(opt);
    var storageResolver = provider.GetService<IStorageResolver>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(IStorageResolver)}.");

    if (opt.MongoDb != null)
    {
      var mongoImpl = provider.GetService<TestModuleMongoStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(TestModuleMongoStorageImpl)}.");
      await storageResolver.ConfigureStorage<ITestStorageModule>(new StorageImplementation(mongoImpl));
    }

    if (opt.PGDb != null)
    {
      var pgImpl = provider.GetService<TestModulePGStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(TestModulePGStorageImpl)}.");
      await storageResolver.ConfigureStorage<ITestStorageModule>(new StorageImplementation(pgImpl));
    }

    if (opt.UseMemoryStorage)
    {
      var memoryImpl = provider.GetService<TestModuleMemoryStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(TestModuleMemoryStorageImpl)}.");
      await storageResolver.ConfigureStorage<ITestStorageModule>(new StorageImplementation(memoryImpl));
    }
  }
}