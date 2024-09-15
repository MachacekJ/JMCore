using System.Reflection;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.AuditModule.CQRS.Audit.AuditGet;
using ACore.Server.Modules.AuditModule.CQRS.Audit.AuditGet.Models;
using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Storages.CQRS;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.Test.Get;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.Test.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.Test.Save;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAttributeAudit.Delete;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAttributeAudit.Get;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAttributeAudit.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAttributeAudit.Save;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using FluentAssertions;
using MediatR;
using Xunit;

// ReSharper disable NullableWarningSuppressionIsUsed

namespace ACore.Tests.Server.Modules.AuditModule;

/// <summary>
/// Test audit items when entity class contains audit attributes.
/// Sample -> <see cref="TestAttributeAuditPKIntEntity"/> and <see cref="AuditableAttribute"/>
/// </summary>
public class AuditAttribute : AuditAttributeBase
{
  [Fact]
  public async Task NoAuditTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditAttributeTHelper.NoAuditAsyncTest(Mediator, GetTableName); });
  }


  [Fact]
  public async Task AddItemTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditAttributeTHelper.AddItemAsyncTest(Mediator, GetTableName, GetColumnName); });
  }

  [Fact]
  public async Task UpdateItemTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditAttributeTHelper.UpdateItemAsyncTest(Mediator, GetTableName, GetColumnName); });
  }

  [Fact]
  public async Task DeleteItemTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditAttributeTHelper.DeleteItemTest(Mediator, GetTableName, GetColumnName); });
  }
}

public static class AuditAttributeTHelper
{
  public static async Task NoAuditAsyncTest(IMediator mediator, Func<string, string> getTableName)
  {
    var testDateTime = DateTime.UtcNow;
    const string testName = "AuditTest";
    string entityName = nameof(TestEntity);

    var item = new TestData(testName)
    {
      Created = testDateTime,
    };

    var res = (await mediator.Send(new TestSaveCommand(item))) as DbSaveResult;
    res.Should().NotBeNull();
    res?.PKValues.Should().HaveCountGreaterThan(0);
    var fv = res?.PKValues.First();
    fv?.Value.Should().NotBeNull();

    // Assert.
    var allTestData = (await mediator.Send(new TestGetQuery())).ResultValue;
    var savedItem = allTestData as TestData[] ?? allTestData.ToArray();
    savedItem.Should().NotBeNull();
    savedItem.Length.Should().Be(1);

    var resAuditItems = (await mediator.Send(new AuditGetQuery<int>(getTableName(entityName), savedItem.First().Id))).ResultValue;
    resAuditItems.Should().HaveCount(0);
  }

  public static async Task AddItemAsyncTest(IMediator mediator, Func<string, string> getTableName, Func<string, string, string> getColumnName)
  {
    var testDateTime = DateTime.UtcNow;
    var testName = "AuditTest";
    var entityName = nameof(TestAttributeAuditPKIntEntity);

    // Action.
    var item = new TestAttributeAuditData<int>
    {
      Created = testDateTime,
      Name = testName,
      NotAuditableColumn = "Audit"
    };

    var result = await mediator.Send(new TestAttributeAuditSaveCommand<int>(item)) as DbSaveResult;
    result?.PKValues.Should().HaveCountGreaterThan(0);

    // Assert.
    var allData = (await mediator.Send(new TestAttributeAuditGetQuery<int>())).ResultValue;
    ArgumentNullException.ThrowIfNull(allData);
    allData.Should().HaveCount(1);

    var savedItem = allData.Single();
    var resAuditItems = (await mediator.Send(new AuditGetQuery<long>(getTableName(entityName), savedItem.Id))).ResultValue;
    ArgumentNullException.ThrowIfNull(resAuditItems);
    resAuditItems.Should().HaveCount(1);
    resAuditItems.Single().EntityState.Should().Be(AuditStateEnum.Added);

    var auditItem = resAuditItems.Single();
    auditItem.Columns.Should().HaveCount(3);

    var aid = auditItem.GetColumn(getColumnName(entityName, nameof(TestAttributeAuditData<int>.Id)));
    var aName = auditItem.GetColumn(getColumnName(entityName, nameof(TestAttributeAuditData<int>.Name)));
    var aCreated = auditItem.GetColumn(getColumnName(entityName, nameof(TestAttributeAuditData<int>.Created)));

    aid.Should().NotBeNull();
    aName.Should().NotBeNull();
    aName!.NewValue.Should().NotBeNull();
    aCreated.Should().NotBeNull();
    aCreated!.NewValue.Should().NotBeNull();

    aid!.NewValue.Should().Be(savedItem.Id);
    aName.NewValue.Should().Be(testName);
    new DateTime(Convert.ToInt64(aCreated.NewValue)).Should().Be(testDateTime);
  }

  public static async Task UpdateItemAsyncTest(IMediator mediator, Func<string, string> getTableName, Func<string, string, string> getColumnName)
  {
    var testDateTime = DateTime.UtcNow;
    var testNameOld = "AuditTest";
    var testNameNew = "AuditTestNew";
    var entityName = nameof(TestAttributeAuditPKIntEntity);

    // Action.
    var item = new TestAttributeAuditData<int>()
    {
      Created = testDateTime,
      Name = testNameOld,
      NotAuditableColumn = "Audit"
    };

    var res = await mediator.Send(new TestAttributeAuditSaveCommand<int>(item)) as DbSaveResult;
    ArgumentNullException.ThrowIfNull(res);
    res.IsSuccess.Should().Be(true);
    var itemId = (int)res.PKValues.First().Value;
    itemId.Should().BeGreaterThan(0);

    item.Id = itemId;
    item.Name = testNameNew;

    await mediator.Send(new TestAttributeAuditSaveCommand<int>(item));
    item.Id.Should().Be(itemId);

    // Assert.
    var allData = (await mediator.Send(new TestAttributeAuditGetQuery<int>())).ResultValue;
    allData.Should().HaveCount(1);

    var savedItem = allData.Single();
    var resAuditItems = (await mediator.Send(new AuditGetQuery<int>(getTableName(entityName), savedItem.Id))).ResultValue;

    resAuditItems.Should().HaveCount(2);
    resAuditItems.Last().EntityState.Should().Be(AuditStateEnum.Modified);


    var auditItem = resAuditItems.Last();
    auditItem.Columns.Should().HaveCount(3);

    var aid = auditItem.GetColumn(getColumnName(entityName, nameof(TestAttributeAuditData<int>.Id)));
    var aName = auditItem.GetColumn(getColumnName(entityName, nameof(TestAttributeAuditData<int>.Name)));
    var aCreated = auditItem.GetColumn(getColumnName(entityName, nameof(TestAttributeAuditData<int>.Created)));

    aid.Should().NotBeNull();
    aName.Should().NotBeNull();
    aName!.OldValue.Should().NotBeNull();
    aName.NewValue.Should().Be(testNameNew);
    aCreated.Should().NotBeNull();
    aCreated!.OldValue.Should().NotBeNull();
    aCreated.NewValue.Should().BeNull();
  }

  public static async Task DeleteItemTest(IMediator mediator, Func<string, string> getTableName, Func<string, string, string> getColumnName)
  {
    var testDateTimeOld = DateTime.UtcNow;
    var testNameOld = "AuditTest";
    var entityName = nameof(TestAttributeAuditPKIntEntity);

    // Arrange.
    var item = new TestAttributeAuditData<int>()
    {
      Created = testDateTimeOld,
      Name = testNameOld,
      NotAuditableColumn = "Audit"
    };

    var res = await mediator.Send(new TestAttributeAuditSaveCommand<int>(item)) as DbSaveResult;
    ArgumentNullException.ThrowIfNull(res);
    var itemId = (int)res.PKValues.First().Value;

    // Action.
    var res2 = await mediator.Send(new TestAttributeAuditDeleteCommand<int>(itemId));

    // Assert.
    res2.IsSuccess.Should().Be(true);

    var allData = (await mediator.Send(new TestAttributeAuditGetQuery<int>())).ResultValue;
    allData.Should().HaveCount(0);

    var resAuditItems = (await mediator.Send(new AuditGetQuery<int>(getTableName(entityName), itemId))).ResultValue;
    ArgumentNullException.ThrowIfNull(resAuditItems);
    resAuditItems.Should().HaveCount(2);
    resAuditItems.Last().EntityState.Should().Be(AuditStateEnum.Deleted);


    var auditItem = resAuditItems.Last();
    auditItem.Columns.Should().HaveCount(3);

    var aid = auditItem.GetColumn(getColumnName(entityName, nameof(TestAttributeAuditData<int>.Id)));
    var aName = auditItem.GetColumn(getColumnName(entityName, nameof(TestAttributeAuditData<int>.Name)));
    var aCreated = auditItem.GetColumn(getColumnName(entityName, nameof(TestAttributeAuditData<int>.Created)));

    aid.Should().NotBeNull();
    aName.Should().NotBeNull();
    aName!.OldValue.Should().NotBeNull();
    aName.NewValue.Should().BeNull();
    aCreated.Should().NotBeNull();
    aCreated!.OldValue.Should().NotBeNull();
    aCreated.NewValue.Should().BeNull();
  }
}