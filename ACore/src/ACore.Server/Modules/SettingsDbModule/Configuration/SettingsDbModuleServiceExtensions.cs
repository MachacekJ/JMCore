using ACore.Server.Configuration;
using ACore.Server.Modules.SettingsDbModule.CQRS;
using ACore.Server.Modules.SettingsDbModule.Storage;
using ACore.Server.Modules.SettingsDbModule.Storage.Mongo;
using ACore.Server.Modules.SettingsDbModule.Storage.SQL.Memory;
using ACore.Server.Modules.SettingsDbModule.Storage.SQL.PG;
using ACore.Server.Storages.Configuration;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.SettingsDbModule.Configuration;

internal static class SettingsDbModuleServiceExtensions
{
  public static void AddSettingsDbModule(this IServiceCollection services, SettingsDbModuleOptions options)
  {
    services.AddMediatR(c => { c.RegisterServicesFromAssemblyContaining(typeof(ISettingsDbModuleStorage)); });
    services.TryAddTransient(typeof(IPipelineBehavior<,>), typeof(SettingsDbModulePipelineBehavior<,>));

    if (options.Storages == null)
      throw new ArgumentException($"{nameof(options.Storages)} is null.");
    
    services.AddDbMongoStorage<SettingsDbModuleMongoStorageImpl>(options.Storages);
    services.AddDbPGStorage<SettingsDbModuleSqlPGStorageImpl>(options.Storages);
    services.AddDbMemoryStorage<SettingsDbModuleSqlMemoryStorageImpl>(options.Storages, nameof(ISettingsDbModuleStorage));
  }

  public static async Task UseSettingServiceModule(this IServiceProvider provider)
  {
    var opt = provider.GetService<IOptions<ACoreServerOptions>>()?.Value
              ?? throw new ArgumentException($"{nameof(SettingsDbModuleOptions)} is not configured.");

    StorageOptions? storageOptions = null;
    if (opt.DefaultStorages != null)
      storageOptions = opt.DefaultStorages;
    if (opt.SettingsDbModuleOptions.Storages != null)
      storageOptions = opt.SettingsDbModuleOptions.Storages;
    if (storageOptions == null)
      throw new ArgumentException($"{nameof(opt.SettingsDbModuleOptions)} is null. You can also use {nameof(opt.DefaultStorages)}.");

    await provider.ConfigureMongoStorage<ISettingsDbModuleStorage, SettingsDbModuleMongoStorageImpl>(storageOptions);
    await provider.ConfigurePGStorage<ISettingsDbModuleStorage, SettingsDbModuleSqlPGStorageImpl>(storageOptions);
    await provider.ConfigureMemoryStorage<ISettingsDbModuleStorage, SettingsDbModuleSqlMemoryStorageImpl>(storageOptions);
  }
}