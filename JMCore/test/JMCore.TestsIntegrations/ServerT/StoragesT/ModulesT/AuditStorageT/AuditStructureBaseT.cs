using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Base.Audit.Configuration;
using JMCore.Server.Storages.Base.Audit.EF;
using JMCore.Server.Storages.Base.Audit.UserProvider;
using JMCore.Server.Storages.Modules.AuditModule;
using JMCore.Server.Storages.Modules.BasicModule;
using JMCore.Tests.ServerT.StoragesT.Implementations.TestStorageModule;
using JMCore.Tests.ServerT.StoragesT.Implementations.TestStorageModule.Models;
using JMCore.Tests.ServerT.StoragesT.ModulesT.AuditStorageT;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.TestsIntegrations.ServerT.StoragesT.ModulesT.AuditStorageT;

public class AuditStructureBaseT : StorageBaseT
{
  protected static StorageTypeEnum StorageTypesToTest => StorageTypeEnum.Postgres;

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

  protected IAuditStorageModule GetAuditStorageModule(StorageTypeEnum storageType) => StorageResolver.FirstStorageModuleImplementation<IAuditStorageModule>(storageType);
  protected ITestStorageModule GetTestStorageModule(StorageTypeEnum storageType) => StorageResolver.FirstStorageModuleImplementation<ITestStorageModule>(storageType);

  protected string GetAuditTableName(StorageTypeEnum storageType, string entityName)
  {
    return storageType switch
    {
      StorageTypeEnum.Memory => entityName,
      StorageTypeEnum.Mongo => entityName,
      StorageTypeEnum.Postgres => entityName switch
      {
        nameof(TestEntity) => "test",
        nameof(TestAttributeAuditEntity) => "test_attribute_audit",
        nameof(TestManualAuditEntity) => "test_manual_audit",
        nameof(TestValueTypeEntity) => "test_value_type",
        nameof(TestPKGuidEntity) => "test_pk_guid",
        nameof(TestPKStringEntity) => "test_pk_string",
        _ => throw new Exception($"Register name of table '{Enum.GetName(storageType.GetType(), storageType)}' for '{entityName}'.")
      },
      _ => throw new Exception($"Register name of table '{Enum.GetName(storageType.GetType(), storageType)}' for '{entityName}'.")
    };
  }
}