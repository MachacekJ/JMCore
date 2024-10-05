using ACore.Base.CQRS.Extensions;
using Autofac;
using ACore.Configuration;
using ACore.Configuration.CQRS;
using ACore.Server.Configuration.CQRS.OptionsGet;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.AuditModule.CQRS.AuditGet;
using ACore.Server.Modules.ICAMModule.Configuration;
using ACore.Server.Modules.SettingsDbModule.Configuration;
using ACore.Server.Storages.Configuration;
using ACore.Server.Storages.Services.StorageResolvers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ACore.Server.Configuration;

public static class ACoreServerServiceExtensions
{
  public static void AddACoreServer(this IServiceCollection services, Action<ACoreServerOptionBuilder>? optionsBuilder = null)
  {
    var aCoreServerOptionBuilder = ACoreServerOptionBuilder.Empty();
    optionsBuilder?.Invoke(aCoreServerOptionBuilder);
    var aCoreServerOptions = aCoreServerOptionBuilder.Build();
    AddACoreServer(services, aCoreServerOptions);
  }

  public static void AddACoreServer(this IServiceCollection services, ACoreServerOptions aCoreServerOptions)
  {
    ValidateDependencyInConfiguration(aCoreServerOptions);

    services.AddACore(aCoreServerOptions.ACoreOptions);

    var myOptionsInstance = Options.Create(aCoreServerOptions);
    services.AddSingleton(myOptionsInstance);

    // Adding CQRS only from ACore assembly.
    services.AddCQRS();
    // Adding CQRS from ACore.Server assembly.
    services.AddMediatR(c =>
    {
      c.RegisterServicesFromAssemblyContaining(typeof(ACoreServerServiceExtensions));
      c.AllNotificationWithoutException();
    });
    services.AddValidatorsFromAssembly(typeof(ACoreServerServiceExtensions).Assembly, includeInternalTypes: true);

    services.TryAddSingleton<IStorageResolver>(new DefaultStorageResolver());

    if (aCoreServerOptions.SettingsDbModuleOptions.IsActive)
      services.AddSettingsDbModule(aCoreServerOptions.SettingsDbModuleOptions);

    if (aCoreServerOptions.AuditModuleOptions.IsActive)
      services.AddAuditModule(aCoreServerOptions.AuditModuleOptions);

    if (aCoreServerOptions.ICAMModuleOptions.IsActive)
      services.AddICAMModule(aCoreServerOptions.ICAMModuleOptions);
  }

  public static async Task UseACoreServer(this IServiceProvider provider)
  {
    var opt = provider.GetService<IOptions<ACoreServerOptions>>()?.Value ?? throw new Exception($"{nameof(ACoreOptions)} is not registered.");

    if (opt.SettingsDbModuleOptions.IsActive)
      await provider.UseSettingServiceModule();
    if (opt.AuditModuleOptions.IsActive)
      await provider.UseAuditServiceModule();
  }

  public static void ConfigureAutofacACoreServer(this ContainerBuilder containerBuilder)
  {
    containerBuilder.RegisterGeneric(typeof(AppOptionHandler<>)).AsImplementedInterfaces();
    containerBuilder.RegisterGeneric(typeof(AuditGetHandler<>)).AsImplementedInterfaces();
  }
  
  private static void ValidateDependencyInConfiguration(ACoreServerOptions aCoreServerOptions)
  {
    ValidateSettingsDbOptions(aCoreServerOptions);
    ValidateAuditModuleOptions(aCoreServerOptions);
  }

  private static void ValidateAuditModuleOptions(ACoreServerOptions aCoreServerOptions)
  {
    if (!aCoreServerOptions.AuditModuleOptions.IsActive)
      return;

    if (!aCoreServerOptions.ICAMModuleOptions.IsActive)
      throw new Exception($"Module {nameof(ACore.Server.Modules.ICAMModule)} must be activated.");
    
    if (aCoreServerOptions.SettingsDbModuleOptions.IsActive == false)
      throw new Exception($"Module {nameof(ACore.Server.Modules.SettingsDbModule)} must be activated.");

    if (aCoreServerOptions.AuditModuleOptions.Storages == null && aCoreServerOptions.DefaultStorages == null)
      throw new Exception($"Module {nameof(ACore.Server.Modules.AuditModule)} must have {nameof(StorageOptions)}.");
  }

  private static void ValidateSettingsDbOptions(ACoreServerOptions aCoreServerOptions)
  {
    if (aCoreServerOptions.SettingsDbModuleOptions.Storages == null && aCoreServerOptions.DefaultStorages == null)
      throw new Exception($"Module {nameof(ACore.Server.Modules.SettingsDbModule)} must have {nameof(StorageOptions)}.");
  }
}