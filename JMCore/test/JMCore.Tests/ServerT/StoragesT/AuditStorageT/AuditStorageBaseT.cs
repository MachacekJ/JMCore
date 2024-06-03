using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Base.Audit.EF;
using JMCore.Server.Storages.Base.Audit.UserProvider;
using JMCore.Server.Storages.Modules.AuditModule;
using JMCore.Server.Storages.Modules.AuditModule.EF;
using JMCore.Server.Storages.Modules.BasicModule;
using JMCore.Tests.ServerT.StoragesT.Impl.MemoryStorage;
using JMCore.Tests.ServerT.StoragesT.Impl.TestStorageModule;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.ServerT.StoragesT.AuditStorageT;

public class AuditStorageBaseT : DbBaseT
{
  protected AuditStorageEfContext AuditEfStorageEfContext = null!;
  protected TestStorageEfContext TestStorageEfContext = null!;
  protected IAuditUserProvider UserProvider = null!;

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    StorageResolver.RegisterStorage(sc, new MemoryStorageConfiguration(new[] { nameof(IBasicStorageModule), nameof(IAuditStorageModule), nameof(ITestStorageModule) }));
    sc.AddSingleton<IAuditUserProvider>(TestAuditUserProvider.CreateDefaultUser());
    sc.AddScoped<IAuditDbService, AuditDbService>();
  }

  protected override async Task GetServicesAsync(IServiceProvider sp)
  {
    await base.GetServicesAsync(sp);
    var auditStorageModule = StorageResolver.FirstStorageModuleImplementation<IAuditStorageModule>(StorageTypeEnum.Memory);
    AuditEfStorageEfContext = (auditStorageModule as AuditStorageEfContext) ?? throw new ArgumentException();

    var testStorageModule = StorageResolver.FirstStorageModuleImplementation<ITestStorageModule>(StorageTypeEnum.Memory);
    TestStorageEfContext = (testStorageModule as TestStorageEfContext) ?? throw new ArgumentException();

    UserProvider = sp.GetService<IAuditUserProvider>() ?? throw new ArgumentException($"{nameof(IAuditUserProvider)} is null.");
  }
}