using JMCore.Server.CQRS.DB.BasicStructure.SettingSave;
using JMCore.Server.DataStorages;
using JMCore.Server.DataStorages.PG;
using JMCore.Server.DataStorages.PG.BasicStructure;
using JMCore.Server.DB;
using JMCore.Server.DB.Abstract;
using JMCore.Server.DB.Audit;
using JMCore.Server.DB.DbContexts.AuditStructure;
using JMCore.Server.DB.DbContexts.BasicStructure;
using JMCore.Tests.ServerT.DbT.TestDBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.ServerT.DbT.DbContexts.AuditStructureT;

public class AuditStructureBaseT : DbBaseT
{
    protected AuditDbContext AuditDbContext = null!;
    protected TestBasicPGDbContext TestBasicPGDbContext = null!;
    protected IAuditUserProvider UserProvider = null!;

    protected override void RegisterServices(ServiceCollection sc)
    {
        base.RegisterServices(sc);
        sc.AddDbContext<BasicPGDbContext>(opt => opt.UseInMemoryDatabase(nameof(AuditDbContext) + TestData.TestName));
        sc.AddScoped<IBasicDbContext, BasicPGDbContext>();
        sc.AddDbContext<AuditDbContext>(opt => opt.UseInMemoryDatabase(nameof(AuditDbContext) + TestData.TestName));
        sc.AddScoped<IAuditDbContext, AuditDbContext>();
        sc.AddDbContext<TestBasicPGDbContext>(opt => opt.UseInMemoryDatabase(nameof(TestBasicPGDbContext) + TestData.TestName));
        sc.AddScoped<ITestBasicDbContext, TestBasicPGDbContext>();

        sc.AddScoped<IAuditDbService, AuditDbService>();
        sc.AddSingleton<IAuditUserProvider>(TestAuditUserProvider.CreateDefaultUser());

        var option = new JMPGDbContextConfiguration(sc, string.Empty)
        {
            AuditStructure = true
        };
        sc.AddSingleton<IJMDbContextConfiguration>(option);

  
        
    }

    protected override async Task GetServicesAsync(IServiceProvider sp)
    {
        await base.GetServicesAsync(sp);
        await Mediator.Send(new SettingSaveCommand(DbContextBase.DbContextVersionKeyPrefix + nameof(AuditDbContext), "1.0.0.0"));
        AuditDbContext = sp.GetService<AuditDbContext>() ?? throw new ArgumentException($"{nameof(AuditDbContext)} is null.");
        TestBasicPGDbContext = sp.GetService<TestBasicPGDbContext>() ?? throw new ArgumentException($"{nameof(TestBasicPGDbContext)} is null.");
        UserProvider = sp.GetService<IAuditUserProvider>() ?? throw new ArgumentException($"{nameof(IAuditUserProvider)} is null.");
    }
}