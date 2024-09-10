using ACore.Configuration;
using ACore.CQRS;
using ACore.Server.Modules.AuditModule;
using ACore.Server.Modules.SettingModule;
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
    var opt = provider.GetService<IOptions<ACoreServerServiceOptions>>()?.Value ?? throw new Exception($"{nameof(ACoreServiceOptions)} is not registered.");
   
    if (opt.SettingModuleOptions.IsActive)
      await provider.UseSettingServiceModule();
    if (opt.AuditModuleOptions.IsActive)
      await provider.UseAuditServiceModule(opt.AuditModuleOptions);
  }

  private static void AddACoreServer(this IServiceCollection services, ACoreServerServiceOptions opt)
  {
    services.AddACore(opt.ACoreServiceOptions);

    var myOptionsInstance = Options.Create(opt);
    services.AddSingleton(myOptionsInstance);

    services.AddCQRS();
    services.AddMediatR((c) => { c.RegisterServicesFromAssemblyContaining(typeof(ACoreServerServiceExtensions)); });
    services.AddValidatorsFromAssembly(typeof(ACoreServerServiceExtensions).Assembly,
      includeInternalTypes: true);

    services.TryAddSingleton<IStorageResolver>(new DefaultStorageResolver());
    if (opt.SettingModuleOptions.IsActive)
      services.AddSettingServiceModule(opt.SettingModuleOptions);

    if (opt.AuditModuleOptions.IsActive)
      services.AddAuditServiceModule(opt.AuditModuleOptions);
  }
}