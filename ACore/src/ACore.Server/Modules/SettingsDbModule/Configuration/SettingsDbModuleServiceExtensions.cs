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
  public static void AddSettingServiceModule(this IServiceCollection services, SettingsDbModuleOptions options)
  {
    var myOptionsInstance = Options.Create(options);
    services.TryAddSingleton(myOptionsInstance);

    services.TryAddTransient(typeof(IPipelineBehavior<,>), typeof(SettingsDbModulePipelineBehavior<,>));


    if (options.Storages == null)
      throw new ArgumentException($"{nameof(options.Storages)} is null.");
    services.AddDbMongoStorage<SettingsDbModuleMongoStorageImpl>(options.Storages);
    services.AddDbPGStorage<SettingsDbModuleSqlPGStorageImpl>(options.Storages);
    services.AddDbMemoryStorage<SettingsDbModuleSqlMemoryStorageImpl>(options.Storages, nameof(ISettingsDbStorageModule));
  }

  public static async Task UseSettingServiceModule(this IServiceProvider provider)
  {
    var opt = provider.GetService<IOptions<SettingsDbModuleOptions>>()?.Value
              ?? throw new ArgumentException($"{nameof(SettingsDbModuleOptions)} is not configured.");

    if (opt.Storages == null)
      throw new ArgumentException($"{nameof(opt.Storages)} is null.");

    await provider.ConfigureMongoStorage<ISettingsDbStorageModule, SettingsDbModuleMongoStorageImpl>(opt.Storages);
    await provider.ConfigurePGStorage<ISettingsDbStorageModule, SettingsDbModuleSqlPGStorageImpl>(opt.Storages);
    await provider.ConfigureMemoryStorage<ISettingsDbStorageModule, SettingsDbModuleSqlMemoryStorageImpl>(opt.Storages);
  }
}