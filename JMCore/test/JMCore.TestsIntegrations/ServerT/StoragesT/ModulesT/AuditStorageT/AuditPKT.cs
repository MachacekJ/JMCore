using System.Reflection;
using JMCore.Tests.ServerT.StoragesT.ModulesT.AuditStorageT;
using Xunit;

namespace JMCore.TestsIntegrations.ServerT.StoragesT.ModulesT.AuditStorageT;

// ReSharper disable once InconsistentNaming
public class AuditPKT : AuditStructureBaseT
{

  [Fact]
  public async Task GuidPK()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
    {
      var auDb = GetAuditStorageModule(storageType);
      var testDb = GetTestStorageModule(storageType);
      await AuditPKTHelper.GuidPK(auDb, testDb,(name) => GetAuditTableName(storageType, name));
    });
  }

  [Fact]
  public async Task StringPK()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
    {
      var auDb = GetAuditStorageModule(storageType);
      var testDb = GetTestStorageModule(storageType);
      await AuditPKTHelper.StringPK(auDb, testDb, (name) => GetAuditTableName(storageType, name));
    });
  }
}