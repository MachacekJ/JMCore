using JMCore.Server.Modules.AuditModule.Storage;
using JMCore.Server.Modules.AuditModule.UserProvider;
using JMCore.Server.Modules.SettingModule.Storage;
using JMCore.Server.Storages.Models;
using JMCore.Tests.Implementations.Modules.TestModule;
using JMCore.Tests.Implementations.Modules.TestModule.Storages;
using JMCore.Tests.Implementations.Modules.TestModule.Storages.Memory;
using JMCore.Tests.Server.Storages;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.Server.Modules.TestModule;

public class AuditStorageBaseTests : StorageBaseTests
{
  protected IAuditStorageModule AuditStorageModule = null!;
  protected ITestStorageModule TestStorageModule = null!;
  protected IAuditUserProvider UserProvider = null!;

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    sc.AddTestServiceModule();
    StorageResolver.RegisterStorage(sc, new MemoryTestStorageConfiguration(new[] { nameof(IBasicStorageModule), nameof(IAuditStorageModule), nameof(ITestStorageModule) }));
  }

  protected override async Task GetServicesAsync(IServiceProvider sp)
  {
    await base.GetServicesAsync(sp);
    var auditStorageModule = StorageResolver.FirstReadWriteStorage<IAuditStorageModule>(StorageTypeEnum.Memory);
    AuditStorageModule = (auditStorageModule as AuditSqlStorageImpl) ?? throw new ArgumentException();

    var testStorageModule = StorageResolver.FirstReadWriteStorage<ITestStorageModule>(StorageTypeEnum.Memory);
    TestStorageModule = (testStorageModule as TestStorageEfContext) ?? throw new ArgumentException();

    UserProvider = sp.GetService<IAuditUserProvider>() ?? throw new ArgumentException($"{nameof(IAuditUserProvider)} is null.");
  }
  
  protected string GetTableName(Type entityName)
  {
    return entityName.Name;
  }

  protected string GetColumnName(Type entityName, string propertyName)
  {
    return propertyName;
  }
}