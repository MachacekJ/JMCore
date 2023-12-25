using JMCore.Server.DB.DbContexts.BasicStructure;
using JMCore.Server.Services.JMCache;
using JMCore.Services.JMCache;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.ServerT.DbT;

/// <summary>
/// Working with memory EF.
/// </summary>
public class DbBaseT : ServerTestBaseT
{
    protected override void RegisterServices(ServiceCollection sc)
    {
        base.RegisterServices(sc);
        sc.AddJMMemoryCache<JMCacheServerCategory>();
        sc.AddMediatR((c) =>
        {
            c.RegisterServicesFromAssemblyContaining(typeof(IBasicDbContext));
        });
    }
}