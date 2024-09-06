using ACore.Server.Modules.SettingModule.Configuration;
using ACore.Server.Modules.SettingModule.CQRS;
using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Modules.SettingModule.Storage.Mongo;
using ACore.Server.Storages;
using ACore.Server.Modules.SettingModule.Storage.SQL.Memory;
using ACore.Server.Modules.SettingModule.Storage.SQL.PG;
using ACore.Server.Storages.Configuration.Options;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.SettingModule;

public static class SettingModuleServiceExtensions
{
  // public static void AddSettingServiceModule(this IServiceCollection services, Action<ACoreStorageOptions>? options = null)
  // {
  //   var testModuleConfiguration = new ACoreStorageOptions();
  //   options?.Invoke(testModuleConfiguration);
  //   AddSettingServiceModule(services, testModuleConfiguration);
  // }

  public static void AddSettingServiceModule(this IServiceCollection services, SettingModuleOptions options)
  {
    var myOptionsInstance = Options.Create(options);
    services.TryAddSingleton(myOptionsInstance);
    
    services.TryAddTransient(typeof(IPipelineBehavior<,>), typeof(SettingModulePipelineBehavior<,>));
    services.TryAddSingleton<IStorageResolver>(new DefaultStorageResolver());
    
    if (options.Storages.MongoDb != null)
    {
      services.AddDbContext<SettingModuleMongoStorageImpl>(opt => 
        opt.UseMongoDB(options.Storages.MongoDb.ReadWriteConnectionString, options.Storages.MongoDb.CollectionName));
    }

    if (options.Storages.PGDb != null)
    {
      services.AddDbContext<SettingModuleSqlPGStorageImpl>(opt =>
      {
        opt.UseNpgsql(options.Storages.PGDb.ReadWriteConnectionString);
        // opt.AddInterceptors(new SlowQueryDetectionHelper());
      });
    }

    if (options.Storages.UseMemoryStorage)
    {
      services.AddDbContext<SettingModuleSqlMemoryStorageImpl>(dbContextOptionsBuilder => dbContextOptionsBuilder.UseInMemoryDatabase(StorageConst.MemoryConnectionString + nameof(ISettingStorageModule) + Guid.NewGuid()));
    }
  }

  public static async Task UseSettingServiceModule(this IServiceProvider provider)
  {
    var opt = provider.GetService<IOptions<SettingModuleOptions>>()?.Value 
              ?? throw new ArgumentNullException("0","");
    
    var storageResolver = provider.GetService<IStorageResolver>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(IStorageResolver)}.");
    if (opt.Storages.MongoDb != null)
    {
      var mongoImpl = provider.GetService<SettingModuleMongoStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(SettingModuleMongoStorageImpl)}.");
      await storageResolver.ConfigureStorage<ISettingStorageModule>(new StorageImplementation(mongoImpl));
    }
    
    if (opt.Storages.PGDb != null)
    {
      var pgImpl = provider.GetService<SettingModuleSqlPGStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(SettingModuleSqlPGStorageImpl)}.");
      await storageResolver.ConfigureStorage<ISettingStorageModule>(new StorageImplementation(pgImpl));
    }

    if (opt.Storages.UseMemoryStorage)
    {
      var memoryImpl = provider.GetService<SettingModuleSqlMemoryStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(SettingModuleSqlMemoryStorageImpl)}.");
      await storageResolver.ConfigureStorage<ISettingStorageModule>(new StorageImplementation(memoryImpl));
    }
  }

  public static void AddSettingServiceModule(this IServiceCollection services, ACoreStorageOptions testOptions)
  {
    
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(SettingModulePipelineBehavior<,>));
    
    services.TryAddSingleton<IStorageResolver>(new DefaultStorageResolver());
    
    if (testOptions.MongoDb != null)
    {
      services.AddDbContext<SettingModuleMongoStorageImpl>(opt => opt.UseMongoDB(testOptions.MongoDb.ReadWriteConnectionString, testOptions.MongoDb.CollectionName));
    }

    if (testOptions.PGDb != null)
    {
      services.AddDbContext<SettingModuleSqlPGStorageImpl>(opt =>
      {
        opt.UseNpgsql(testOptions.PGDb.ReadWriteConnectionString);
        // opt.AddInterceptors(new SlowQueryDetectionHelper());
      });
    }

    if (testOptions.UseMemoryStorage)
    {
      services.AddDbContext<SettingModuleSqlMemoryStorageImpl>(dbContextOptionsBuilder => dbContextOptionsBuilder.UseInMemoryDatabase(StorageConst.MemoryConnectionString + nameof(ISettingStorageModule) + Guid.NewGuid()));
    }
  }

  public static async Task UseSettingServiceModule(this IServiceProvider provider,  ACoreStorageOptions opt)
  {
    var storageResolver = provider.GetService<IStorageResolver>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(IStorageResolver)}.");
    if (opt.MongoDb != null)
    {
      var mongoImpl = provider.GetService<SettingModuleMongoStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(SettingModuleMongoStorageImpl)}.");
      await storageResolver.ConfigureStorage<ISettingStorageModule>(new StorageImplementation(mongoImpl));
    }
    
    if (opt.PGDb != null)
    {
      var pgImpl = provider.GetService<SettingModuleSqlPGStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(SettingModuleSqlPGStorageImpl)}.");
      await storageResolver.ConfigureStorage<ISettingStorageModule>(new StorageImplementation(pgImpl));
    }

    if (opt.UseMemoryStorage)
    {
      var memoryImpl = provider.GetService<SettingModuleSqlMemoryStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(SettingModuleSqlMemoryStorageImpl)}.");
      await storageResolver.ConfigureStorage<ISettingStorageModule>(new StorageImplementation(memoryImpl));
    }
  }
}