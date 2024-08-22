using ACore.AppTest.Modules.TestModule.CQRS;
using ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit;
using ACore.AppTest.Modules.TestModule.Storages.EF;
using ACore.AppTest.Modules.TestModule.Storages.Memory;
using ACore.AppTest.Modules.TestModule.Storages.Mongo;
using ACore.AppTest.Modules.TestModule.Storages.PG;
using ACore.Server.Storages;
using Autofac;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ACore.Server.Modules.AuditModule;
using ACore.Server.Modules.AuditModule.CQRS.Audit.AuditGet;
using ACore.Server.Modules.AuditModule.Extensions;

namespace ACore.AppTest.Modules.TestModule;

public static class TestModuleServiceExtensions
{
  private const string _memName = "memory";

  public static void AddTestServiceModule(this IServiceCollection services, Configuration.TestModuleConfiguration options)
  {
    var testModuleConfiguration = options;
    services.TryAddSingleton<IStorageResolver>(new DefaultStorageResolver());
    services.AddAuditServiceModule(testModuleConfiguration);

    if (testModuleConfiguration.MongoDb != null)
    {
      services.AddDbContext<EfTestMongoStorageImpl>(dbContextOptionsBuilder => dbContextOptionsBuilder.UseMongoDB(testModuleConfiguration.MongoDb.ReadWrite.ConnectionString, testModuleConfiguration.MongoDb.DbName));
    }

    if (testModuleConfiguration.PGDb != null)
    {
      services.AddDbContext<PGEFTestStorageImpl>(dbContextOptionsBuilder => dbContextOptionsBuilder.UseNpgsql(testModuleConfiguration.PGDb.ReadWriteConnectionString));
    }

    if (testModuleConfiguration.UseMemoryStorage)
    {
      services.AddDbContext<MemoryTestStorageImpl>(dbContextOptionsBuilder => dbContextOptionsBuilder.UseInMemoryDatabase(_memName + nameof(IEFTestStorageModule) + Guid.NewGuid()));
    }

    AddTestServiceModuleOld(services);
  }

  public static void AddTestServiceModuleOld(this IServiceCollection services)
  {
    services.AddMediatR(c => { c.RegisterServicesFromAssemblyContaining(typeof(IEFTestStorageModule)); });
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TestModuleBehavior<,>));
  }

  public static void RegisterAutofacTestService(this ContainerBuilder containerBuilder)
  {
    containerBuilder.RegisterGeneric(typeof(TestAttributeAuditGetHandler<>)).AsImplementedInterfaces();
    containerBuilder.RegisterGeneric(typeof(TestAttributeAuditSaveHandler<>)).AsImplementedInterfaces();
    containerBuilder.RegisterGeneric(typeof(TestAttributeAuditDeleteHandler<>)).AsImplementedInterfaces();
    containerBuilder.RegisterGeneric(typeof(AuditGetHandler<>)).AsImplementedInterfaces();
  }

  public static async Task UseTestServiceModule(this IServiceProvider provider, Configuration.TestModuleConfiguration opt)
  {
    await provider.UseAuditServiceModule(opt);
    var storageResolver = provider.GetService<IStorageResolver>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(IStorageResolver)}.");

    if (opt.MongoDb != null)
    {
      var mongoImpl = provider.GetService<EfTestMongoStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(EfTestMongoStorageImpl)}.");
      await storageResolver.ConfigureStorage<IEFTestStorageModule>(new StorageImplementation(mongoImpl));
    }

    if (opt.PGDb != null)
    {
      var pgImpl = provider.GetService<PGEFTestStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(PGEFTestStorageImpl)}.");
      await storageResolver.ConfigureStorage<IEFTestStorageModule>(new StorageImplementation(pgImpl));
    }

    if (opt.UseMemoryStorage)
    {
      var pgImpl = provider.GetService<MemoryTestStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(MemoryTestStorageImpl)}.");
      await storageResolver.ConfigureStorage<IEFTestStorageModule>(new StorageImplementation(pgImpl));
    }
  }
}