using System.Diagnostics;
using System.Reflection;
using FluentAssertions;
using JMCore.Server.Modules.AuditModule.Configuration;
using JMCore.Server.Modules.AuditModule.EF;
using JMCore.Server.Modules.AuditModule.Storage.Models;
using JMCore.Server.Modules.AuditModule.UserProvider;
using JMCore.Tests.Implementations.Storages.TestModule.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace JMCore.Tests.ServerT.StoragesT.ModulesT.AuditStorageT;

/// <summary>
/// Test for audit which is manually set for entity. <see cref="AuditConfiguration"/>.
/// </summary>
public class AuditManualT : AuditStorageBaseT
{
  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);

    var auditConfiguration = new AuditConfiguration
    {
      AuditEntities = new List<string> { nameof(TestManualAuditEntity) },
      NotAuditProperty = new Dictionary<string, IEnumerable<string>>
      {
        { nameof(TestManualAuditEntity), new List<string> { nameof(TestManualAuditEntity.NotAuditableColumn) } }
      }
    };
    sc.AddSingleton<IAuditConfiguration>(auditConfiguration);
    sc.AddScoped<IAuditDbService, AuditDbService>();
    sc.AddSingleton<IAuditUserProvider>(TestAuditUserProvider.CreateDefaultUser());
  }

  [Fact]
  public async Task NoAudit()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      var testDateTime = DateTime.Now;
      const string testName = "AuditTest";

      var item = new TestEntity()
      {
        Created = testDateTime,
        Name = testName,
      };
      await TestStorageModule.AddAsync(item);

      // Assert.
      (await TestStorageModule.AllTest()).Count().Should().Be(1);
      var isAudit = await AuditStorageModule.AuditItemsAsync("Test", item.Id);
      isAudit.Count().Should().Be(0);
    });
  }


  [Fact]
  public async Task AddItemAsync()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      var testDateTime = DateTime.Now;
      var testName = "AuditTest";

      // Action.
      var item = new TestManualAuditEntity()
      {
        Created = testDateTime,
        Name = testName,
        NotAuditableColumn = "Audit"
      };

      await TestStorageModule.AddAsync(item);

      // Assert.
      (await TestStorageModule.AllTestManual()).Count().Should().Be(1);

      var auditValues = await AuditStorageModule.AuditItemsAsync(nameof(TestManualAuditEntity), item.Id);
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

      Assert.True(aid.AuditId == aName.AuditId && aid.AuditId == aCreated.AuditId);
      Assert.Equal(aid.NewValueInt, item.Id);
      Assert.Equal(aName.NewValueString, testName);
      Assert.Equal(new DateTime(aCreated.NewValueLong.Value), testDateTime);

    });
  }

  [Fact]
  public async Task UpdateItem()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      var testDateTimeOld = DateTime.Now;
      var testNameOld = "AuditTest";

      var testDateTimeNew = DateTime.Now.AddDays(22);
      var testNameNew = "AuditTestNew";

      // Action.
      var item = new TestManualAuditEntity()
      {
        Created = testDateTimeOld,
        Name = testNameOld,
        NotAuditableColumn = "Audit"
      };

      await TestStorageModule.AddAsync(item);

      item.Name = testNameNew;
     // item.Created = testDateTimeNew;
      await TestStorageModule.UpdateAsync(item);

      // Assert.
      (await TestStorageModule.AllTestManual()).Should().HaveCount(1);

      var allAuditItems = await AuditStorageModule.AllAuditItemsAsync(nameof(TestManualAuditEntity));
      Assert.Equal(1, allAuditItems.Count(i => i.EntityState == EntityState.Modified));

      var auditValues = await AuditStorageModule.AuditItemsAsync(nameof(TestManualAuditEntity), item.Id);
      var auditVwAuditEntities = auditValues as AuditVwAuditEntity[] ?? auditValues.ToArray();
      Assert.Equal(4, auditVwAuditEntities.Length);
      var aid = auditVwAuditEntities.FirstOrDefault(a => a is { ColumnName: "Id", EntityState: EntityState.Added });
      var aName = auditVwAuditEntities.FirstOrDefault(a => a is { ColumnName: "Name", EntityState: EntityState.Added });
      var aCreated = auditVwAuditEntities.FirstOrDefault(a => a is { ColumnName: "Created", EntityState: EntityState.Added });

      Assert.NotNull(aid);
      Assert.NotNull(aName);
      Assert.NotNull(aCreated);
      Assert.NotNull(aCreated.NewValueLong);

      Assert.Equal(aid.NewValueInt, item.Id);
      Assert.Equal(aName.NewValueString, testNameOld);
      Assert.Equal(new DateTime(aCreated.NewValueLong.Value), testDateTimeOld);

      var aNameUpdate = auditVwAuditEntities.FirstOrDefault(a => a is { ColumnName: "Name", EntityState: EntityState.Modified });
      var aCreatedUpdate = auditVwAuditEntities.FirstOrDefault(a => a is { ColumnName: "Created", EntityState: EntityState.Modified });
      Assert.NotNull(aNameUpdate);
      Assert.Null(aCreatedUpdate);

      Assert.Equal(aNameUpdate.OldValueString, testNameOld);
      Assert.Equal(aNameUpdate.NewValueString, testNameNew);
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
      var item = new TestManualAuditEntity()
      {
        Created = testDateTimeOld,
        Name = testNameOld,
        NotAuditableColumn = "Audit"
      };

      await TestStorageModule.AddAsync(item);

      await TestStorageModule.DeleteAsync(item);
      
      // Assert.
      (await TestStorageModule.AllTestManual()).Should().HaveCount(0);

      var allAuditItems = await AuditStorageModule.AllAuditItemsAsync(nameof(TestManualAuditEntity));


      Assert.Equal(3, allAuditItems.Count(i => i.EntityState == EntityState.Deleted));

      var auditValues = await AuditStorageModule.AuditItemsAsync(nameof(TestManualAuditEntity), item.Id);
      var auditVwAuditEntities = auditValues as AuditVwAuditEntity[] ?? auditValues.ToArray();
      Assert.Equal(6, auditVwAuditEntities.Length);
      var aid = auditVwAuditEntities.FirstOrDefault(a => a is { ColumnName: "Id", EntityState: EntityState.Added });
      var aName = auditVwAuditEntities.FirstOrDefault(a => a is { ColumnName: "Name", EntityState: EntityState.Added });
      var aCreated = auditVwAuditEntities.FirstOrDefault(a => a is { ColumnName: "Created", EntityState: EntityState.Added });
    
      Assert.NotNull(aid);
      Assert.NotNull(aName);
      Assert.NotNull(aCreated);
      Assert.NotNull(aCreated.NewValueLong);

      Assert.Equal(aid.NewValueInt, item.Id);
      Assert.Equal(aName.NewValueString, testNameOld);
      Assert.Equal(new DateTime(aCreated.NewValueLong.Value), testDateTimeOld);

      var aidDeleted = auditVwAuditEntities.FirstOrDefault(a => a is { ColumnName: "Id", EntityState: EntityState.Deleted });
      var aNameDeleted = auditVwAuditEntities.FirstOrDefault(a => a is { ColumnName: "Name", EntityState: EntityState.Deleted });
      var aCreatedDeleted = auditVwAuditEntities.FirstOrDefault(a => a is { ColumnName: "Created", EntityState: EntityState.Deleted });

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