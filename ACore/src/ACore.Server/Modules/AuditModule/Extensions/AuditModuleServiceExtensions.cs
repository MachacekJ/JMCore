using ACore.Server.Configuration;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Modules.AuditModule.Storage.Mongo;
using ACore.Server.Modules.AuditModule.Storage.SQL.Memory;
using ACore.Server.Modules.AuditModule.Storage.SQL.PG;
using ACore.Server.Modules.SettingModule;
using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Storages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Server.Modules.AuditModule.Extensions;

public static class AuditModuleServiceExtensions
{
  public static void AddAuditServiceModule(this IServiceCollection services, Action<StorageModuleConfiguration>? options = null)
  {
    var testModuleConfiguration = new StorageModuleConfiguration();
    options?.Invoke(testModuleConfiguration);
    AddAuditServiceModule(services, testModuleConfiguration);
  }

  public static void AddAuditServiceModule(this IServiceCollection services, StorageModuleConfiguration testModuleConfiguration)
  {
    services.AddSettingServiceModule(testModuleConfiguration);

    if (testModuleConfiguration.MongoDb != null)
    {
      services.AddDbContext<AuditMongoStorageImpl>(opt => opt.UseMongoDB(testModuleConfiguration.MongoDb.ReadWrite.ConnectionString, testModuleConfiguration.MongoDb.DbName));
    }

    if (testModuleConfiguration.PGDb != null)
    {
      services.AddDbContext<AuditPGEfStorageImpl>(opt =>
      {
        opt.UseNpgsql(testModuleConfiguration.PGDb.ReadWriteConnectionString);
        // opt.AddInterceptors(new SlowQueryDetectionHelper());
      });
    }

    if (testModuleConfiguration.UseMemoryStorage)
    {
      services.AddDbContext<AuditSqlMemoryStorageImpl>(dbContextOptionsBuilder => dbContextOptionsBuilder.UseInMemoryDatabase(StorageConst.MemoryConnectionString + nameof(IBasicStorageModule) + Guid.NewGuid()));
    }
  }

  public static async Task UseAuditServiceModule(this IServiceProvider provider, StorageModuleConfiguration opt)
  {
    await provider.UseSettingServiceModule(opt);
    var storageResolver = provider.GetService<IStorageResolver>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(IStorageResolver)}.");
    if (opt.MongoDb != null)
    {
      var mongoImpl = provider.GetService<AuditMongoStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(AuditMongoStorageImpl)}.");
      await storageResolver.ConfigureStorage<IAuditStorageModule>(new StorageImplementation(mongoImpl));
    }

    if (opt.PGDb != null)
    {
      var pgImpl = provider.GetService<AuditPGEfStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(AuditPGEfStorageImpl)}.");
      await storageResolver.ConfigureStorage<IAuditStorageModule>(new StorageImplementation(pgImpl));
    }

    if (opt.UseMemoryStorage)
    {
      var memoryImpl = provider.GetService<AuditSqlMemoryStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(AuditSqlMemoryStorageImpl)}.");
      await storageResolver.ConfigureStorage<IAuditStorageModule>(new StorageImplementation(memoryImpl));
    }
  }
}