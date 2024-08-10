using System.Reflection;
using ACore.Tests.Server.Modules.TestModule;
using Xunit;

namespace ACore.TestsIntegrations.Modules.TestModule.PG;

public class AuditAttributeTests : PGAuditTestBase
{
  [Fact]
  public async Task NoAudit()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) => { await AuditAttributeTHelper.NoAuditAsyncTest(Mediator, (name) => GetTestTableName(storageType, name)); });
  }

  [Fact]
  public async Task AddItem()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
    {
      await AuditAttributeTHelper.AddItemAsyncTest(Mediator, (name) => GetTestTableName(storageType, name), (name, prop) => GetTestColumnName(storageType, name, prop));
    });
  }

  [Fact]
  public async Task UpdateItem()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) => { await AuditAttributeTHelper.UpdateItemAsyncTest(Mediator, (name) => GetTestTableName(storageType, name), (name, prop) => GetTestColumnName(storageType, name, prop)); });
  }

  [Fact]
  public async Task DeleteItem()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) => { await AuditAttributeTHelper.DeleteItemTest(Mediator, (name) => GetTestTableName(storageType, name), (name, prop) => GetTestColumnName(storageType, name, prop)); });
  }
}