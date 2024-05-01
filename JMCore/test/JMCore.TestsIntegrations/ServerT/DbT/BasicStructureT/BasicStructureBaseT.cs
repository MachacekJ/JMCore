using JMCore.Server.DataStorages;
using JMCore.Server.DataStorages.PG.BasicStructure;
using JMCore.Server.DB;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.TestsIntegrations.ServerT.DbT.BasicStructureT;

public class BasicStructureBaseT : DbBaseT
{
    protected BasicPGDbContext PGDb = null!;


    protected override void RegisterServices(ServiceCollection sc)
    {
        base.RegisterServices(sc);
        sc.AddJMStorage(ConnectionString);
    }

    protected override async Task GetServicesAsync(IServiceProvider sp)
    {
        await base.GetServicesAsync(sp);
        await sp.ConfigureJMDbAsync();
        PGDb = sp.GetService<BasicPGDbContext>() ?? throw new ArgumentException($"{nameof(BasicPGDbContext)} is null.");
    }
}