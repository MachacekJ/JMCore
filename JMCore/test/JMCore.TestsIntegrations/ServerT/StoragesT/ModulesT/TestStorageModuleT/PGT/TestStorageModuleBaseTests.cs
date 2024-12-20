﻿using JMCore.Server.Storages.Models;
using JMCore.Tests.Implementations.Modules.TestModule.Storages;
using JMCore.Tests.Implementations.Modules.TestModule.Storages.PG;
using JMCore.TestsIntegrations.ServerT.StoragesT.ModulesT.AuditStorageT;

namespace JMCore.TestsIntegrations.ServerT.StoragesT.ModulesT.TestStorageModuleT.PGT;

public class TestStorageModuleBaseTests : AuditStructureBaseTests
{
  protected override StorageTypeEnum StorageTypesToTest => StorageTypeEnum.Postgres;
  /// <summary>
  /// <see cref="TestPGEfStorageImpl"/>
  /// </summary>
  protected TestPGEfStorageImpl GetTestStorageImplementation() => (StorageResolver.FirstReadWriteStorage<ITestStorageModule>(StorageTypesToTest) as TestPGEfStorageImpl)!;
}