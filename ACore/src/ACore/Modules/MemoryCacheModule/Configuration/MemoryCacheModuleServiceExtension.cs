using ACore.Modules.MemoryCacheModule.CQRS;
using ACore.Modules.MemoryCacheModule.Storages;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Modules.MemoryCacheModule.Configuration;

public static class MemoryCacheModuleServiceExtension
{
  public static void AddMemoryCacheModule(this IServiceCollection services, MemoryCacheModuleOptions options)
  {
    if (options.MemoryCacheOptionAction != null)
      services.AddMemoryCache(options.MemoryCacheOptionAction);
    else
      services.AddMemoryCache();
    
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MemoryCacheModulePipelineBehavior<,>));
    services.AddSingleton<IMemoryCacheModuleStorage, MemoryCacheModuleModuleStorage>();
  }
}