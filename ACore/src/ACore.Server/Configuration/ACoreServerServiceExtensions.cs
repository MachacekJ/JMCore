using ACore.Configuration;
using ACore.Configuration.CQRS;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.SettingsDbModule.Configuration;
using ACore.Server.Storages;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ACore.Server.Configuration;

public static class ACoreServerServiceExtensions
{
  public static void AddACoreServer(this IServiceCollection services, Action<ACoreServerServiceOptionBuilder>? options = null)
  {
    var opt = ACoreServerServiceOptionBuilder.Empty();
    options?.Invoke(opt);
    var oo = opt.Build();
    AddACoreServer(services, oo);
  }

  public static async Task UseACoreServer(this IServiceProvider provider)
  {
    var opt = provider.GetService<IOptions<ACoreServerServiceOptions>>()?.Value ?? throw new Exception($"{nameof(ACoreOptions)} is not registered.");
   
    if (opt.SettingsDbModuleOptions.IsActive)
      await provider.UseSettingServiceModule();
    if (opt.AuditServerModuleOptions.IsActive)
      await provider.UseAuditServiceModule();
  }

  private static void AddACoreServer(this IServiceCollection services, ACoreServerServiceOptions opt)
  {
    services.AddACore(opt.ACoreOptions);

    var myOptionsInstance = Options.Create(opt);
    services.AddSingleton(myOptionsInstance);

    services.AddCQRS();
    services.AddMediatR((c) => { c.RegisterServicesFromAssemblyContaining(typeof(ACoreServerServiceExtensions)); });
    services.AddValidatorsFromAssembly(typeof(ACoreServerServiceExtensions).Assembly,
      includeInternalTypes: true);

    services.TryAddSingleton<IStorageResolver>(new DefaultStorageResolver());
    if (opt.SettingsDbModuleOptions.IsActive)
      services.AddSettingServiceModule(opt.SettingsDbModuleOptions);

    if (opt.AuditServerModuleOptions.IsActive)
      services.AddAuditServiceModule(opt.AuditServerModuleOptions);
  }
}