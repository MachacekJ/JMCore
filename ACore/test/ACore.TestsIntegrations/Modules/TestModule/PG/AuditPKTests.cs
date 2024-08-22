using System.Reflection;
using ACore.Tests.Server.Modules.TestModule;
using Xunit;

namespace ACore.TestsIntegrations.Modules.TestModule.PG;

// ReSharper disable once InconsistentNaming
public class AuditPKTests : PGAuditTestBase
{

  [Fact]
  public async Task GuidPK()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
    {
      await AuditPKTHelper.GuidPK(Mediator,(name) => GetTestTableName(storageType, name));
    });
  }

  [Fact]
  public async Task StringPK()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
    {
      await AuditPKTHelper.StringPK(Mediator, (name) => GetTestTableName(storageType, name));
    });
  }
}