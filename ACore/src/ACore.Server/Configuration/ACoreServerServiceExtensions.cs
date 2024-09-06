using ACore.Configuration;
using ACore.CQRS;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ACore.Server.Configuration;

public static class ACoreServerServiceExtensions
{
  public static void AddACoreServer(this IServiceCollection services, Action<ACoreServerServiceOptions>? options = null)
  {
    var opt = new ACoreServerServiceOptions();
    options?.Invoke(opt);
    AddACoreServer(services, opt);
  }

  public static void AddACoreServer(this IServiceCollection services, ACoreServerServiceOptions opt)
  {
    services.AddACore(opt.ACoreServiceOptions);
    
    var myOptionsInstance = Options.Create(opt);
    services.AddSingleton(myOptionsInstance);
    
    
    services.AddCQRS();
    services.AddMediatR((c) =>
    {
      c.RegisterServicesFromAssemblyContaining(typeof(ACoreServerServiceExtensions));
    });
    
    services.AddValidatorsFromAssembly(typeof(ACoreServerServiceExtensions).Assembly,
      includeInternalTypes: true);
  }
}