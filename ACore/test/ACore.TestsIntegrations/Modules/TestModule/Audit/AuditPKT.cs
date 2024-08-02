using System.Reflection;
using ACore.Tests.Server.Modules.TestModule;
using Xunit;

namespace ACore.TestsIntegrations.Modules.TestModule.Audit;

// ReSharper disable once InconsistentNaming
public class AuditPKT : AuditStructureBaseTests
{

  [Fact]
  public async Task GuidPK()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
    {
      var auDb = GetAuditStorageModule(storageType);
      await AuditPKTHelper.GuidPK(auDb, Mediator,(name) => GetTestTableName(storageType, name));
    });
  }

  [Fact]
  public async Task StringPK()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
    {
      var auDb = GetAuditStorageModule(storageType);
      await AuditPKTHelper.StringPK(auDb, Mediator, (name) => GetTestTableName(storageType, name));
    });
  }
}