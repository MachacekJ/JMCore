using System.Reflection;
using ACore.AppTest.Modules.TestModule.CQRS.Test;
using ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit;
using ACore.AppTest.Modules.TestModule.Models;
using ACore.AppTest.Modules.TestModule.Storages.EF.Models;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.AuditModule.CQRS.Audit;
using ACore.Server.Modules.AuditModule.CQRS.Models;
using FluentAssertions;
using MediatR;
using Xunit;

namespace ACore.Tests.Server.Modules.TestModule;

/// <summary>
/// Test audit items when entity class contains audit attributes.
/// Sample -> <see cref="TestAttributeAuditEntity"/> and <see cref="AuditableAttribute"/>
/// </summary>
public class AuditAttributeTests : AuditAttributeBaseTests
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
    string entityName = "TestEntity";

    var item = new TestData
    {
      Created = testDateTime,
      Name = testName,
    };

    var res = await mediator.Send(new TestSaveCommand(item));
    res.Should().BeGreaterThan(0);

    // Assert.
    var allTestData = await mediator.Send(new TestGetQuery());
    var savedItem = allTestData as TestData[] ?? allTestData.ToArray();
    savedItem.Should().NotBeNull();
    savedItem.Length.Should().Be(1);

    var resAuditItems = await mediator.Send(new AuditGetQuery(getTableName(entityName), savedItem.First().Id));
    resAuditItems.Should().HaveCount(0);
  }

  public static async Task AddItemAsyncTest(IMediator mediator, Func<string, string> getTableName, Func<string, string, string> getColumnName)
  {
    var testDateTime = DateTime.UtcNow;
    var testName = "AuditTest";
    var entityName = "TestAttributeAuditEntity";

    // Action.
    var item = new TestAttributeAuditData
    {
      Created = testDateTime,
      Name = testName,
      NotAuditableColumn = "Audit"
    };

    var res = await mediator.Send(new TestAttributeAuditSaveCommand(item));
    res.Should().BeGreaterThan(0);

    // Assert.
    var allData = await mediator.Send(new TestAttributeAuditGetQuery());
    allData.Should().HaveCount(1);
    
    var savedItem = allData.Single();
    var resAuditItems = await mediator.Send(new AuditGetQuery(getTableName(entityName), savedItem.Id));
    resAuditItems.Should().HaveCount(1);
    resAuditItems.Single().EntityState.Should().Be(AuditStateEnum.Added);
    
    var auditItem = resAuditItems.Single();
    auditItem.Columns.Should().HaveCount(3);

    var aid = auditItem.GetColumn(getColumnName(entityName, nameof(TestAttributeAuditData.Id)));
    var aName = auditItem.GetColumn(getColumnName(entityName, nameof(TestAttributeAuditData.Name)));
    var aCreated = auditItem.GetColumn(getColumnName(entityName, nameof(TestAttributeAuditData.Created)));

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
    var entityName = "TestAttributeAuditEntity";

    // Action.
    var item = new TestAttributeAuditData()
    {
      Created = testDateTime,
      Name = testNameOld,
      NotAuditableColumn = "Audit"
    };

    var res = await mediator.Send(new TestAttributeAuditSaveCommand(item));
    res.Should().BeGreaterThan(0);

    item.Id = res;
    item.Name = testNameNew;

    var res2 = await mediator.Send(new TestAttributeAuditSaveCommand(item));
    res2.Should().Be(res);

    // Assert.
    var allData = await mediator.Send(new TestAttributeAuditGetQuery());
    allData.Should().HaveCount(1);
    
    var savedItem = allData.Single();
    var resAuditItems = await mediator.Send(new AuditGetQuery(getTableName(entityName), savedItem.Id));
    
    resAuditItems.Should().HaveCount(2);
    resAuditItems.Last().EntityState.Should().Be(AuditStateEnum.Modified);
    
    
    var auditItem = resAuditItems.Last();
    auditItem.Columns.Should().HaveCount(1);
    
    var aName = auditItem.GetColumn(getColumnName(entityName, nameof(TestAttributeAuditData.Name)));
    aName.Should().NotBeNull();
    aName!.NewValue.Should().NotBeNull();
    aName.OldValue.Should().NotBeNull();
    aName.OldValue.Should().Be(testNameOld);
    aName.NewValue.Should().Be(testNameNew);

  }

  public static async Task DeleteItemTest(IMediator mediator, Func<string, string> getTableName, Func<string, string, string> getColumnName)
  {
    var testDateTimeOld = DateTime.UtcNow;
    var testNameOld = "AuditTest";
    var entityName = "TestAttributeAuditEntity";

    // Action.
    var item = new TestAttributeAuditData()
    {
      Created = testDateTimeOld,
      Name = testNameOld,
      NotAuditableColumn = "Audit"
    };

    var res = await mediator.Send(new TestAttributeAuditSaveCommand(item));
    res.Should().BeGreaterThan(0);

    item.Id = res;
    
    var res2 = await mediator.Send(new TestAttributeAuditDeleteCommand(item));
    res2.Should().Be(true);

    // Assert.
    var allData = await mediator.Send(new TestAttributeAuditGetQuery());
    allData.Should().HaveCount(0);

    // var allAuditItems = await auditStorageModule.AllTableAuditAsync(getTableName(entityName));
    //
    // Assert.Equal(3, allAuditItems.Count(i => i.EntityState == EntityState.Deleted));
    //
    // var auditValues = await auditStorageModule.AuditItemsAsync(getTableName(entityName), item.Id);
    // var auditVwAuditEntities = auditValues as AuditVwAuditEntity[] ?? auditValues.ToArray();
    // Assert.Equal(6, auditVwAuditEntities.Length);
    // var aid = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(entityName, nameof(TestAttributeAuditData.Id)) && a.EntityState == EntityState.Added);
    // var aName = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(entityName, nameof(TestAttributeAuditData.Name)) && a.EntityState == EntityState.Added);
    // var aCreated = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(entityName, nameof(TestAttributeAuditData.Created)) && a.EntityState == EntityState.Added);
    //
    // Assert.NotNull(aid);
    // Assert.NotNull(aName);
    // Assert.NotNull(aCreated);
    // Assert.NotNull(aCreated.NewValueLong);
    //
    // Assert.Equal(aid.NewValueInt, item.Id);
    // Assert.Equal(aName.NewValueString, testNameOld);
    // Assert.Equal(new DateTime(aCreated.NewValueLong.Value), testDateTimeOld);
    //
    // var aidDeleted = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(entityName, nameof(TestAttributeAuditData.Id)) && a.EntityState == EntityState.Deleted);
    // var aNameDeleted = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(entityName, nameof(TestAttributeAuditData.Name)) && a.EntityState == EntityState.Deleted);
    // var aCreatedDeleted = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(entityName, nameof(TestAttributeAuditData.Created)) && a.EntityState == EntityState.Deleted);
    //
    // Assert.NotNull(aNameDeleted);
    // Assert.NotNull(aCreatedDeleted);
    // Assert.NotNull(aidDeleted);
    //
    // Assert.Null(aCreatedDeleted.NewValueLong);
    // Assert.Null(aNameDeleted.NewValueString);
    // Assert.Null(aidDeleted.NewValueInt);
    // Assert.NotNull(aCreatedDeleted.OldValueLong);
    //
    // Assert.Equal(aidDeleted.OldValueInt, item.Id);
    // Assert.Equal(aNameDeleted.OldValueString, testNameOld);
    // Assert.Equal(new DateTime(aCreatedDeleted.OldValueLong.Value), testDateTimeOld);
  }
}