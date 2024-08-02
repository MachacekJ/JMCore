using ACore.Blazor.Services;
using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using ACore.Modules.CacheModule;

namespace ACore.Blazor;

public static class JMCoreBlazorExtensions
{
    public static void AddJMBlazor(this IServiceCollection services)
    {
        services.AddJMMemoryCache<JMBlazorCacheCategory>();
        services.AddTelerikBlazor();

        services.AddBlazoredLocalStorage();

        services.AddOptions();

        services.AddMediatR((c) =>
        {
            c.RegisterServicesFromAssemblyContaining(typeof(JMComponentBase));
        });
    }
}