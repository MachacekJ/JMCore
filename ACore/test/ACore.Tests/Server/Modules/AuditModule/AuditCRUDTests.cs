using System.Reflection;
using ACore.Tests.Server.Modules.AuditModule.Helpers;
using Xunit;

// ReSharper disable NullableWarningSuppressionIsUsed

namespace ACore.Tests.Server.Modules.AuditModule;

/// <summary>
/// Test audit items when entity class contains audit attributes.
/// </summary>
public class AuditTests : AuditTestsBase
{
  [Fact]
  public async Task NoAuditTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditCRUDHelper.NoAuditAsyncTest(Mediator, GetTableName); });
  }


  [Fact]
  public async Task AddItemTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditCRUDHelper.AddItemAsyncTest(Mediator, GetTableName, GetColumnName); });
  }

  [Fact]
  public async Task UpdateItemTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditCRUDHelper.UpdateItemAsyncTest(Mediator, GetTableName, GetColumnName); });
  }

  [Fact]
  public async Task DeleteItemTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditCRUDHelper.DeleteItemTest(Mediator, GetTableName, GetColumnName); });
  }
}