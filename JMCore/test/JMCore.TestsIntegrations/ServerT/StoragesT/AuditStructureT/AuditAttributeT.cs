using System.Reflection;
using JMCore.Tests.ServerT.StoragesT.AuditStorageT;
using Xunit;

namespace JMCore.TestsIntegrations.ServerT.StoragesT.AuditStructureT;

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
}