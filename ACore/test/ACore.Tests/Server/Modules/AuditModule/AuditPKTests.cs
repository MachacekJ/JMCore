using System.Reflection;
using ACore.Server.Modules.AuditModule.CQRS.AuditGet;
using ACore.Server.Modules.AuditModule.CQRS.AuditGet.Models;
using ACore.Server.Storages.CQRS;
using ACore.Tests.Server.Modules.AuditModule.Helpers;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Get;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Save;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Get;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Save;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using FluentAssertions;
using MediatR;
using Xunit;

namespace ACore.Tests.Server.Modules.AuditModule;

/// <summary>
/// Two kinds of table primary key are supported.
/// ObjectId for mongoDb is tested in integration tests. 
/// </summary>
// ReSharper disable once InconsistentNaming
public class AuditPKTests : AuditTestsBase
{
  [Fact]
  public async Task IntPK()
  {
  }

  [Fact]
  public async Task LongPK()
  {
  }

  [Fact]
  public async Task GuidPK()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditPKTHelper.GuidPK(Mediator, GetTableName); });
  }

  [Fact]
  public async Task StringPK()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditPKTHelper.StringPK(Mediator, GetTableName); });
  }
}

// ReSharper disable once InconsistentNaming
public static class AuditPKTHelper
{
  private const string TestName = "AuditPK";
  private const string TestPKGuidEntityName = nameof(TestPKGuidEntity);
  private const string TestPKStringEntityName = nameof(TestPKStringEntity);

  public static async Task GuidPK(IMediator mediator, Func<string, string> getTableName)
  {
    // Arrange.
    var item = new TestPKGuidData
    {
      Name = TestName,
    };

    // Act.
    var saveResult = await mediator.Send(new TestPKGuidSaveCommand(item)) as DbSaveResult;

    // Assert.
    ArgumentNullException.ThrowIfNull(saveResult);
    var idItem = saveResult.PrimaryKeySingle<Guid>();

    var allData = (await mediator.Send(new TestPKGuidGetQuery())).ResultValue;
    allData.Should().HaveCount(1);

    var resAuditItems = (await mediator.Send(new AuditGetQuery<TestPKGuidEntity, Guid>(getTableName(TestPKGuidEntityName), idItem))).ResultValue;
    ArgumentNullException.ThrowIfNull(resAuditItems);
    resAuditItems.Length.Should().Be(2);
  }

  public static async Task StringPK(IMediator mediator, Func<string, string> getTableName)
  {
    var entityName = nameof(TestPKStringEntity);

    // Arrange.
    var item = new TestPKStringData()
    {
      Name = TestName,
    };

    // Action 1
    var id = await mediator.Send(new TestPKStringSaveCommand(item)) as DbSaveResult;
    var allData = (await mediator.Send(new TestPKStringGetQuery())).ResultValue;
    var itemId = AuditAssertHelpers.AssertSinglePrimaryKeyWithResult<TestPKStringData, string>(id, allData);

    var resAuditItems = (await mediator.Send(new AuditGetQuery<TestPKStringEntity, string>(getTableName(entityName), itemId))).ResultValue;

    var auditVwAuditEntities = resAuditItems ?? throw new Exception("Should not be null.");
    auditVwAuditEntities.Count().Should().Be(2);
    //auditVwAuditEntities.Single(a => a.NewValueString == item.Id).NewValueString.Should().Be(item.Id);
    //
    // // Arrange 2
    // item.Name = TestName + "2";
    //
    // // Action 2 
    // var res2 = await mediator.Send(new TestPKStringSaveCommand(item)); 
    // res2.Should().Be(true);
    //
    // // Assert 2
    // auditValues = await auditStorageModule.AuditItemsAsync(getTableName(TestPKStringEntityName), item.Id);
    // auditValues.Count(a => a.PKValueString == item.Id).Should().Be(3);
  }
}