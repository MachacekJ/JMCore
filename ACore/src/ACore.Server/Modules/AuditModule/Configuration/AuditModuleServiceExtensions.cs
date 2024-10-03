using ACore.Server.Modules.AuditModule.CQRS;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Modules.AuditModule.Storage.Mongo;
using ACore.Server.Modules.AuditModule.Storage.SQL.Memory;
using ACore.Server.Modules.AuditModule.Storage.SQL.PG;
using ACore.Server.Modules.SettingsDbModule.Configuration;
using ACore.Server.Storages.Configuration;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.AuditModule.Configuration;

internal static class AuditModuleServiceExtensions
{
  public static void AddAuditModule(this IServiceCollection services, AuditModuleOptions options)
  {
    var myOptionsInstance = Options.Create(options);
    services.TryAddSingleton(myOptionsInstance);

    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuditModulePipelineBehavior<,>));
    
    if (options.Storages == null)
      throw new ArgumentException($"{nameof(options.Storages)} is null.");

    services.AddDbMongoStorage<AuditModuleMongoStorageImpl>(options.Storages);
    services.AddDbPGStorage<AuditPGEFStorageImpl>(options.Storages);
    services.AddDbMemoryStorage<AuditSqlMemoryStorageImpl>(options.Storages, nameof(IAuditStorageModule));
  }

  public static async Task UseAuditServiceModule(this IServiceProvider provider)
  {
    var opt = provider.GetService<IOptions<AuditModuleOptions>>()?.Value
              ?? throw new ArgumentException($"{nameof(SettingsDbModuleOptions)} is not configured.");

    if (opt.Storages == null)
      throw new ArgumentException($"{nameof(opt.Storages)} is null.");

    await provider.ConfigureMongoStorage<IAuditStorageModule, AuditModuleMongoStorageImpl>(opt.Storages);
    await provider.ConfigurePGStorage<IAuditStorageModule, AuditPGEFStorageImpl>(opt.Storages);
    await provider.ConfigureMemoryStorage<IAuditStorageModule, AuditSqlMemoryStorageImpl>(opt.Storages);
  }
}