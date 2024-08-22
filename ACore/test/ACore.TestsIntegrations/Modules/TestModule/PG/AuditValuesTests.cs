using System.Reflection;
using ACore.AppTest.Modules.TestModule.CQRS.TestValueType;
using ACore.AppTest.Modules.TestModule.Models;
using ACore.Tests.Server.Modules.TestModule;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ACore.TestsIntegrations.Modules.TestModule.PG;

public class AuditValuesTests : PGAuditTestBase
{
  [Fact]
  public async Task AllTypes()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
    {
      await AuditValuesTHelper.AllTypes(Mediator, LogInMemorySink, name => GetTestTableName(storageType, name), (name, propName) => GetTestColumnName(storageType, name, propName));
    });
  }
  
  [Fact]
  public async Task StringDbSize()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async _ =>
    {
      // Arrange
      var item = new TestValueTypeData
      {
        NVarChar2 = "123456789012",
      };

      // Act.
      await Assert.ThrowsAsync<DbUpdateException>(async () => { await Mediator.Send(new TestValueTypeSaveCommand(item)); });
    });
  }
}