using ACore.AppTest.Modules.TestModule.Storages;
using ACore.AppTest.Modules.TestModule.Storages.Mongo;
using ACore.AppTest.Modules.TestModule.Storages.PG;
using ACore.Server.Storages.Models;
using ACore.TestsIntegrations.Modules.TestModule.Audit;

namespace ACore.TestsIntegrations.Modules.TestModule.MongoT;

public class TestStorageModuleBaseTests : AuditStructureBaseTests
{
  protected override StorageTypeEnum StorageTypesToTest => StorageTypeEnum.Mongo;
  /// <summary>
  /// <see cref="TestPGEfStorageImpl"/>
  /// </summary>
  //protected TestMongoStorageImpl GetTestStorageImplementation() => (StorageResolver.FirstReadWriteStorage<ITestStorageModule>(StorageTypesToTest) as TestMongoStorageImpl)!;
}