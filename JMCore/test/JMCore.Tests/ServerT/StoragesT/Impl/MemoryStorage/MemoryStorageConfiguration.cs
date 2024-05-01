using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Modules;
using JMCore.Server.Storages.Modules.AuditModule;
using JMCore.Server.Storages.Modules.BasicModule;
using JMCore.Server.Storages.Modules.LocalizeModule;
using JMCore.Tests.ServerT.StoragesT.Impl.MemoryStorage.Modules;
using JMCore.Tests.ServerT.StoragesT.Impl.TestStorageModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.ServerT.StoragesT.Impl.MemoryStorage;

public class MemoryStorageConfiguration(string connectionString, StorageNativeModuleTypeEnum registerCoreNativeModules) : StorageConfigurationItem(registerCoreNativeModules)
{
  public override StorageTypeEnum StorageType => StorageTypeEnum.Memory;

  public override void RegisterServices(IServiceCollection services)
  {
    if (RegisterCoreNativeModules.HasFlag(StorageNativeModuleTypeEnum.BasicModule))
    {
      services.AddDbContext<BasicEfStorageImpl>(opt => opt.UseInMemoryDatabase(connectionString + nameof(IBasicStorageModule) + Guid.NewGuid()));
      services.AddSingleton<IBasicStorageModule, BasicEfStorageImpl>();
    }

    if (RegisterCoreNativeModules.HasFlag(StorageNativeModuleTypeEnum.AuditModule))
    {
      services.AddDbContext<AuditEfStorageImpl>(opt => opt.UseInMemoryDatabase(connectionString + nameof(IAuditStorageModule) + Guid.NewGuid()));
      services.AddSingleton<IAuditStorageModule, AuditEfStorageImpl>();
    }


    if (RegisterCoreNativeModules.HasFlag(StorageNativeModuleTypeEnum.LocalizeModule))
    {
      services.AddDbContext<LocalizationEfStorageImpl>(opt => opt.UseInMemoryDatabase(connectionString + nameof(ILocalizeStorageModule) + Guid.NewGuid()));
      services.AddSingleton<ILocalizeStorageModule, LocalizationEfStorageImpl>();
    }

    if (RegisterCoreNativeModules.HasFlag(StorageNativeModuleTypeEnum.UnitTestModule))
    {
      services.AddDbContext<TestStorageEfStorageImpl>(opt => opt.UseInMemoryDatabase(connectionString + nameof(ILocalizeStorageModule) + Guid.NewGuid()));
      services.AddSingleton<ITestStorageModule, TestStorageEfStorageImpl>();
    }
  }

  public override async Task ConfigureServices(IServiceProvider serviceProvider)
  {
    if (RegisterCoreNativeModules.HasFlag(StorageNativeModuleTypeEnum.BasicModule))
      await ConfigureEfSqlServiceLocal<IBasicStorageModule, BasicEfStorageImpl>(serviceProvider);
    if (RegisterCoreNativeModules.HasFlag(StorageNativeModuleTypeEnum.AuditModule))
      await ConfigureEfSqlServiceLocal<IAuditStorageModule, AuditEfStorageImpl>(serviceProvider);
    if (RegisterCoreNativeModules.HasFlag(StorageNativeModuleTypeEnum.LocalizeModule))
      await ConfigureEfSqlServiceLocal<ILocalizeStorageModule, LocalizationEfStorageImpl>(serviceProvider);
    if (RegisterCoreNativeModules.HasFlag(StorageNativeModuleTypeEnum.UnitTestModule))
      await ConfigureEfSqlServiceLocal<ITestStorageModule, TestStorageEfStorageImpl>(serviceProvider);
  }
}