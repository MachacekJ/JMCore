using JMCore.Services.JMCache;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.CoreT.ServicesT.JMCacheT.JMMemoryCacheT
{
    public class MemoryBaseT : TestBaseT
    {
        protected IJMCache MemoryCache { get; set; } = null!;

        protected override void RegisterServices(ServiceCollection sc)
        {
            base.RegisterServices(sc);
            sc.AddJMMemoryCache<JMCacheTestCategoryT>();
        }

        protected override async Task GetServicesAsync(IServiceProvider sp)
        {
            await base.GetServicesAsync(sp);
            MemoryCache = sp.GetService<IJMCache>() ?? throw new ArgumentException($"{nameof(IJMCache)} is null.");
        }
    }
}