using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Base.Audit.UserProvider;
using JMCore.Server.Storages.Modules.AuditModule;
using JMCore.Server.Storages.Modules.AuditModule.BaseImpl;
using JMCore.Server.Storages.Modules.BasicModule;
using JMCore.Tests.ServerT.StoragesT.Implementations.MemoryStorage;
using JMCore.Tests.ServerT.StoragesT.Implementations.TestStorageModule;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.ServerT.StoragesT.ModulesT.AuditStorageT;

public class AuditStorageBaseT : DbBaseT
{
  protected IAuditStorageModule AuditStorageModule = null!;
  protected ITestStorageModule TestStorageModule = null!;
  protected IAuditUserProvider UserProvider = null!;

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    StorageResolver.RegisterStorage(sc, new MemoryStorageConfiguration(new[] { nameof(IBasicStorageModule), nameof(IAuditStorageModule), nameof(ITestStorageModule) }));
  }

  protected override async Task GetServicesAsync(IServiceProvider sp)
  {
    await base.GetServicesAsync(sp);
    var auditStorageModule = StorageResolver.FirstStorageModuleImplementation<IAuditStorageModule>(StorageTypeEnum.Memory);
    AuditStorageModule = (auditStorageModule as AuditSqlStorageImpl) ?? throw new ArgumentException();

    var testStorageModule = StorageResolver.FirstStorageModuleImplementation<ITestStorageModule>(StorageTypeEnum.Memory);
    TestStorageModule = (testStorageModule as TestStorageEfContext) ?? throw new ArgumentException();

    UserProvider = sp.GetService<IAuditUserProvider>() ?? throw new ArgumentException($"{nameof(IAuditUserProvider)} is null.");
  }
}