using JMCore.Server.Configuration.Storage.Models;
using JMCore.Tests.ServerT.StoragesT.Implementations.TestStorageModule;
using JMCore.TestsIntegrations.ServerT.StoragesT.ModulesT.AuditStorageT;
using JMCore.TestsIntegrations.ServerT.StoragesT.TestStorageImplementations.PG.TestStorageModule;

namespace JMCore.TestsIntegrations.ServerT.StoragesT.ModulesT.TestStorageModuleT.PGT;

public class TestStorageModuleBaseT : AuditStructureBaseT
{
  protected override StorageTypeEnum StorageTypesToTest => StorageTypeEnum.Postgres;
  /// <summary>
  /// <see cref="TestPGEfStorageImpl"/>
  /// </summary>
  protected TestPGEfStorageImpl GetTestStorageImplementation() => (StorageResolver.FirstStorageModuleImplementation<ITestStorageModule>(StorageTypesToTest) as TestPGEfStorageImpl)!;
}