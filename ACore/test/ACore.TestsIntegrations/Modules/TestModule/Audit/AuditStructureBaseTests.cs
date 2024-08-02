using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.AuditModule.EF;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Modules.AuditModule.UserProvider;
using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Storages.Models;
using ACore.Tests.Implementations.Modules.TestModule.Storages;
using ACore.Tests.Implementations.Modules.TestModule.Storages.PG;
using ACore.Tests.Server.Modules.TestModule;
using ACore.TestsIntegrations.BaseInfrastructure.Storages;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.TestsIntegrations.Modules.TestModule.Audit;

public class AuditStructureBaseTests : StorageBaseTests
{
  // Virtual is needed for stress tests.
  protected virtual StorageTypeEnum StorageTypesToTest => StorageTypeEnum.Postgres;

  protected override IEnumerable<string> RequiredBaseStorageModules => new[]
  {
    nameof(IBasicStorageModule),
    nameof(IAuditStorageModule),
    nameof(ITestStorageModule)
  };

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    sc.AddScoped<IAuditConfiguration, AuditConfiguration>();
    sc.AddSingleton<IAuditUserProvider>(TestAuditUserProvider.CreateDefaultUser());
    sc.AddScoped<IAuditDbService, AuditDbService>();
  }

  protected IAuditStorageModule GetAuditStorageModule(StorageTypeEnum storageType) => StorageResolver.FirstReadWriteStorage<IAuditStorageModule>(storageType);
  protected ITestStorageModule GetTestStorageModule(StorageTypeEnum storageType) => StorageResolver.FirstReadWriteStorage<ITestStorageModule>(storageType);

  protected static string GetTestTableName(StorageTypeEnum storageType, Type entityName)
  {
    return storageType switch
    {
      StorageTypeEnum.Memory => entityName.Name,
      StorageTypeEnum.Mongo => entityName.Name,
      StorageTypeEnum.Postgres => TestPGEfDbNames.ObjectNameMapping[entityName].TableName,
      _ => throw new Exception($"Register name of table '{Enum.GetName(storageType.GetType(), storageType)}' for '{entityName}'.")
    };
  }

  protected static string GetTestColumnName(StorageTypeEnum storageType, Type entityName, string propName)
  {
    return storageType switch
    {
      StorageTypeEnum.Memory => entityName.Name,
      StorageTypeEnum.Mongo => entityName.Name,
      StorageTypeEnum.Postgres => TestPGEfDbNames.ObjectNameMapping[entityName].ColumnNames[propName],
      _ => throw new Exception($"Register name of table '{Enum.GetName(storageType.GetType(), storageType)}' for '{entityName}'.")
    };
  }
}