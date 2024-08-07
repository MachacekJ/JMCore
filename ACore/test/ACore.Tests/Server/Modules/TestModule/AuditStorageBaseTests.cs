using ACore.AppTest;
using ACore.AppTest.Modules.TestModule;
using ACore.AppTest.Modules.TestModule.Storages.EF.Memory;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Modules.AuditModule.UserProvider;
using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Storages.Models;
using ACore.Tests.Server.Storages;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.Modules.TestModule;

public class AuditStorageBaseTests : StorageBaseTests
{
  //protected IAuditStorageModule AuditStorageModule = null!;

  // protected ITestStorageModule TestStorageModule = null!;
  protected IAuditUserProvider UserProvider = null!;

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    sc.AddTestServiceModule();
    StorageResolver.RegisterStorage(sc, new MemoryTestStorageConfiguration(new[]
    {
      nameof(IBasicStorageModule), 
      nameof(IAuditStorageModule),
      AppTestModulesNames.TestModule
    }));
  }

  protected override async Task GetServicesAsync(IServiceProvider sp)
  {
    await base.GetServicesAsync(sp);
  //  var auditStorageModule = StorageResolver.FirstReadWriteStorage<IAuditStorageModule>(StorageTypeEnum.Memory);
   // AuditStorageModule = (auditStorageModule as AuditSqlStorageImpl) ?? throw new ArgumentException();

    //var testStorageModule = StorageResolver.FirstReadWriteStorage<ITestStorageModule>(StorageTypeEnum.Memory);
    // TestStorageModule = (testStorageModule as TestStorageEfContext) ?? throw new ArgumentException();

    UserProvider = sp.GetService<IAuditUserProvider>() ?? throw new ArgumentException($"{nameof(IAuditUserProvider)} is null.");
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