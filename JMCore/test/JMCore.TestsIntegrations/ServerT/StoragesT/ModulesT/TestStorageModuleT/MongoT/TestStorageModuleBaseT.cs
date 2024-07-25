using JMCore.Server.Configuration.Storage.Models;
using JMCore.Tests.ServerT.StoragesT.Implementations.TestStorageModule;
using JMCore.TestsIntegrations.ServerT.StoragesT.ModulesT.AuditStorageT;
using JMCore.TestsIntegrations.ServerT.StoragesT.TestStorageImplementations.Mongo.TestStorageModule;
using JMCore.TestsIntegrations.ServerT.StoragesT.TestStorageImplementations.PG.TestStorageModule;

namespace JMCore.TestsIntegrations.ServerT.StoragesT.ModulesT.TestStorageModuleT.MongoT;

public class TestStorageModuleBaseT : AuditStructureBaseT
{
  protected override StorageTypeEnum StorageTypesToTest => StorageTypeEnum.Mongo;
  /// <summary>
  /// <see cref="TestPGEfStorageImpl"/>
  /// </summary>
  protected TestMongoStorageImpl GetTestStorageImplementation() => (StorageResolver.FirstStorageModuleImplementation<ITestStorageModule>(StorageTypesToTest) as TestMongoStorageImpl)!;
}