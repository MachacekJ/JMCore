using ACore.AppTest.Modules.TestModule.Configuration;
using ACore.AppTest.Modules.TestModule.Configuration.Options;
using ACore.Server.Modules.AuditModule.UserProvider;
using ACore.Tests.Server.Storages;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.Modules.TestModule;

public class AuditStorageBase : StorageBase
{
  protected IAuditUserProvider? UserProvider;
  private readonly TestModuleOptions _testModuleConfig = new()
  {
    UseMemoryStorage = true
  };
  
  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    sc.AddTestServiceModule(_testModuleConfig);
  }

  protected override async Task GetServicesAsync(IServiceProvider sp)
  {
    await base.GetServicesAsync(sp);
    await sp.UseTestServiceModule(_testModuleConfig);
    UserProvider = sp.GetService<IAuditUserProvider>() ?? throw new ArgumentException($"{nameof(IAuditUserProvider)} is null.");
  }

  protected override void RegisterAutofacContainer(ServiceCollection services, ContainerBuilder containerBuilder)
  {
    base.RegisterAutofacContainer(services, containerBuilder);
    containerBuilder.RegisterAutofacTestService();
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