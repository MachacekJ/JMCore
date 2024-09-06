using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Modules.AuditModule.Storage.Mongo;
using ACore.Server.Modules.AuditModule.Storage.SQL.Memory;
using ACore.Server.Modules.AuditModule.Storage.SQL.PG;
using ACore.Server.Modules.SettingModule;
using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Storages;
using ACore.Server.Storages.Configuration.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Server.Modules.AuditModule;

public static class AuditModuleServiceExtensions
{
  public static void AddAuditServiceModule(this IServiceCollection services, Action<ACoreStorageOptions>? options = null)
  {
    var storageOptions = new ACoreStorageOptions();
    options?.Invoke(storageOptions);
    AddAuditServiceModule(services, storageOptions);
  }

  public static void AddAuditServiceModule(this IServiceCollection services, ACoreStorageOptions storageOptions)
  {
    services.AddSettingServiceModule(storageOptions);

    if (storageOptions.MongoDb != null)
    {
      services.AddDbContext<AuditModuleMongoStorageImpl>(opt => opt.UseMongoDB(storageOptions.MongoDb.ReadWriteConnectionString, storageOptions.MongoDb.CollectionName));
    }

    if (storageOptions.PGDb != null)
    {
      services.AddDbContext<AuditPGEfStorageImpl>(opt =>
      {
        opt.UseNpgsql(storageOptions.PGDb.ReadWriteConnectionString);
        // opt.AddInterceptors(new SlowQueryDetectionHelper());
      });
    }

    if (storageOptions.UseMemoryStorage)
    {
      services.AddDbContext<AuditSqlMemoryStorageImpl>(dbContextOptionsBuilder => dbContextOptionsBuilder.UseInMemoryDatabase(StorageConst.MemoryConnectionString + nameof(ISettingStorageModule) + Guid.NewGuid()));
    }
  }

  public static async Task UseAuditServiceModule(this IServiceProvider provider, ACoreStorageOptions storageOptions)
  {
    await provider.UseSettingServiceModule(storageOptions);
    var storageResolver = provider.GetService<IStorageResolver>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(IStorageResolver)}.");
    if (storageOptions.MongoDb != null)
    {
      var mongoImpl = provider.GetService<AuditModuleMongoStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(AuditModuleMongoStorageImpl)}.");
      await storageResolver.ConfigureStorage<IAuditStorageModule>(new StorageImplementation(mongoImpl));
    }

    if (storageOptions.PGDb != null)
    {
      var pgImpl = provider.GetService<AuditPGEfStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(AuditPGEfStorageImpl)}.");
      await storageResolver.ConfigureStorage<IAuditStorageModule>(new StorageImplementation(pgImpl));
    }

    if (storageOptions.UseMemoryStorage)
    {
      var memoryImpl = provider.GetService<AuditSqlMemoryStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(AuditSqlMemoryStorageImpl)}.");
      await storageResolver.ConfigureStorage<IAuditStorageModule>(new StorageImplementation(memoryImpl));
    }
  }
}