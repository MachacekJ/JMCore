using ACore.Configuration;
using ACore.CQRS;
using ACore.Server.Modules.SettingModule;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
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
    await provider.UseSettingServiceModule();
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

    services.AddSettingServiceModule(opt.SettingModuleOptions);
  }
}