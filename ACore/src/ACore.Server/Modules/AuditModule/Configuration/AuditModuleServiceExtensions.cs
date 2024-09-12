using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Modules.AuditModule.Storage.Mongo;
using ACore.Server.Modules.AuditModule.Storage.SQL.Memory;
using ACore.Server.Modules.AuditModule.Storage.SQL.PG;
using ACore.Server.Modules.SettingModule.Configuration;
using ACore.Server.Storages.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.AuditModule.Configuration;

internal static class AuditModuleServiceExtensions
{
  public static void AddAuditServiceModule(this IServiceCollection services, AuditServerModuleOptions options)
  {
    var myOptionsInstance = Options.Create(options);
    services.TryAddSingleton(myOptionsInstance);

    if (options.ManualConfiguration != null)
      services.TryAddSingleton(options.ManualConfiguration);

    if (options.Storages == null)
      throw new ArgumentException($"{nameof(options.Storages)} is null.");

    services.AddDbMongoStorage<AuditModuleMongoStorageImpl>(options.Storages);
    services.AddDbPGStorage<AuditPGEfStorageImpl>(options.Storages);
    services.AddDbMemoryStorage<AuditSqlMemoryStorageImpl>(options.Storages, nameof(IAuditStorageModule));
  }

  public static async Task UseAuditServiceModule(this IServiceProvider provider)
  {
    var opt = provider.GetService<IOptions<AuditServerModuleOptions>>()?.Value
              ?? throw new ArgumentException($"{nameof(SettingServerModuleOptions)} is not configured.");

    if (opt.Storages == null)
      throw new ArgumentException($"{nameof(opt.Storages)} is null.");

    await provider.ConfigureMongoStorage<IAuditStorageModule, AuditModuleMongoStorageImpl>(opt.Storages);
    await provider.ConfigurePGStorage<IAuditStorageModule, AuditPGEfStorageImpl>(opt.Storages);
    await provider.ConfigureMemoryStorage<IAuditStorageModule, AuditSqlMemoryStorageImpl>(opt.Storages);

    // var storageResolver = provider.GetService<IStorageResolver>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(IStorageResolver)}.");
    // if (storageOptions.Storages?.MongoDb != null)
    // {
    //   var mongoImpl = provider.GetService<AuditModuleMongoStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(AuditModuleMongoStorageImpl)}.");
    //   await storageResolver.ConfigureStorage<IAuditStorageModule>(new StorageImplementation(mongoImpl));
    // }
    //
    // if (storageOptions.Storages?.PGDb != null)
    // {
    //   var pgImpl = provider.GetService<AuditPGEfStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(AuditPGEfStorageImpl)}.");
    //   await storageResolver.ConfigureStorage<IAuditStorageModule>(new StorageImplementation(pgImpl));
    // }
    //
    // if (storageOptions.Storages is { UseMemoryStorage : true })
    // {
    //   var memoryImpl = provider.GetService<AuditSqlMemoryStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(AuditSqlMemoryStorageImpl)}.");
    //   await storageResolver.ConfigureStorage<IAuditStorageModule>(new StorageImplementation(memoryImpl));
    // }
  }
}