using ACore.Server.Configuration;
using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Modules.SettingModule.Storage.Memory;
using ACore.Server.Modules.SettingModule.Storage.Mongo;
using ACore.Server.Modules.SettingModule.Storage.PG;
using ACore.Server.Storages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ACore.Server.Modules.SettingModule;

public static class BasicModuleServiceExtensions
{
  public static void AddSettingServiceModule(this IServiceCollection services, Action<StorageModuleConfiguration>? options = null)
  {
    var testModuleConfiguration = new StorageModuleConfiguration();
    options?.Invoke(testModuleConfiguration);
    AddSettingServiceModule(services, testModuleConfiguration);
  }
  
  public static void AddSettingServiceModule(this IServiceCollection services, StorageModuleConfiguration testModuleConfiguration)
  {
    services.AddMediatR((c) => { c.RegisterServicesFromAssemblyContaining(typeof(IBasicStorageModule)); });
    services.TryAddSingleton<IStorageResolver>(new DefaultStorageResolver());
    
    if (testModuleConfiguration.MongoDb != null)
    {
      services.AddDbContext<BasicSqlMongoEfStorageImpl>(opt => opt.UseMongoDB(testModuleConfiguration.MongoDb.ReadWrite.ConnectionString, testModuleConfiguration.MongoDb.DbName));
    }

    if (testModuleConfiguration.PGDb != null)
    {
      services.AddDbContext<BasicSqlPGEfStorageImpl>(opt =>
      {
        opt.UseNpgsql(testModuleConfiguration.PGDb.ReadWriteConnectionString);
        // opt.AddInterceptors(new SlowQueryDetectionHelper());
      });
    }

    if (testModuleConfiguration.UseMemoryStorage)
    {
      services.AddDbContext<BasicSqlMemoryEfStorageImpl>(dbContextOptionsBuilder => dbContextOptionsBuilder.UseInMemoryDatabase(StorageConst.MemoryConnectionString + nameof(IBasicStorageModule) + Guid.NewGuid()));
    }
  }

  public static async Task UseSettingServiceModule(this IServiceProvider provider,  StorageModuleConfiguration opt)
  {
    var storageResolver = provider.GetService<IStorageResolver>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(IStorageResolver)}.");
    if (opt.MongoDb != null)
    {
      var mongoImpl = provider.GetService<BasicSqlMongoEfStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(BasicSqlMongoEfStorageImpl)}.");
      await storageResolver.ConfigureStorage<IBasicStorageModule>(new StorageImplementation(mongoImpl));
    }
    
    if (opt.PGDb != null)
    {
      var pgImpl = provider.GetService<BasicSqlPGEfStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(BasicSqlPGEfStorageImpl)}.");
      await storageResolver.ConfigureStorage<IBasicStorageModule>(new StorageImplementation(pgImpl));
    }

    if (opt.UseMemoryStorage)
    {
      var memoryImpl = provider.GetService<BasicSqlMemoryEfStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(BasicSqlMemoryEfStorageImpl)}.");
      await storageResolver.ConfigureStorage<IBasicStorageModule>(new StorageImplementation(memoryImpl));
    }
  }
}