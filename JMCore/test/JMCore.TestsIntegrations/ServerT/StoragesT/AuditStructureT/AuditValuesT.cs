﻿using System.Reflection;
using JMCore.Tests.ServerT.StoragesT.AuditStorageT;
using Xunit;

namespace JMCore.TestsIntegrations.ServerT.StoragesT.AuditStructureT;

public class AuditValuesT : AuditStructureBaseT
{
  [Fact]
  public async Task AllTypes()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
    {
      var auDb = GetAuditStorageModule(storageType);
      var testDb = GetTestStorageModule(storageType);
      await AuditValuesTHelper.AllTypes(auDb, testDb, LogInMemorySink, (name) => GetAuditTableName(storageType, name));
    });
  }
}