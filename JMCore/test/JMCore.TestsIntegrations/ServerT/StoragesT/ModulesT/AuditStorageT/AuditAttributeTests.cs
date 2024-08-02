using System.Reflection;
using JMCore.Tests.Server.Modules.TestModule;
using Xunit;

namespace JMCore.TestsIntegrations.ServerT.StoragesT.ModulesT.AuditStorageT;

public class AuditAttributeTests : AuditStructureBaseTests
{
  [Fact]
  public async Task NoAudit()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
    {
      var auDb = GetAuditStorageModule(storageType);
      var testDb = GetTestStorageModule(storageType);
      await AuditAttributeTHelper.NoAuditAsyncTest(auDb, testDb, (name) => GetTestTableName(storageType, name));
    });
  }
  [Fact]
  public async Task AddItem()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
    {
      var auDb = GetAuditStorageModule(storageType);
      var testDb = GetTestStorageModule(storageType);
      await AuditAttributeTHelper.AddItemAsyncTest(auDb, testDb, (name) => GetTestTableName(storageType, name), (name, prop) => GetTestColumnName(storageType, name, prop));
    });
  }

  [Fact]
  public async Task UpdateItem()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
    {
      var auDb = GetAuditStorageModule(storageType);
      var testDb = GetTestStorageModule(storageType);
      await AuditAttributeTHelper.UpdateItemAsyncTest(auDb, testDb, (name) => GetTestTableName(storageType, name), (name, prop) => GetTestColumnName(storageType, name, prop));
    });
  }
  
  [Fact]
  public async Task DeleteItem()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
    {
      var auDb = GetAuditStorageModule(storageType);
      var testDb = GetTestStorageModule(storageType);
      await AuditAttributeTHelper.DeleteItemTest(auDb, testDb, (name) => GetTestTableName(storageType, name), (name, prop) => GetTestColumnName(storageType, name, prop));
    });
  }
}