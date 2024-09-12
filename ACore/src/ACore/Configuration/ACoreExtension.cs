using ACore.CQRS;
using ACore.Modules.MemoryCacheModule.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ACore.Configuration;

public static class ACoreExtension
{
  public static void AddACore(this IServiceCollection services, Action<ACoreOptionBuilder>? options = null)
  {
    var opt = ACoreOptionBuilder.Empty();
    options?.Invoke(opt);
    AddACore(services, opt.Build());
  }
  
  public static void AddACore(this IServiceCollection services, ACoreOptions opt)
  {
    var myOptionsInstance = Options.Create(opt);
    services.AddSingleton(myOptionsInstance);
    
    services.AddCQRS();

    services.AddMemoryCacheModule(opt.MemoryCacheModuleOptions);
  }
}