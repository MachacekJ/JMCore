﻿using System.Reflection;
using ACore.Tests.Server.Modules.TestModule;
using Xunit;

namespace ACore.TestsIntegrations.Modules.TestModule.Audit;

public class AuditValuesTests : AuditStructureBaseTests
{
  [Fact]
  public async Task AllTypes()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
    {
      var auDb = GetAuditStorageModule(storageType);
      await AuditValuesTHelper.AllTypes(auDb, Mediator, LogInMemorySink, (name) => GetTestTableName(storageType, name), (name, propName) => GetTestColumnName(storageType, name, propName));
    });
  }
}