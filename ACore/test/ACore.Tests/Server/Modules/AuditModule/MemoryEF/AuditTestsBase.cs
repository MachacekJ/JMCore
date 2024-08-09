using ACore.Server.Configuration;
using ACore.Server.Modules.AuditModule.UserProvider;
using ACore.Tests.Server.Storages;
using ACore.Tests.Server.TestImplementations.Server;
using ACore.Tests.Server.TestImplementations.Server.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ACore.Tests.Server.Modules.AuditModule.MemoryEF;

public class AuditTestsBase : StorageTestsBase
{
  private IAuditUserProvider UserProvider = TestAuditUserProvider.CreateDefaultUser();

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    sc.AddACoreTest(ot =>
    {
      ot.AddTestModule()
        .ACoreServer(o =>
        {
          MemoryStorageConfiguration.Invoke(o);
          o.AddAuditModule(a => { a.UserProvider(UserProvider); });
        });
    });
  }

  protected override async Task GetServices(IServiceProvider sp)
  {
    await base.GetServices(sp);
    await sp.UseACoreTest();
    UserProvider = sp.GetService<IOptions<ACoreServerOptions>>()?.Value.AuditModuleOptions.AuditUserProvider ?? throw new ArgumentException($"{nameof(IAuditUserProvider)} is null.");
  }
  
  protected string GetTableName(string entityName)
  {
    return entityName;
  }

  protected string GetColumnName(string entityName, string propertyName)
  {
    return propertyName;
  }
}