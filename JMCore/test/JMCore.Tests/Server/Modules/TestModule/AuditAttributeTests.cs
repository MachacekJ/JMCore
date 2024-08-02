using System.Reflection;
using FluentAssertions;
using JMCore.Server.Modules.AuditModule.Configuration;
using JMCore.Server.Modules.AuditModule.Storage;
using JMCore.Server.Modules.AuditModule.Storage.Models;
using JMCore.Tests.Implementations.Modules.TestModule.Storages;
using JMCore.Tests.Implementations.Modules.TestModule.Storages.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace JMCore.Tests.Server.Modules.TestModule;

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
    await RunTestAsync(method, async () => { await AuditAttributeTHelper.NoAuditAsyncTest(AuditStorageModule, TestStorageModule, GetTableName); });
  }


  [Fact]
  public async Task AddItemTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditAttributeTHelper.AddItemAsyncTest(AuditStorageModule, TestStorageModule, GetTableName, GetColumnName); });
  }

  [Fact]
  public async Task UpdateItemTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditAttributeTHelper.UpdateItemAsyncTest(AuditStorageModule, TestStorageModule, GetTableName, GetColumnName); });
  }


  [Fact]
  public async Task UpdateItemSyncTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      var auditSqlStorageImpl = AuditStorageModule as AuditSqlStorageImpl ?? throw new ArgumentNullException($"{nameof(AuditStorageModule)} doesn't implement {nameof(AuditSqlStorageImpl)}'");
      var testStorageEfContext = TestStorageModule as TestStorageEfContext ?? throw new ArgumentNullException($"{nameof(TestStorageModule)} doesn't implement {nameof(TestStorageEfContext)}'");

      var testDateTimeOld = DateTime.Now;
      var testNameOld = "AuditTest";

      var testDateTimeNew = DateTime.Now.AddDays(22);
      var testNameNew = "AuditTestNew";

      // Action.
      var item = new TestAttributeAuditEntity()
      {
        Created = testDateTimeOld,
        Name = testNameOld,
        NotAuditableColumn = "Audit"
      };

      testStorageEfContext.TestAttributeAudits.Add(item);
      // ReSharper disable once MethodHasAsyncOverload
      testStorageEfContext.SaveChanges();

      item.Name = testNameNew;
      item.Created = testDateTimeNew;
      // ReSharper disable once MethodHasAsyncOverload
      testStorageEfContext.SaveChanges();

      // Assert.
      Assert.True(testStorageEfContext.TestAttributeAudits.Count() == 1);

      var isAudit = await auditSqlStorageImpl.Audits
        .Include(a => a.AuditTable)
        .CountAsync(ae => ae.AuditTable.TableName == nameof(TestAttributeAuditEntity));

      Assert.Equal(2, isAudit);

      var auditValues = await AuditStorageModule.AuditItemsAsync(nameof(TestAttributeAuditEntity), item.Id);
      var auditVwAuditEntities = auditValues as AuditVwAuditEntity[] ?? auditValues.ToArray();
      Assert.Equal(5, auditVwAuditEntities.Length);
      var aid = auditVwAuditEntities.FirstOrDefault(a => a is { ColumnName: "Id", EntityState: EntityState.Added });
      var aName = auditVwAuditEntities.FirstOrDefault(a => a is { ColumnName: "Name", EntityState: EntityState.Added });
      var aCreated = auditVwAuditEntities.FirstOrDefault(a => a is { ColumnName: "Created", EntityState: EntityState.Added });

      Assert.NotNull(aid);
      Assert.NotNull(aName);
      Assert.NotNull(aCreated);
      Assert.NotNull(aCreated.NewValueLong);
      Assert.NotNull(aName.NewValueString);

      Assert.Equal(aid.NewValueInt, item.Id);
      Assert.Equal(aName.NewValueString, testNameOld);
      Assert.Equal(new DateTime(aCreated.NewValueLong.Value), testDateTimeOld);

      var aNameUpdate = auditVwAuditEntities.FirstOrDefault(a => a is { ColumnName: "Name", EntityState: EntityState.Modified });
      var aCreatedUpdate = auditVwAuditEntities.FirstOrDefault(a => a is { ColumnName: "Created", EntityState: EntityState.Modified });
      Assert.NotNull(aNameUpdate);
      Assert.NotNull(aCreatedUpdate);
      Assert.NotNull(aCreatedUpdate.OldValueLong);
      Assert.NotNull(aCreatedUpdate.NewValueLong);

      Assert.Equal(aNameUpdate.OldValueString, testNameOld);
      Assert.Equal(aNameUpdate.NewValueString, testNameNew);

      Assert.Equal(new DateTime(aCreatedUpdate.OldValueLong.Value), testDateTimeOld);
      Assert.Equal(new DateTime(aCreatedUpdate.NewValueLong.Value), testDateTimeNew);
    });
  }

  [Fact]
  public async Task DeleteItemTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditAttributeTHelper.DeleteItemTest(AuditStorageModule, TestStorageModule, GetTableName, GetColumnName); });
  }
}

public static class AuditAttributeTHelper
{
  public static async Task NoAuditAsyncTest(IAuditStorageModule auditStorageModule, ITestStorageModule testStorageEfContext, Func<Type, string> getTableName)
  {
    var testDateTime = DateTime.UtcNow;
    const string testName = "AuditTest";

    var item = new TestEntity
    {
      Created = testDateTime,
      Name = testName,
    };
    await testStorageEfContext.AddAsync(item);

    // Assert.
    (await testStorageEfContext.AllTest()).Count().Should().Be(1);
    var isAudit = await auditStorageModule.AuditItemsAsync(getTableName(typeof(TestEntity)), item.Id);
    isAudit.Count().Should().Be(0);
  }

  public static async Task AddItemAsyncTest(IAuditStorageModule auditStorageModule, ITestStorageModule testStorageEfContext, Func<Type, string> getTableName, Func<Type, string, string> getColumnName)
  {
    var testDateTime = DateTime.UtcNow;
    var testName = "AuditTest";

    // Action.
    var item = new TestAttributeAuditEntity
    {
      Created = testDateTime,
      Name = testName,
      NotAuditableColumn = "Audit"
    };

    await testStorageEfContext.AddAsync(item);


    // Assert.
    (await testStorageEfContext.AllTestAttribute()).Count().Should().Be(1);

    var auditValues = await auditStorageModule.AuditItemsAsync(getTableName(typeof(TestAttributeAuditEntity)), item.Id);
    var auditVwAuditEntities = auditValues as AuditVwAuditEntity[] ?? auditValues.ToArray();
    auditVwAuditEntities.Length.Should().Be(3);

    var aid = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(typeof(TestAttributeAuditEntity), nameof(TestAttributeAuditEntity.Id)));
    var aName = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(typeof(TestAttributeAuditEntity), nameof(TestAttributeAuditEntity.Name)));
    var aCreated = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(typeof(TestAttributeAuditEntity), nameof(TestAttributeAuditEntity.Created)));

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

  public static async Task UpdateItemAsyncTest(IAuditStorageModule auditStorageModule, ITestStorageModule testStorageEfContext, Func<Type, string> getTableName, Func<Type, string, string> getColumnName)
  {
    var testDateTime = DateTime.UtcNow;
    var testNameOld = "AuditTest";
    var testNameNew = "AuditTestNew";

    // Action.
    var item = new TestAttributeAuditEntity()
    {
      Created = testDateTime,
      Name = testNameOld,
      NotAuditableColumn = "Audit"
    };

    await testStorageEfContext.AddAsync(item);

    item.Name = testNameNew;
    await testStorageEfContext.UpdateAsync(item);

    // Assert.
    (await testStorageEfContext.AllTestAttribute()).Should().HaveCount(1);

    var allAuditItems = await auditStorageModule.AllAuditItemsAsync(getTableName(typeof(TestAttributeAuditEntity)));

    Assert.Equal(1, allAuditItems.Count(i => i.EntityState == EntityState.Modified));

    var auditValues = await auditStorageModule.AuditItemsAsync(getTableName(typeof(TestAttributeAuditEntity)), item.Id);
    var auditVwAuditEntities = auditValues as AuditVwAuditEntity[] ?? auditValues.ToArray();
    Assert.Equal(4, auditVwAuditEntities.Length);
    var aid = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(typeof(TestAttributeAuditEntity), nameof(TestAttributeAuditEntity.Id)) && a.EntityState == EntityState.Added);
    var aName = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(typeof(TestAttributeAuditEntity), nameof(TestAttributeAuditEntity.Name)) && a.EntityState == EntityState.Added);
    var aCreated = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(typeof(TestAttributeAuditEntity), nameof(TestAttributeAuditEntity.Created)) && a.EntityState == EntityState.Added);

    Assert.NotNull(aid);
    Assert.NotNull(aName);
    Assert.NotNull(aCreated);
    Assert.NotNull(aCreated.NewValueLong);

    Assert.Equal(aid.NewValueInt, item.Id);
    Assert.Equal(aName.NewValueString, testNameOld);
    Assert.Equal(new DateTime(aCreated.NewValueLong.Value), testDateTime);

    var aNameUpdate = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(typeof(TestAttributeAuditEntity), nameof(TestAttributeAuditEntity.Name)) && a.EntityState == EntityState.Modified);
    var aCreatedUpdate = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(typeof(TestAttributeAuditEntity), nameof(TestAttributeAuditEntity.Created)) && a.EntityState == EntityState.Modified);
    Assert.NotNull(aNameUpdate);
    Assert.Null(aCreatedUpdate);

    Assert.Equal(aNameUpdate.OldValueString, testNameOld);
    Assert.Equal(aNameUpdate.NewValueString, testNameNew);
  }

  public static async Task DeleteItemTest(IAuditStorageModule auditStorageModule, ITestStorageModule testStorageEfContext, Func<Type, string> getTableName, Func<Type, string, string> getColumnName)
  {
    var testDateTimeOld = DateTime.UtcNow;
    var testNameOld = "AuditTest";

    // Action.
    var item = new TestAttributeAuditEntity()
    {
      Created = testDateTimeOld,
      Name = testNameOld,
      NotAuditableColumn = "Audit"
    };

    await testStorageEfContext.AddAsync(item);
    await testStorageEfContext.DeleteAsync(item);

    // Assert.
    (await testStorageEfContext.AllTestAttribute()).Should().HaveCount(0);

    var allAuditItems = await auditStorageModule.AllAuditItemsAsync(getTableName(typeof(TestAttributeAuditEntity)));

    Assert.Equal(3, allAuditItems.Count(i => i.EntityState == EntityState.Deleted));

    var auditValues = await auditStorageModule.AuditItemsAsync(getTableName(typeof(TestAttributeAuditEntity)), item.Id);
    var auditVwAuditEntities = auditValues as AuditVwAuditEntity[] ?? auditValues.ToArray();
    Assert.Equal(6, auditVwAuditEntities.Length);
    var aid = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(typeof(TestAttributeAuditEntity), nameof(TestAttributeAuditEntity.Id)) && a.EntityState == EntityState.Added);
    var aName = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(typeof(TestAttributeAuditEntity), nameof(TestAttributeAuditEntity.Name)) && a.EntityState == EntityState.Added);
    var aCreated = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(typeof(TestAttributeAuditEntity), nameof(TestAttributeAuditEntity.Created)) && a.EntityState == EntityState.Added);

    Assert.NotNull(aid);
    Assert.NotNull(aName);
    Assert.NotNull(aCreated);
    Assert.NotNull(aCreated.NewValueLong);

    Assert.Equal(aid.NewValueInt, item.Id);
    Assert.Equal(aName.NewValueString, testNameOld);
    Assert.Equal(new DateTime(aCreated.NewValueLong.Value), testDateTimeOld);

    var aidDeleted = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(typeof(TestAttributeAuditEntity), nameof(TestAttributeAuditEntity.Id)) && a.EntityState == EntityState.Deleted);
    var aNameDeleted = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(typeof(TestAttributeAuditEntity), nameof(TestAttributeAuditEntity.Name)) && a.EntityState == EntityState.Deleted);
    var aCreatedDeleted = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == getColumnName(typeof(TestAttributeAuditEntity), nameof(TestAttributeAuditEntity.Created)) && a.EntityState == EntityState.Deleted);

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