using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Modules.SettingModule.Storage.Mongo;
using ACore.Server.Storages;
using ACore.Server.Modules.SettingModule.Storage.SQL.Memory;
using ACore.Server.Modules.SettingModule.Storage.SQL.PG;
using ACore.Server.Storages.Configuration.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ACore.Server.Modules.SettingModule;

public static class SettingModuleServiceExtensions
{
  public static void AddSettingServiceModule(this IServiceCollection services, Action<ACoreStorageOptions>? options = null)
  {
    var testModuleConfiguration = new ACoreStorageOptions();
    options?.Invoke(testModuleConfiguration);
    AddSettingServiceModule(services, testModuleConfiguration);
  }
  
  public static void AddSettingServiceModule(this IServiceCollection services, ACoreStorageOptions testOptions)
  {
    services.AddMediatR((c) => { c.RegisterServicesFromAssemblyContaining(typeof(ISettingStorageModule)); });
    services.TryAddSingleton<IStorageResolver>(new DefaultStorageResolver());
    
    if (testOptions.MongoDb != null)
    {
      services.AddDbContext<SettingModuleMongoStorageImpl>(opt => opt.UseMongoDB(testOptions.MongoDb.ReadWrite, testOptions.MongoDb.DbName));
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