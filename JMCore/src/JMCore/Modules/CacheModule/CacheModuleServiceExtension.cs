using JMCore.CQRS;
using JMCore.Modules.CacheModule.CQRS;
using JMCore.Modules.CacheModule.Storages;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Modules.CacheModule;

public static class CacheModuleServiceExtension
{
  public static void AddJMMemoryCache<T>(this IServiceCollection services, Action<MemoryCacheOptions>? setupAction = null) where T : JMCacheCategory
  {
    if (setupAction != null)
      services.AddMemoryCache(setupAction);
    else
      services.AddMemoryCache();
    
    services.AddCQRS();
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheBehavior<,>));
    services.AddSingleton<IJMCacheCategories, T>();
    services.AddSingleton<IJMCache, JMMemoryCache>();
  }
}