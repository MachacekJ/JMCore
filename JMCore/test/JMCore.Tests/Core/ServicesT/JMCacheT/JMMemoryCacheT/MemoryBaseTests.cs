using JMCore.Modules.CacheModule;
using JMCore.Tests.BaseInfrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.Core.ServicesT.JMCacheT.JMMemoryCacheT
{
    public class MemoryBaseTests : BaseTests
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