using ACore.AppTest;
using ACore.AppTest.Modules.TestModule;
using ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit;
using ACore.AppTest.Modules.TestModule.Storages.EF;
using ACore.AppTest.Modules.TestModule.Storages.Memory;
using ACore.Server.Modules.AuditModule.CQRS.Audit;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Modules.AuditModule.UserProvider;
using ACore.Server.Modules.SettingModule.Storage;
using ACore.Tests.Server.Storages;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.Modules.TestModule;

public class AuditStorageBaseTests : StorageBaseTests
{
  protected IAuditUserProvider UserProvider = null!;

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    sc.AddTestServiceModule();
    StorageResolver.RegisterStorage(sc, new MemoryTestStorageConfiguration(new[]
    {
      nameof(IBasicStorageModule),
      nameof(IAuditStorageModule),
      nameof(IEFTestStorageModule)
    }));
  }

  protected override async Task GetServicesAsync(IServiceProvider sp)
  {
    await base.GetServicesAsync(sp);
    UserProvider = sp.GetService<IAuditUserProvider>() ?? throw new ArgumentException($"{nameof(IAuditUserProvider)} is null.");
  }

  protected override void RegisterAutofacContainer(ServiceCollection services, ContainerBuilder containerBuilder)
  {
    base.RegisterAutofacContainer(services, containerBuilder);
    RegisterAutofacContainerStatic(containerBuilder);
  }

  protected string GetTableName(string entityName)
  {
    return entityName;
  }

  protected string GetColumnName(string entityName, string propertyName)
  {
    return propertyName;
  }

  public static void RegisterAutofacContainerStatic(ContainerBuilder containerBuilder)
  {
    containerBuilder.RegisterGeneric(typeof(TestAttributeAuditGetHandler<>)).AsImplementedInterfaces();
    containerBuilder.RegisterGeneric(typeof(TestAttributeAuditSaveHandler<>)).AsImplementedInterfaces();
    containerBuilder.RegisterGeneric(typeof(TestAttributeAuditDeleteHandler<>)).AsImplementedInterfaces();
    containerBuilder.RegisterGeneric(typeof(AuditGetHandler<>)).AsImplementedInterfaces();
  }

  public static void RegisterServicesStatic()
  {
  }
}