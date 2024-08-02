using System.Reflection;
using ACore.Tests.Server.Modules.TestModule;
using Xunit;

namespace ACore.TestsIntegrations.Modules.TestModule.Audit;

public class AuditAttributeTests : AuditStructureBaseTests
{
  [Fact]
  public async Task NoAudit()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
    {
      var auDb = GetAuditStorageModule(storageType);
      await AuditAttributeTHelper.NoAuditAsyncTest(auDb, Mediator, (name) => GetTestTableName(storageType, name));
    });
  }
  [Fact]
  public async Task AddItem()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
    {
      var auDb = GetAuditStorageModule(storageType);
      await AuditAttributeTHelper.AddItemAsyncTest(auDb, Mediator, (name) => GetTestTableName(storageType, name), (name, prop) => GetTestColumnName(storageType, name, prop));
    });
  }

  [Fact]
  public async Task UpdateItem()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
    {
      var auDb = GetAuditStorageModule(storageType);
      await AuditAttributeTHelper.UpdateItemAsyncTest(auDb, Mediator, (name) => GetTestTableName(storageType, name), (name, prop) => GetTestColumnName(storageType, name, prop));
    });
  }
  
  [Fact]
  public async Task DeleteItem()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
    {
      var auDb = GetAuditStorageModule(storageType);
      await AuditAttributeTHelper.DeleteItemTest(auDb, Mediator, (name) => GetTestTableName(storageType, name), (name, prop) => GetTestColumnName(storageType, name, prop));
    });
  }
}