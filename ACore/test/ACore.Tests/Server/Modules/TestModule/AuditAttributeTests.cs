using System.Reflection;
using ACore.AppTest.Modules.TestModule.CQRS.Models;
using ACore.AppTest.Modules.TestModule.CQRS.Test;
using ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit;
using ACore.AppTest.Modules.TestModule.Storages;
using ACore.AppTest.Modules.TestModule.Storages.Models;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Modules.AuditModule.Storage.Models;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
    await RunTestAsync(method, async () => { await AuditAttributeTHelper.NoAuditAsyncTest(AuditStorageModule, Mediator, GetTableName); });
  }


  [Fact]
  public async Task AddItemTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditAttributeTHelper.AddItemAsyncTest(AuditStorageModule, Mediator, GetTableName, GetColumnName); });
  }

  [Fact]
  public async Task UpdateItemTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditAttributeTHelper.UpdateItemAsyncTest(AuditStorageModule, Mediator, GetTableName, GetColumnName); });
  }
  
  [Fact]
  public async Task DeleteItemTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditAttributeTHelper.DeleteItemTest(AuditStorageModule, Mediator, GetTableName, GetColumnName); });
  }
}

public static class AuditAttributeTHelper
{
  public static async Task NoAuditAsyncTest(IAuditStorageModule auditStorageModule, IMediator mediator, Func<string, string> getTableName)
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
    res.Should().Be(true);
    
    // Assert.
    var allTestData = await mediator.Send(new TestGetQuery());
    allTestData.Count().Should().Be(1);
    var isAudit = await auditStorageModule.AuditItemsAsync(getTableName(entityName), item.Id);
    isAudit.Count().Should().Be(0);
  }

  public static async Task AddItemAsyncTest(IAuditStorageModule auditStorageModule, IMediator mediator, Func<string, string> getTableName, Func<string, string, string> getColumnName)
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

    var res = await mediator.Send(new TestAttributeAuditSaveCommand(item));  //testStorageEfContext.AddAsync(item);
    res.Should().Be(true);

    // Assert.
    var allData = await mediator.Send(new TestAttributeAuditGetQuery());
    allData.Should().HaveCount(1);

    var auditValues = await auditStorageModule.AuditItemsAsync(getTableName(entityName), item.Id);
    var auditVwAuditEntities = auditValues as AuditVwAuditEntity[] ?? auditValues.ToArray();
    auditVwAuditEntities.Length.Should().Be(3);

    var aid = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(entityName, nameof(TestAttributeAuditData.Id)));
    var aName = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(entityName, nameof(TestAttributeAuditData.Name)));
    var aCreated = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(entityName, nameof(TestAttributeAuditData.Created)));

    Assert.NotNull(aid);
    Assert.NotNull(aName);
    Assert.NotNull(aCreated);
    Assert.NotNull(aName.NewValueString);
    Assert.NotNull(aCreated.NewValueLong);

    Assert.True(aid.AuditId == aName.AuditId && aid.AuditId == aCreated.AuditId);
    Assert.Equal(aid.NewValueInt, item.Id);
    Assert.Equal(aName.NewValueString, testName);
    Assert.Equal(new DateTime(aCreated.NewValueLong.Value), testDateTime);
  }

  public static async Task UpdateItemAsyncTest(IAuditStorageModule auditStorageModule, IMediator mediator, Func<string, string> getTableName, Func<string, string, string> getColumnName)
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
    res.Should().Be(true);
    
    item.Name = testNameNew;
    
    var res2 = await mediator.Send(new TestAttributeAuditSaveCommand(item)); 
    res2.Should().Be(true);

    // Assert.
    var allData = await mediator.Send(new TestAttributeAuditGetQuery());
    allData.Should().HaveCount(1);

    var allAuditItems = await auditStorageModule.AllAuditItemsAsync(getTableName(entityName));

    Assert.Equal(1, allAuditItems.Count(i => i.EntityState == EntityState.Modified));

    var auditValues = await auditStorageModule.AuditItemsAsync(getTableName(entityName), item.Id);
    var auditVwAuditEntities = auditValues as AuditVwAuditEntity[] ?? auditValues.ToArray();
    Assert.Equal(4, auditVwAuditEntities.Length);
    var aid = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(entityName, nameof(TestAttributeAuditData.Id)) && a.EntityState == EntityState.Added);
    var aName = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(entityName, nameof(TestAttributeAuditData.Name)) && a.EntityState == EntityState.Added);
    var aCreated = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(entityName, nameof(TestAttributeAuditData.Created)) && a.EntityState == EntityState.Added);

    Assert.NotNull(aid);
    Assert.NotNull(aName);
    Assert.NotNull(aCreated);
    Assert.NotNull(aCreated.NewValueLong);

    Assert.Equal(aid.NewValueInt, item.Id);
    Assert.Equal(aName.NewValueString, testNameOld);
    Assert.Equal(new DateTime(aCreated.NewValueLong.Value), testDateTime);

    var aNameUpdate = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(entityName, nameof(TestAttributeAuditData.Name)) && a.EntityState == EntityState.Modified);
    var aCreatedUpdate = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(entityName, nameof(TestAttributeAuditData.Created)) && a.EntityState == EntityState.Modified);
    Assert.NotNull(aNameUpdate);
    Assert.Null(aCreatedUpdate);

    Assert.Equal(aNameUpdate.OldValueString, testNameOld);
    Assert.Equal(aNameUpdate.NewValueString, testNameNew);
  }

  public static async Task DeleteItemTest(IAuditStorageModule auditStorageModule, IMediator mediator, Func<string, string> getTableName, Func<string, string, string> getColumnName)
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
    res.Should().Be(true);
    
    var res2 = await mediator.Send(new TestAttributeAuditDeleteCommand(item)); 
    res2.Should().Be(true);

    // Assert.
    var allData = await mediator.Send(new TestAttributeAuditGetQuery());
    allData.Should().HaveCount(0);

    var allAuditItems = await auditStorageModule.AllAuditItemsAsync(getTableName(entityName));

    Assert.Equal(3, allAuditItems.Count(i => i.EntityState == EntityState.Deleted));

    var auditValues = await auditStorageModule.AuditItemsAsync(getTableName(entityName), item.Id);
    var auditVwAuditEntities = auditValues as AuditVwAuditEntity[] ?? auditValues.ToArray();
    Assert.Equal(6, auditVwAuditEntities.Length);
    var aid = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(entityName, nameof(TestAttributeAuditData.Id)) && a.EntityState == EntityState.Added);
    var aName = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(entityName, nameof(TestAttributeAuditData.Name)) && a.EntityState == EntityState.Added);
    var aCreated = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(entityName, nameof(TestAttributeAuditData.Created)) && a.EntityState == EntityState.Added);

    Assert.NotNull(aid);
    Assert.NotNull(aName);
    Assert.NotNull(aCreated);
    Assert.NotNull(aCreated.NewValueLong);

    Assert.Equal(aid.NewValueInt, item.Id);
    Assert.Equal(aName.NewValueString, testNameOld);
    Assert.Equal(new DateTime(aCreated.NewValueLong.Value), testDateTimeOld);

    var aidDeleted = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(entityName, nameof(TestAttributeAuditData.Id)) && a.EntityState == EntityState.Deleted);
    var aNameDeleted = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(entityName, nameof(TestAttributeAuditData.Name)) && a.EntityState == EntityState.Deleted);
    var aCreatedDeleted = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(entityName, nameof(TestAttributeAuditData.Created)) && a.EntityState == EntityState.Deleted);

    Assert.NotNull(aNameDeleted);
    Assert.NotNull(aCreatedDeleted);
    Assert.NotNull(aidDeleted);

    Assert.Null(aCreatedDeleted.NewValueLong);
    Assert.Null(aNameDeleted.NewValueString);
    Assert.Null(aidDeleted.NewValueInt);
    Assert.NotNull(aCreatedDeleted.OldValueLong);

    Assert.Equal(aidDeleted.OldValueInt, item.Id);
    Assert.Equal(aNameDeleted.OldValueString, testNameOld);
    Assert.Equal(new DateTime(aCreatedDeleted.OldValueLong.Value), testDateTimeOld);
  }
}