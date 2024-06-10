using System.Diagnostics;
using System.Reflection;
using FluentAssertions;
using JMCore.Server.Storages.Modules.AuditModule;
using JMCore.Server.Storages.Modules.AuditModule.Models;
using JMCore.Tests.ServerT.StoragesT.Impl.TestStorageModule;
using JMCore.Tests.ServerT.StoragesT.Impl.TestStorageModule.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace JMCore.Tests.ServerT.StoragesT.AuditStorageT;

public static class AuditAttributeTHelper
{
  public static async Task NoAudit(IAuditStorageModule auditStorageModule, ITestStorageModule testStorageEfContext, Func<string, string> getTableName)
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
    var isAudit = await auditStorageModule.AuditItemsAsync(getTableName(nameof(TestEntity)), item.Id);
    isAudit.Count().Should().Be(0);
  }

  public static async Task AddItem(IAuditStorageModule auditStorageModule, ITestStorageModule testStorageEfContext, Func<string, string> getTableName)
  {
    var testDateTime = DateTime.Now;
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
    
    var auditValues = await auditStorageModule.AuditItemsAsync(getTableName(nameof(TestAttributeAuditEntity)), item.Id);
    var auditVwAuditEntities = auditValues as AuditVwAuditEntity[] ?? auditValues.ToArray();
    auditVwAuditEntities.Length.Should().Be(3);
    
    var aid = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == "Id");
    var aName = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == "Name");
    var aCreated = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == "Created");

    Assert.NotNull(aid);
    Assert.NotNull(aName);
    Assert.NotNull(aCreated);
    Assert.NotNull(aName.NewValueString);
    Assert.NotNull(aCreated.NewValueLong);
    
    Assert.True(aid.AuditId == aName.AuditId && aid.AuditId  == aCreated.AuditId);
    Assert.Equal(aid.NewValueInt, item.Id);
    Assert.Equal(aName.NewValueString, testName);
    Assert.Equal(new DateTime(aCreated.NewValueLong.Value), testDateTime);
  }
}

public class AuditAttributeT : AuditAttributeBaseT
{
  [Fact]
  public async Task NoAudit()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditAttributeTHelper.NoAudit(AuditEfStorageEfContext, TestStorageEfContext, (name) => name); });
  }

  [Fact]
  public async Task AddItem()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditAttributeTHelper.AddItem(AuditEfStorageEfContext, TestStorageEfContext, (name) => name); });
  }

  [Fact]
  public async Task UpdateItem()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      var testDateTime = DateTime.Now;
      var testNameOld = "AuditTest";
      var testNameNew = "AuditTestNew";

      // Action.
      var item = new TestAttributeAuditEntity()
      {
        Created = testDateTime,
        Name = testNameOld,
        NotAuditableColumn = "Audit"
      };

      await TestStorageEfContext.TestAttributeAudits.AddAsync(item);
      await TestStorageEfContext.SaveChangesAsync();

      item.Name = testNameNew;
      await TestStorageEfContext.SaveChangesAsync();

      // Assert.
      Assert.True(TestStorageEfContext.TestAttributeAudits.Count() == 1);

      var isAudit = await AuditEfStorageEfContext.Audits
        .Include(a => a.AuditTable)
        .CountAsync(ae => ae.AuditTable.TableName == nameof(TestAttributeAuditEntity));

      Assert.Equal(2, isAudit);

      var auditValues = await AuditEfStorageEfContext.AuditItemsAsync(nameof(TestAttributeAuditEntity), item.Id);
      var auditVwAuditEntities = auditValues as AuditVwAuditEntity[] ?? auditValues.ToArray();
      Assert.Equal(4, auditVwAuditEntities.Length);
      var aid = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == "Id" && a.EntityState == EntityState.Added);
      var aName = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == "Name" && a.EntityState == EntityState.Added);
      var aCreated = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == "Created" && a.EntityState == EntityState.Added);

      Assert.NotNull(aid);
      Assert.NotNull(aName);
      Assert.NotNull(aCreated);
      Assert.NotNull(aCreated.NewValueLong);

      Assert.Equal(aid.NewValueInt, item.Id);
      Assert.Equal(aName.NewValueString, testNameOld);
      Assert.Equal(new DateTime(aCreated.NewValueLong.Value), testDateTime);

      var aNameUpdate = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == "Name" && a.EntityState == EntityState.Modified);
      var aCreatedUpdate = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == "Created" && a.EntityState == EntityState.Modified);
      Assert.NotNull(aNameUpdate);
      Assert.Null(aCreatedUpdate);

      Assert.Equal(aNameUpdate.OldValueString, testNameOld);
      Assert.Equal(aNameUpdate.NewValueString, testNameNew);
    });
  }

  [Fact]
  public async Task UpdateItemSync()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
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

      TestStorageEfContext.TestAttributeAudits.Add(item);
      // ReSharper disable once MethodHasAsyncOverload
      TestStorageEfContext.SaveChanges();

      item.Name = testNameNew;
      item.Created = testDateTimeNew;
      // ReSharper disable once MethodHasAsyncOverload
      TestStorageEfContext.SaveChanges();

      // Assert.
      Assert.True(TestStorageEfContext.TestAttributeAudits.Count() == 1);

      var isAudit = await AuditEfStorageEfContext.Audits
        .Include(a => a.AuditTable)
        .CountAsync(ae => ae.AuditTable.TableName == nameof(TestAttributeAuditEntity));

      Assert.Equal(2, isAudit);

      var auditValues = await AuditEfStorageEfContext.AuditItemsAsync(nameof(TestAttributeAuditEntity), item.Id);
      var auditVwAuditEntities = auditValues as AuditVwAuditEntity[] ?? auditValues.ToArray();
      Assert.Equal(5, auditVwAuditEntities.Length);
      var aid = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == "Id" && a.EntityState == EntityState.Added);
      var aName = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == "Name" && a.EntityState == EntityState.Added);
      var aCreated = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == "Created" && a.EntityState == EntityState.Added);

      Assert.NotNull(aid);
      Assert.NotNull(aName);
      Assert.NotNull(aCreated);
      Assert.NotNull(aCreated.NewValueLong);
      Assert.NotNull(aName.NewValueString);

      Assert.Equal(aid.NewValueInt, item.Id);
      Assert.Equal(aName.NewValueString, testNameOld);
      Assert.Equal(new DateTime(aCreated.NewValueLong.Value), testDateTimeOld);

      var aNameUpdate = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == "Name" && a.EntityState == EntityState.Modified);
      var aCreatedUpdate = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == "Created" && a.EntityState == EntityState.Modified);
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
  public async Task DeleteItem()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      var testDateTimeOld = DateTime.Now;
      var testNameOld = "AuditTest";

      // Action.
      var item = new TestAttributeAuditEntity()
      {
        Created = testDateTimeOld,
        Name = testNameOld,
        NotAuditableColumn = "Audit"
      };

      await TestStorageEfContext.TestAttributeAudits.AddAsync(item);
      await TestStorageEfContext.SaveChangesAsync();

      TestStorageEfContext.TestAttributeAudits.Remove(item);
      await TestStorageEfContext.SaveChangesAsync();

      // Assert.
      Assert.True(!TestStorageEfContext.TestAttributeAudits.Any());

      var isAudit = await AuditEfStorageEfContext.Audits
        .Include(a => a.AuditTable)
        .CountAsync(ae => ae.AuditTable.TableName == nameof(TestAttributeAuditEntity));

      Assert.Equal(2, isAudit);

      Assert.Equal(2, isAudit);

      var auditValues = await AuditEfStorageEfContext.AuditItemsAsync(nameof(TestAttributeAuditEntity), item.Id);
      var auditVwAuditEntities = auditValues as AuditVwAuditEntity[] ?? auditValues.ToArray();
      Assert.Equal(6, auditVwAuditEntities.Length);
      var aid = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == "Id" && a.EntityState == EntityState.Added);
      var aName = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == "Name" && a.EntityState == EntityState.Added);
      var aCreated = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == "Created" && a.EntityState == EntityState.Added);

      Assert.NotNull(aid);
      Assert.NotNull(aName);
      Assert.NotNull(aCreated);
      Assert.NotNull(aCreated.NewValueLong);

      Assert.Equal(aid.NewValueInt, item.Id);
      Assert.Equal(aName.NewValueString, testNameOld);
      Assert.Equal(new DateTime(aCreated.NewValueLong.Value), testDateTimeOld);

      var aidDeleted = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == "Id" && a.EntityState == EntityState.Deleted);
      var aNameDeleted = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == "Name" && a.EntityState == EntityState.Deleted);
      var aCreatedDeleted = auditVwAuditEntities.FirstOrDefault(a => a.ColumnName == "Created" && a.EntityState == EntityState.Deleted);

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
    });
  }
}