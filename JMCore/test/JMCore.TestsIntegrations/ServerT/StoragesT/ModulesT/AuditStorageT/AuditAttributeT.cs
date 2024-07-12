using System.Reflection;
using JMCore.Tests.ServerT.StoragesT.ModulesT.AuditStorageT;
using Xunit;

namespace JMCore.TestsIntegrations.ServerT.StoragesT.ModulesT.AuditStorageT;

public class AuditAttributeT : AuditStructureBaseT
{
  [Fact]
  public async Task NoAudit()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
    {
      var auDb = GetAuditStorageModule(storageType);
      var testDb = GetTestStorageModule(storageType);
      await AuditAttributeTHelper.NoAudit(auDb, testDb, (name) => GetAuditTableName(storageType, name));
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
      await AuditAttributeTHelper.AddItem(auDb, testDb, (name) => GetAuditTableName(storageType, name));
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
      await AuditAttributeTHelper.UpdateItem(auDb, testDb, (name) => GetAuditTableName(storageType, name));
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
      await AuditAttributeTHelper.DeleteItem(auDb, testDb, (name) => GetAuditTableName(storageType, name));
    });
  }
}