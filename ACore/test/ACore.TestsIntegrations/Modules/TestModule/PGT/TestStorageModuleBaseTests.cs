using ACore.AppTest.Modules.TestModule.Storages;
using ACore.AppTest.Modules.TestModule.Storages.PG;
using ACore.Server.Storages.Models;
using ACore.TestsIntegrations.Modules.TestModule.Audit;

namespace ACore.TestsIntegrations.Modules.TestModule.PGT;

public class TestStorageModuleBaseTests : AuditStructureBaseTests
{
  protected override StorageTypeEnum StorageTypesToTest => StorageTypeEnum.Postgres;
  /// <summary>
  /// <see cref="TestPGEfStorageImpl"/>
  /// </summary>
  //protected TestPGEfStorageImpl GetTestStorageImplementation() => (StorageResolver.FirstReadWriteStorage<ITestStorageModule>(StorageTypesToTest) as TestPGEfStorageImpl)!;
}