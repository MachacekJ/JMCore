using System.Diagnostics;
using System.Reflection;
using JMCore.Server.Storages.Base.Audit.Configuration;
using JMCore.Tests.ServerT.StoragesT.Impl.TestStorageModule.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace JMCore.Tests.ServerT.StoragesT.AuditStorageT;

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
                { nameof(TestManualAuditEntity), new List<string>() { nameof(TestManualAuditEntity.NotAuditableColumn) } }
            }
        };
        sc.AddSingleton<IAuditConfiguration>(auditConfiguration);
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
            await TestStorageEfContext.Tests.AddAsync(item);
            await TestStorageEfContext.SaveChangesAsync();

            // Assert.
            Assert.True(await TestStorageEfContext.Tests.CountAsync() == 1);
            var isAudit = await AuditEfStorageEfContext.Audits
                .Include(a => a.AuditTable)
                .CountAsync(ae => ae.AuditTable.TableName == "Test");
            Assert.Equal(0, isAudit);
        });
    }


    [Fact]
    public async Task AddItem()
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

            TestStorageEfContext.TestManualAudits.Add(item);
            await TestStorageEfContext.SaveChangesAsync();

            // Assert.
            Assert.True(TestStorageEfContext.TestManualAudits.Count() == 1);

            var isAudit = await AuditEfStorageEfContext.Audits
                .Include(a => a.AuditTable)
                .CountAsync(ae => ae.AuditTable.TableName == nameof(TestManualAuditEntity));

            Assert.Equal(1, isAudit);

            var auditValues = await AuditEfStorageEfContext.VwAudits().Where(a => a.TableName ==  nameof(TestManualAuditEntity) && a.PKValue == item.Id).ToListAsync();
            Assert.Equal(3, auditValues.Count);
            var aid = auditValues.FirstOrDefault(a => a.ColumnName == "Id");
            var aName = auditValues.FirstOrDefault(a => a.ColumnName == "Name");
            var aCreated = auditValues.FirstOrDefault(a => a.ColumnName == "Created");

            Assert.NotNull(aid);
            Assert.NotNull(aName);
            Assert.NotNull(aCreated);

            Assert.Equal(aid.NewValueInt, item.Id);
            Assert.Equal(aName.NewValueString, testName);
            Debug.Assert(aCreated.NewValueLong != null, "aCreated.NewValueLong != null");
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

            TestStorageEfContext.TestManualAudits.Add(item);
            await TestStorageEfContext.SaveChangesAsync();

            item.Name = testNameNew;
            item.Created = testDateTimeNew;
            await TestStorageEfContext.SaveChangesAsync();

            // Assert.
            Assert.True(TestStorageEfContext.TestManualAudits.Count() == 1);

            var isAudit = await AuditEfStorageEfContext.Audits
                .Include(a => a.AuditTable)
                .CountAsync(ae => ae.AuditTable.TableName == nameof(TestManualAuditEntity));

            Assert.Equal(2, isAudit);

            var auditValues = await AuditEfStorageEfContext.VwAudits().Where(a => a.TableName == nameof(TestManualAuditEntity) && a.PKValue == item.Id).ToListAsync();
            Assert.Equal(5, auditValues.Count);
            var aid = auditValues.FirstOrDefault(a => a.ColumnName == "Id" && a.EntityState == EntityState.Added);
            var aName = auditValues.FirstOrDefault(a => a.ColumnName == "Name" && a.EntityState == EntityState.Added);
            var aCreated = auditValues.FirstOrDefault(a => a.ColumnName == "Created" && a.EntityState == EntityState.Added);

            Assert.NotNull(aid);
            Assert.NotNull(aName);
            Assert.NotNull(aCreated);
            Assert.NotNull(aCreated.NewValueLong);

            Assert.Equal(aid.NewValueInt, item.Id);
            Assert.Equal(aName.NewValueString, testNameOld);
            Assert.Equal(new DateTime(aCreated.NewValueLong.Value), testDateTimeOld);

            var aNameUpdate = auditValues.FirstOrDefault(a => a.ColumnName == "Name" && a.EntityState == EntityState.Modified);
            var aCreatedUpdate = auditValues.FirstOrDefault(a => a.ColumnName == "Created" && a.EntityState == EntityState.Modified);
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
            var item = new TestManualAuditEntity()
            {
                Created = testDateTimeOld,
                Name = testNameOld,
                NotAuditableColumn = "Audit"
            };

            TestStorageEfContext.TestManualAudits.Add(item);
            await TestStorageEfContext.SaveChangesAsync();

            TestStorageEfContext.TestManualAudits.Remove(item);
            await TestStorageEfContext.SaveChangesAsync();

            // Assert.
            Assert.True(!TestStorageEfContext.TestManualAudits.Any());

            var isAudit = await AuditEfStorageEfContext.Audits
                .Include(a => a.AuditTable)
                .CountAsync(ae => ae.AuditTable.TableName == nameof(TestManualAuditEntity));

            Assert.Equal(2, isAudit);

            var auditValues = await AuditEfStorageEfContext.VwAudits().Where(a => a.TableName == nameof(TestManualAuditEntity) && a.PKValue == item.Id).ToListAsync();
            Assert.Equal(6, auditValues.Count);
            var aid = auditValues.FirstOrDefault(a => a.ColumnName == "Id" && a.EntityState == EntityState.Added);
            var aName = auditValues.FirstOrDefault(a => a.ColumnName == "Name" && a.EntityState == EntityState.Added);
            var aCreated = auditValues.FirstOrDefault(a => a.ColumnName == "Created" && a.EntityState == EntityState.Added);

            Assert.NotNull(aid);
            Assert.NotNull(aName);
            Assert.NotNull(aCreated);
            Assert.NotNull(aCreated.NewValueLong);

            Assert.Equal(aid.NewValueInt, item.Id);
            Assert.Equal(aName.NewValueString, testNameOld);
            Assert.Equal(new DateTime(aCreated.NewValueLong.Value), testDateTimeOld);

            var aidDeleted = auditValues.FirstOrDefault(a => a.ColumnName == "Id" && a.EntityState == EntityState.Deleted);
            var aNameDeleted = auditValues.FirstOrDefault(a => a.ColumnName == "Name" && a.EntityState == EntityState.Deleted);
            var aCreatedDeleted = auditValues.FirstOrDefault(a => a.ColumnName == "Created" && a.EntityState == EntityState.Deleted);

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