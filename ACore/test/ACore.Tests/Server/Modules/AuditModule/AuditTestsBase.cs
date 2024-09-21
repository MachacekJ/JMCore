using ACore.Server.Configuration;
using ACore.Server.Modules.AuditModule.UserProvider;
using ACore.Tests.Server.Storages;
using ACore.Tests.TestImplementations.Server.Configuration;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ACore.Tests.Server.Modules.AuditModule;

public class AuditTestsBase : StorageTestsBase
{
  protected IAuditUserProvider UserProvider = TestAuditUserProvider.CreateDefaultUser();
  
  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    sc.AddACoreTest(ot =>
    {
      ot.AddTestModule()
        .ACoreServer(o =>
      {
        MemoryStorageConfiguration.Invoke(o);
        o.AddAuditModule(a =>
        {
          a.UserProvider(UserProvider);
        });
      });
    });
  }

  protected override async Task GetServices(IServiceProvider sp)
  {
    await base.GetServices(sp);
    await sp.UseACoreTest();
    UserProvider = sp.GetService<IOptions<ACoreServerOptions>>()?.Value.AuditModuleOptions.AuditUserProvider ?? throw new ArgumentException($"{nameof(IAuditUserProvider)} is null.");
  }

  protected override void SetContainer(ContainerBuilder containerBuilder)
  {
    base.SetContainer(containerBuilder);
    containerBuilder.AddACoreTest();
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