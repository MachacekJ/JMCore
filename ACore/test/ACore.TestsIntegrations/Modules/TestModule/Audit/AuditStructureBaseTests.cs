using ACore.AppTest;
using ACore.AppTest.Modules.TestModule.Storages;
using ACore.AppTest.Modules.TestModule.Storages.EF.PG;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.AuditModule.EF;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Modules.AuditModule.UserProvider;
using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Storages.Models;
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
    AppTestModulesNames.TestModule
  };

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    sc.AddScoped<IAuditConfiguration, AuditConfiguration>();
    sc.AddSingleton<IAuditUserProvider>(TestAuditUserProvider.CreateDefaultUser());
    sc.AddScoped<IAuditDbService, AuditDbService>();
  }
  
  protected static string GetTestTableName(StorageTypeEnum storageType, string entityName)
  {
    return storageType switch
    {
      StorageTypeEnum.Memory => entityName,
      StorageTypeEnum.Mongo => entityName,
      StorageTypeEnum.Postgres => PGTestStorageDbNames.ObjectNameMapping[entityName].TableName,
      _ => throw new Exception($"Register name of table '{Enum.GetName(storageType.GetType(), storageType)}' for '{entityName}'.")
    };
  }

  protected static string GetTestColumnName(StorageTypeEnum storageType, string entityName, string propName)
  {
    return storageType switch
    {
      StorageTypeEnum.Memory => entityName,
      StorageTypeEnum.Mongo => entityName,
      StorageTypeEnum.Postgres => PGTestStorageDbNames.ObjectNameMapping[entityName].ColumnNames[propName],
      _ => throw new Exception($"Register name of table '{Enum.GetName(storageType.GetType(), storageType)}' for '{entityName}'.")
    };
  }
}