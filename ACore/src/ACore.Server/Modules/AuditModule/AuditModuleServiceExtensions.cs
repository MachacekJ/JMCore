
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Modules.AuditModule.Storage.Mongo;
using ACore.Server.Modules.AuditModule.Storage.SQL.Memory;
using ACore.Server.Modules.AuditModule.Storage.SQL.PG;
using ACore.Server.Storages;
using ACore.Server.Storages.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Server.Modules.AuditModule;

public static class AuditModuleServiceExtensions
{
  internal static void AddAuditServiceModule(this IServiceCollection services, AuditModuleOptions storageOptions)
  {
    if (storageOptions.Storages == null)
      throw new ArgumentException($"{nameof(storageOptions.Storages)} is null.");

    services.AddDbMongoStorage<AuditModuleMongoStorageImpl>(storageOptions.Storages);
    services.AddDbPGStorage<AuditPGEfStorageImpl>(storageOptions.Storages);
    services.AddDbMemoryStorage<AuditSqlMemoryStorageImpl>(storageOptions.Storages, nameof(IAuditStorageModule));
  }

  internal static async Task UseAuditServiceModule(this IServiceProvider provider, AuditModuleOptions storageOptions)
  {
    var storageResolver = provider.GetService<IStorageResolver>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(IStorageResolver)}.");
    if (storageOptions.Storages?.MongoDb != null)
    {
      var mongoImpl = provider.GetService<AuditModuleMongoStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(AuditModuleMongoStorageImpl)}.");
      await storageResolver.ConfigureStorage<IAuditStorageModule>(new StorageImplementation(mongoImpl));
    }

    if (storageOptions.Storages?.PGDb != null)
    {
      var pgImpl = provider.GetService<AuditPGEfStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(AuditPGEfStorageImpl)}.");
      await storageResolver.ConfigureStorage<IAuditStorageModule>(new StorageImplementation(pgImpl));
    }

    if (storageOptions.Storages is { UseMemoryStorage : true })
    {
      var memoryImpl = provider.GetService<AuditSqlMemoryStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(AuditSqlMemoryStorageImpl)}.");
      await storageResolver.ConfigureStorage<IAuditStorageModule>(new StorageImplementation(memoryImpl));
    }
  }
}