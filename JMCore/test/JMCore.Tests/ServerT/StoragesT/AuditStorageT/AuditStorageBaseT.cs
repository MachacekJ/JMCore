using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Base.Audit.Configuration;
using JMCore.Server.Storages.Base.Audit.EF;
using JMCore.Server.Storages.Base.Audit.UserProvider;
using JMCore.Server.Storages.Modules;
using JMCore.Server.Storages.Modules.AuditModule;
using JMCore.Server.Storages.Modules.AuditModule.EF;
using JMCore.Tests.ServerT.StoragesT.Impl.MemoryStorage;
using JMCore.Tests.ServerT.StoragesT.Impl.TestStorageModule;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.ServerT.StoragesT.AuditStorageT;

public class AuditStorageBaseT : DbBaseT
{
  protected IAuditStorageModule AuditStorageModule = null!;
  protected AuditStorageEfContext AuditEfStorageImpl = null!;

  protected ITestStorageModule TestStorageModule = null!;
  protected TestStorageEfContext TestStorageEfContext = null!;

  protected IAuditUserProvider UserProvider = null!;

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    StorResolver.RegisterStorage(sc, new MemoryStorageConfiguration("memory", StorageNativeModuleTypeEnum.BasicModule | StorageNativeModuleTypeEnum.AuditModule | StorageNativeModuleTypeEnum.UnitTestModule));
    sc.AddSingleton<IAuditUserProvider>(TestAuditUserProvider.CreateDefaultUser());
    sc.AddScoped<IAuditDbService, AuditDbService>();
  }

  protected override async Task GetServicesAsync(IServiceProvider sp)
  {
    await base.GetServicesAsync(sp);
    AuditStorageModule = StorResolver.StorageModuleImplementation<IAuditStorageModule>(StorageTypeEnum.Memory); //sp.GetService<LocalizeStorageModule>() ?? throw new ArgumentException($"{nameof(LocalizeStorageModule)} is null.");
    AuditEfStorageImpl = (AuditStorageModule as AuditStorageEfContext) ?? throw new ArgumentException();

    TestStorageModule = StorResolver.StorageModuleImplementation<ITestStorageModule>(StorageTypeEnum.Memory); //sp.GetService<LocalizeStorageModule>() ?? throw new ArgumentException($"{nameof(LocalizeStorageModule)} is null.");
    TestStorageEfContext = (TestStorageModule as TestStorageEfContext) ?? throw new ArgumentException();

    UserProvider = sp.GetService<IAuditUserProvider>() ?? throw new ArgumentException($"{nameof(IAuditUserProvider)} is null.");
  }
}