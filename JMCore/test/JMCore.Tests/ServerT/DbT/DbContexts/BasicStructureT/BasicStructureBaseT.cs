using JMCore.Server.DataStorages.Memory.BasicStructure;
using JMCore.Server.DataStorages.PG.BasicStructure;
using JMCore.Server.DB.DbContexts.BasicStructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.ServerT.DbT.DbContexts.BasicStructureT;

public class BasicStructureBaseT : DbBaseT
{
  protected BasicPGDbContext PGDb = null!;


  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    var option = new JMMemoryDbContextConfiguration(sc, nameof(BasicPGDbContext) + TestData.TestName)
    {
      AuditStructure = false,
      LanguageStructure = false
    };
    option.AddJMMemoryDbContext<IBasicDbContext, BasicPGDbContext>();
    
    //sc.AddDbContext<BasicPGDbContext>(opt => opt.UseInMemoryDatabase());
    sc.AddScoped<IBasicDbContext, BasicPGDbContext>();
  }

  protected override async Task GetServicesAsync(IServiceProvider sp)
  {
    await base.GetServicesAsync(sp);
    PGDb = sp.GetService<BasicPGDbContext>() ?? throw new ArgumentException($"{nameof(BasicPGDbContext)} is null.");
  }
}