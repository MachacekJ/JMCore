using JMCore.Server.Storages;
using JMCore.Server.Storages.DbContexts.BasicStructure;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.TestsIntegrations.ServerT.DbT.BasicStructureT;

public class BasicStructureBaseT : DbBaseT
{
    protected BasicDbContext Db = null!;


    protected override void RegisterServices(ServiceCollection sc)
    {
        base.RegisterServices(sc);
        sc.AddJMDb(ConnectionString);
    }

    protected override async Task GetServicesAsync(IServiceProvider sp)
    {
        await base.GetServicesAsync(sp);
        await sp.ConfigureJMDbAsync();
        Db = sp.GetService<BasicDbContext>() ?? throw new ArgumentException($"{nameof(BasicDbContext)} is null.");
    }
}