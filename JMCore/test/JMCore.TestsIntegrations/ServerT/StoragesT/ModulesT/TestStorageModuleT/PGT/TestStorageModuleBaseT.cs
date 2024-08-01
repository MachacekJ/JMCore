using JMCore.Server.Storages.Models;
using JMCore.Tests.Implementations.Storages.TestModule.Storages;
using JMCore.Tests.Implementations.Storages.TestModule.Storages.PG.TestStorageModule;
using JMCore.TestsIntegrations.ServerT.StoragesT.ModulesT.AuditStorageT;

namespace JMCore.TestsIntegrations.ServerT.StoragesT.ModulesT.TestStorageModuleT.PGT;

public class TestStorageModuleBaseT : AuditStructureBaseT
{
  protected override StorageTypeEnum StorageTypesToTest => StorageTypeEnum.Postgres;
  /// <summary>
  /// <see cref="TestPGEfStorageImpl"/>
  /// </summary>
  protected TestPGEfStorageImpl GetTestStorageImplementation() => (StorageResolver.FirstStorageModuleImplementation<ITestStorageModule>(StorageTypesToTest) as TestPGEfStorageImpl)!;
}