using ACore.Server.Storages.Models;
using ACore.Tests.Implementations.Modules.TestModule.Storages;
using ACore.Tests.Implementations.Modules.TestModule.Storages.PG;
using ACore.TestsIntegrations.ServerT.StoragesT.ModulesT.AuditStorageT;

namespace ACore.TestsIntegrations.ServerT.StoragesT.ModulesT.TestStorageModuleT.PGT;

public class TestStorageModuleBaseTests : AuditStructureBaseTests
{
  protected override StorageTypeEnum StorageTypesToTest => StorageTypeEnum.Postgres;
  /// <summary>
  /// <see cref="TestPGEfStorageImpl"/>
  /// </summary>
  protected TestPGEfStorageImpl GetTestStorageImplementation() => (StorageResolver.FirstReadWriteStorage<ITestStorageModule>(StorageTypesToTest) as TestPGEfStorageImpl)!;
}