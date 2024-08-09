using ACore.Configuration.CQRS;
using ACore.Modules.MemoryCacheModule.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ACore.Configuration;

public static class ACoreServiceExtension
{
  public static void AddACore(this IServiceCollection services, Action<ACoreOptionsBuilder>? options = null)
  {
    var opt = ACoreOptionsBuilder.Empty();
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