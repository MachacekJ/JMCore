using ACore.Server.Modules.AuditModule.UserProvider;
using ACore.Tests.Server.TestImplementations.Server;
using ACore.Tests.Server.TestImplementations.Server.Configuration;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.Storages.EF;

public class EFTestBase : StorageTestsBase
{
  private readonly IAuditUserProvider _userProvider = TestAuditUserProvider.CreateDefaultUser();

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    sc.AddACoreTest(ot =>
    {
      ot.AddTestModule()
        .ACoreServer(o =>
        {
          MemoryStorageConfiguration.Invoke(o);
          o.AddAuditModule(a => { a.UserProvider(_userProvider); });
        });
    });
  }
  
  protected override async Task GetServices(IServiceProvider sp)
  {
    await base.GetServices(sp);
    await sp.UseACoreTest();
  }
  
  protected override void SetContainer(ContainerBuilder containerBuilder)
  {
    base.SetContainer(containerBuilder);
    containerBuilder.AddACoreTest();
  }
}