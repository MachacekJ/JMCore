using JMCore.Server.Storages.Models;
using JMCore.Tests.Implementations.Modules.TestModule.Storages;
using JMCore.Tests.Implementations.Modules.TestModule.Storages.Mongo;
using JMCore.Tests.Implementations.Modules.TestModule.Storages.PG;
using JMCore.TestsIntegrations.ServerT.StoragesT.ModulesT.AuditStorageT;

namespace JMCore.TestsIntegrations.ServerT.StoragesT.ModulesT.TestStorageModuleT.MongoT;

public class TestStorageModuleBaseTests : AuditStructureBaseTests
{
  protected override StorageTypeEnum StorageTypesToTest => StorageTypeEnum.Mongo;
  /// <summary>
  /// <see cref="TestPGEfStorageImpl"/>
  /// </summary>
  protected TestMongoStorageImpl GetTestStorageImplementation() => (StorageResolver.FirstReadWriteStorage<ITestStorageModule>(StorageTypesToTest) as TestMongoStorageImpl)!;
}