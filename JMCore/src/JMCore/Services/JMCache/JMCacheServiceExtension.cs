using JMCore.CQRS.JMCache;
using JMCore.Services.JMCache.Implementations;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Services.JMCache;


public static class JMCacheServiceExtension
{
    public static void AddJMMemoryCache<T>(this IServiceCollection services, Action<MemoryCacheOptions>? setupAction = null) where T : JMCacheCategory
    {
        if (setupAction != null)
            services.AddMemoryCache(setupAction);
        else
            services.AddMemoryCache();

        services.AddMediatR((c) =>
        {
            c.RegisterServicesFromAssemblyContaining(typeof(IJMCache));
        });
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheBehavior<,>));
        services.AddSingleton<IJMCacheCategories, T>();
        services.AddSingleton<IJMCache, JMMemoryCache>();
    }
}