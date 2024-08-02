﻿using ACore.Server.Storages.Models;
using ACore.Tests.Implementations.Modules.TestModule.Storages;
using ACore.Tests.Implementations.Modules.TestModule.Storages.Mongo;
using ACore.Tests.Implementations.Modules.TestModule.Storages.PG;
using ACore.TestsIntegrations.ServerT.StoragesT.ModulesT.AuditStorageT;

namespace ACore.TestsIntegrations.ServerT.StoragesT.ModulesT.TestStorageModuleT.MongoT;

public class TestStorageModuleBaseTests : AuditStructureBaseTests
{
  protected override StorageTypeEnum StorageTypesToTest => StorageTypeEnum.Mongo;
  /// <summary>
  /// <see cref="TestPGEfStorageImpl"/>
  /// </summary>
  protected TestMongoStorageImpl GetTestStorageImplementation() => (StorageResolver.FirstReadWriteStorage<ITestStorageModule>(StorageTypesToTest) as TestMongoStorageImpl)!;
}