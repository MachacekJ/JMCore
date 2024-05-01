using JMCore.Server.CQRS.DB.BasicStructure.SettingSave;
using JMCore.Server.Storages;
using JMCore.Server.Storages.Abstract;
using JMCore.Server.Storages.Audit;
using JMCore.Server.Storages.DbContexts.AuditStructure;
using JMCore.Server.Storages.DbContexts.BasicStructure;
using JMCore.Tests.ServerT.DbT.TestDBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.ServerT.DbT.DbContexts.AuditStructureT;

public class AuditStructureBaseT : DbBaseT
{
    protected AuditDbContext AuditDbContext = null!;
    protected TestBasicDbContext TestBasicDbContext = null!;
    protected IAuditUserProvider UserProvider = null!;

    protected override void RegisterServices(ServiceCollection sc)
    {
        base.RegisterServices(sc);
        sc.AddDbContext<BasicDbContext>(opt => opt.UseInMemoryDatabase(nameof(AuditDbContext) + TestData.TestName));
        sc.AddScoped<IBasicDbContext, BasicDbContext>();
        sc.AddDbContext<AuditDbContext>(opt => opt.UseInMemoryDatabase(nameof(AuditDbContext) + TestData.TestName));
        sc.AddScoped<IAuditDbContext, AuditDbContext>();
        sc.AddDbContext<TestBasicDbContext>(opt => opt.UseInMemoryDatabase(nameof(TestBasicDbContext) + TestData.TestName));
        sc.AddScoped<ITestBasicDbContext, TestBasicDbContext>();

        sc.AddScoped<IAuditDbService, AuditDbService>();
        sc.AddSingleton<IAuditUserProvider>(TestAuditUserProvider.CreateDefaultUser());

        var option = new JMDbContextConfiguration(sc, string.Empty)
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
        TestBasicDbContext = sp.GetService<TestBasicDbContext>() ?? throw new ArgumentException($"{nameof(TestBasicDbContext)} is null.");
        UserProvider = sp.GetService<IAuditUserProvider>() ?? throw new ArgumentException($"{nameof(IAuditUserProvider)} is null.");
    }
}