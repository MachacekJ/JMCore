using JMCore.Server.Storages.Models;
using JMCore.Tests.Implementations.Storages.TestModule.Storages;
using JMCore.Tests.Implementations.Storages.TestModule.Storages.Mongo;
using JMCore.Tests.Implementations.Storages.TestModule.Storages.PG.TestStorageModule;
using JMCore.TestsIntegrations.ServerT.StoragesT.ModulesT.AuditStorageT;

namespace JMCore.TestsIntegrations.ServerT.StoragesT.ModulesT.TestStorageModuleT.MongoT;

public class TestStorageModuleBaseT : AuditStructureBaseT
{
  protected override StorageTypeEnum StorageTypesToTest => StorageTypeEnum.Mongo;
  /// <summary>
  /// <see cref="TestPGEfStorageImpl"/>
  /// </summary>
  protected TestMongoStorageImpl GetTestStorageImplementation() => (StorageResolver.FirstStorageModuleImplementation<ITestStorageModule>(StorageTypesToTest) as TestMongoStorageImpl)!;
}