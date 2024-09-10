using ACore.Server.Modules.LocalizationModule.Storage;
using ACore.Server.Modules.LocalizationModule.Storage.Memory;
using ACore.Server.Modules.SettingModule;
using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Storages;
using ACore.Server.Storages.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Server.Modules.LocalizationModule;

public static class LocalizationModuleServiceExtensions
{
  // public static void AddLocalizationServiceModule(this IServiceCollection services, Action<StorageOptions>? options = null)
  // {
  //   var testModuleConfiguration = new StorageOptions();
  //   options?.Invoke(testModuleConfiguration);
  //   AddLocalizationServiceModule(services, testModuleConfiguration);
  // }
  //
  // public static void AddLocalizationServiceModule(this IServiceCollection services, StorageOptions testOptions)
  // {
  //   services.AddSettingServiceModule(testOptions);
  //
  //   if (testOptions.MongoDb != null)
  //   {
  //     throw new NotImplementedException();
  //     // services.AddDbContext<AuditMongoStorageImpl>(opt => opt.UseMongoDB(testModuleConfiguration.MongoDb.ReadWrite.ConnectionString, testModuleConfiguration.MongoDb.DbName));
  //   }
  //
  //   if (testOptions.PGDb != null)
  //   {
  //     throw new NotImplementedException();
  //     // services.AddDbContext<AuditPGEfStorageImpl>(opt =>
  //     // {
  //     //   opt.UseNpgsql(testModuleConfiguration.PGDb.ReadWriteConnectionString);
  //     //   // opt.AddInterceptors(new SlowQueryDetectionHelper());
  //     // });
  //   }
  //
  //   if (testOptions.UseMemoryStorage)
  //   {
  //     services.AddDbContext<LocalizationMemoryEfStorageImpl>(dbContextOptionsBuilder => dbContextOptionsBuilder.UseInMemoryDatabase(StorageConst.MemoryConnectionString + nameof(ISettingStorageModule) + Guid.NewGuid()));
  //   }
  // }
  //
  // public static async Task UseAuditServiceModule(this IServiceProvider provider, StorageOptions opt)
  // {
  //   await provider.UseSettingServiceModule(opt);
  //   var storageResolver = provider.GetService<IStorageResolver>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(IStorageResolver)}.");
  //   // if (opt.MongoDb != null)
  //   // {
  //   //   var mongoImpl = provider.GetService<AuditMongoStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(AuditMongoStorageImpl)}.");
  //   //   await storageResolver.ConfigureStorage<IAuditStorageModule>(new StorageImplementation(mongoImpl));
  //   // }
  //   //
  //   // if (opt.PGDb != null)
  //   // {
  //   //   var pgImpl = provider.GetService<AuditPGEfStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(AuditPGEfStorageImpl)}.");
  //   //   await storageResolver.ConfigureStorage<IAuditStorageModule>(new StorageImplementation(pgImpl));
  //   // }
  //
  //   if (opt.UseMemoryStorage)
  //   {
  //     var memoryImpl = provider.GetService<LocalizationMemoryEfStorageImpl>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(LocalizationMemoryEfStorageImpl)}.");
  //     await storageResolver.ConfigureStorage<ILocalizationStorageModule>(new StorageImplementation(memoryImpl));
  //   }
  // }
}