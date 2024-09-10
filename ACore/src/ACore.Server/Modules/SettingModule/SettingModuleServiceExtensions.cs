using ACore.Server.Modules.SettingModule.Configuration;
using ACore.Server.Modules.SettingModule.CQRS;
using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Modules.SettingModule.Storage.Mongo;
using ACore.Server.Storages;
using ACore.Server.Modules.SettingModule.Storage.SQL.Memory;
using ACore.Server.Modules.SettingModule.Storage.SQL.PG;
using ACore.Server.Storages.Configuration;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.SettingModule;

public static class SettingModuleServiceExtensions
{
  public static void AddSettingServiceModule(this IServiceCollection services, SettingModuleOptions options)
  {
    var myOptionsInstance = Options.Create(options);
    services.TryAddSingleton(myOptionsInstance);

    services.TryAddTransient(typeof(IPipelineBehavior<,>), typeof(SettingModulePipelineBehavior<,>));


    if (options.Storages == null)
      throw new ArgumentException($"{nameof(options.Storages)} is null.");
    services.AddDbMongoStorage<SettingModuleMongoStorageImpl>(options.Storages);
    services.AddDbPGStorage<SettingModuleSqlPGStorageImpl>(options.Storages);
    services.AddDbMemoryStorage<SettingModuleSqlMemoryStorageImpl>(options.Storages, nameof(ISettingStorageModule));
  }

  public static async Task UseSettingServiceModule(this IServiceProvider provider)
  {
    var opt = provider.GetService<IOptions<SettingModuleOptions>>()?.Value
              ?? throw new ArgumentException($"{nameof(SettingModuleOptions)} is not configured.");

    if (opt.Storages == null)
      throw new ArgumentException($"{nameof(opt.Storages)} is null.");

    await provider.ConfigureMongoStorage<ISettingStorageModule, SettingModuleMongoStorageImpl>(opt.Storages);
    await provider.ConfigurePGStorage<ISettingStorageModule, SettingModuleSqlPGStorageImpl>(opt.Storages);
    await provider.ConfigureMemoryStorage<ISettingStorageModule, SettingModuleSqlMemoryStorageImpl>(opt.Storages);
  }
}