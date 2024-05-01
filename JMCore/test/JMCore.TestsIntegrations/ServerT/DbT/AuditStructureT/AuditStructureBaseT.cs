using JMCore.Server.DataStorages;
using JMCore.Server.DB;
using JMCore.Server.DB.Audit;
using JMCore.Server.DB.DbContexts.AuditStructure;
using JMCore.Tests.ServerT.DbT.DbContexts.AuditStructureT;
using JMCore.Tests.ServerT.DbT.TestDBContext;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.TestsIntegrations.ServerT.DbT.AuditStructureT;

public class AuditStructureBaseT : DbBaseT
{
    protected AuditDbContext AuditDbContext = null!;
    protected TestBasicPGDbContext TestBasicPGDbContext = null!;


    protected override void RegisterServices(ServiceCollection sc)
    {
        base.RegisterServices(sc);
        sc.AddSingleton<IAuditUserProvider>(TestAuditUserProvider.CreateDefaultUser());
        sc.AddJMStorage(string.Format(ConnectionString, TestData.TestName), (o) =>
        {
            o.AuditStructure = true;
            o.AddJMPostgresDbContext<ITestBasicDbContext, TestBasicPGDbContext>();
        });
    }

    protected override async Task GetServicesAsync(IServiceProvider sp)
    {
        await base.GetServicesAsync(sp);
        await sp.ConfigureJMDbAsync();
        AuditDbContext = sp.GetService<AuditDbContext>() ?? throw new ArgumentException($"{nameof(AuditDbContext)} is null.");
        TestBasicPGDbContext = sp.GetService<TestBasicPGDbContext>() ?? throw new ArgumentException($"{nameof(TestBasicPGDbContext)} is null.");
    }
}