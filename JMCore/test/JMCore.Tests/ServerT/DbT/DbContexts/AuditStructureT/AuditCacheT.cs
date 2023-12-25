﻿using System.Reflection;
using JMCore.CQRS.JMCache.CacheRemove;
using JMCore.Tests.ServerT.DbT.TestDBContext.Models;
using Serilog.Sinks.InMemory.Assertions;
using Xunit;

namespace JMCore.Tests.ServerT.DbT.DbContexts.AuditStructureT;

public class AuditCacheT : AuditAttributeBaseT
{
    [Fact]
    public async Task UserCache()
    {
        var method = MethodBase.GetCurrentMethod();
        await RunTestAsync(method, async () =>
        {
            var testDateTime = DateTime.Now;
            const string testNameOld = "AuditTest";
            const string testNameNew = "AuditTestNew";

            // Arrange 1
            var item = new TestAttributeAuditEntity
            {
                Created = testDateTime,
                Name = testNameOld,
                NotAuditableColumn = testNameOld
            };
            (UserProvider as TestAuditUserProvider)!.SetContext(TestAuditUserTypeEnum.Admin);
            var userId = UserProvider.GetUser().userId;
            var auditUserCacheKey = AuditDbContext.AuditUserCacheKey(userId);
            var auditUserCacheKeyString = auditUserCacheKey.ToString();

            // Act 1
            LogInMemorySink.Dispose();
            await TestBasicDbContext.TestAttributeAudits.AddAsync(item);
            await TestBasicDbContext.SaveChangesAsync();

            // Assert 1
            LogInMemorySink
                .Should()
                .HaveMessage("Value saved to cache:{GetAuditUserIdAsync}:{keyCache}:{userId}")
                .Appearing().Once()
                .WithProperty("keyCache").WithValue(auditUserCacheKeyString).And
                .WithProperty("userId").WithValue(userId);

            LogInMemorySink
                .Should()
                .HaveMessage("New db value created:{GetAuditUserIdAsync}:{keyCache}:{userId}")
                .Appearing().Once()
                .WithProperty("keyCache").WithValue(auditUserCacheKeyString).And
                .WithProperty("userId").WithValue(userId);

            // Arrange 2
            item.Name = testNameNew;

            // Act 2
            await TestBasicDbContext.SaveChangesAsync();

            // Assert 2
            LogInMemorySink
                .Should()
                .HaveMessage("Value from cache:{GetAuditUserIdAsync}:{keyCache}:{userId}")
                .Appearing().Once()
                .WithProperty("keyCache").WithValue(auditUserCacheKeyString).And
                .WithProperty("userId").WithValue(userId);

            LogInMemorySink
                .Should()
                .HaveMessage("New db value created:{GetAuditUserIdAsync}:{keyCache}:{userId}")
                .Appearing().Once()
                .WithProperty("keyCache").WithValue(auditUserCacheKeyString).And
                .WithProperty("userId").WithValue(userId);

            LogInMemorySink
                .Should()
                .HaveMessage("Value saved to cache:{GetAuditUserIdAsync}:{keyCache}:{userId}")
                .Appearing().Once()
                .WithProperty("keyCache").WithValue(auditUserCacheKeyString).And
                .WithProperty("userId").WithValue(userId);

            // Arrange 3
            await Mediator.Send(new CacheRemoveCommand(auditUserCacheKey));
            item.Name = testNameOld;

            // Act 3
            await TestBasicDbContext.SaveChangesAsync();

            // Assert 3
            LogInMemorySink
                .Should()
                .HaveMessage("Value from cache:{GetAuditUserIdAsync}:{keyCache}:{userId}")
                .Appearing().Once()
                .WithProperty("keyCache").WithValue(auditUserCacheKeyString).And
                .WithProperty("userId").WithValue(userId);

            LogInMemorySink
                .Should()
                .HaveMessage("New db value created:{GetAuditUserIdAsync}:{keyCache}:{userId}")
                .Appearing().Once()
                .WithProperty("keyCache").WithValue(auditUserCacheKeyString).And
                .WithProperty("userId").WithValue(userId);
            LogInMemorySink
                .Should()
                .HaveMessage("Value saved to cache:{GetAuditUserIdAsync}:{keyCache}:{userId}")
                .Appearing().Times(2)
                .WithProperty("keyCache").WithValues(auditUserCacheKeyString, auditUserCacheKeyString).And
                .WithProperty("userId").WithValues(userId, userId);
        });
    }

    [Fact]
    public async Task TableCache()
    {
        var tableName = nameof(TestAttributeAuditEntity);
        var method = MethodBase.GetCurrentMethod();
        await RunTestAsync(method, async () =>
        {
            var auditTableCacheKey = AuditDbContext.AuditTableCacheKey(tableName, null);
            var auditTableCacheKeyString = auditTableCacheKey.ToString();
            var testDateTime = DateTime.Now;
            const string testNameOld = "AuditTest";
            const string testNameNew = "AuditTestNew";

            // Arrange 1
            var item = new TestAttributeAuditEntity
            {
                Created = testDateTime,
                Name = testNameOld,
                NotAuditableColumn = testNameOld
            };

            // Act 1
            LogInMemorySink.Dispose();
            await TestBasicDbContext.TestAttributeAudits.AddAsync(item);
            await TestBasicDbContext.SaveChangesAsync();

            // Assert 1
            LogInMemorySink
                .Should()
                .HaveMessage("Value saved to cache:{GetAuditTableIdAsync}:{keyCache}:{tableName}:{tableSchema}")
                .Appearing().Once()
                .WithProperty("keyCache").WithValue(auditTableCacheKeyString).And
                .WithProperty("tableName").WithValue(tableName);

            LogInMemorySink
                .Should()
                .HaveMessage("New db value created:{GetAuditTableIdAsync}:{keyCache}:{tableName}:{tableSchema}")
                .Appearing().Once()
                .WithProperty("keyCache").WithValue(auditTableCacheKeyString).And
                .WithProperty("tableName").WithValue(tableName);

            // Arrange 2
            item.Name = testNameNew;

            // Act 2
            await TestBasicDbContext.SaveChangesAsync();

            // Assert 2
            LogInMemorySink
                .Should()
                .HaveMessage("Value from cache:{GetAuditTableIdAsync}:{keyCache}:{tableName}:{tableSchema}")
                .Appearing().Once()
                .WithProperty("keyCache").WithValue(auditTableCacheKeyString).And
                .WithProperty("tableName").WithValue(tableName);

            LogInMemorySink
                .Should()
                .HaveMessage("New db value created:{GetAuditTableIdAsync}:{keyCache}:{tableName}:{tableSchema}")
                .Appearing().Once()
                .WithProperty("keyCache").WithValue(auditTableCacheKeyString).And
                .WithProperty("tableName").WithValue(tableName);

            LogInMemorySink
                .Should()
                .HaveMessage("Value saved to cache:{GetAuditTableIdAsync}:{keyCache}:{tableName}:{tableSchema}")
                .Appearing().Once();

            // Arrange 3
            await Mediator.Send(new CacheRemoveCommand(auditTableCacheKey));
            item.Name = testNameOld;

            // Act 3
            await TestBasicDbContext.SaveChangesAsync();

            // Assert 3
            LogInMemorySink
                .Should()
                .HaveMessage("Value from cache:{GetAuditTableIdAsync}:{keyCache}:{tableName}:{tableSchema}")
                .Appearing().Once()
                .WithProperty("keyCache").WithValue(auditTableCacheKeyString).And
                .WithProperty("tableName").WithValue(tableName);

            LogInMemorySink
                .Should()
                .HaveMessage("New db value created:{GetAuditTableIdAsync}:{keyCache}:{tableName}:{tableSchema}")
                .Appearing().Once()
                .WithProperty("keyCache").WithValue(auditTableCacheKeyString).And
                .WithProperty("tableName").WithValue(tableName);
            LogInMemorySink
                .Should()
                .HaveMessage("Value saved to cache:{GetAuditTableIdAsync}:{keyCache}:{tableName}:{tableSchema}")
                .Appearing().Times(2)
                .WithProperty("keyCache").WithValues(auditTableCacheKeyString, auditTableCacheKeyString).And
                .WithProperty("tableName").WithValues(tableName, tableName);
        });
    }

    [Fact]
    public async Task ColumnCache()
    {
        var method = MethodBase.GetCurrentMethod();
        await RunTestAsync(method, async () =>
        {
            var auditTableId = await AuditDbContext.GetAuditTableIdAsync(nameof(TestAttributeAuditEntity), null);
            var auditColumnCacheKey = AuditDbContext.AuditColumnCacheKey(auditTableId);
            var auditColumnCacheKeyString = auditColumnCacheKey.ToString();

            var testDateTime = DateTime.Now;
            const string testNameOld = "AuditTest";
            const string testNameNew = "AuditTestNew";

            // ------- Test 1 ------------
            // Arrange 1
            var item = new TestAttributeAuditEntity
            {
                Created = testDateTime,
                Name = testNameOld,
                NotAuditableColumn = testNameOld
            };
            LogInMemorySink.Dispose();

            // Act 1
            await TestBasicDbContext.TestAttributeAudits.AddAsync(item);
            await TestBasicDbContext.SaveChangesAsync();

            // Assert 1
            LogInMemorySink
                .Should()
                .HaveMessage("Missing value in cache:{GetAuditColumnIdAsync}:{keyCache}:{columnName}")
                .Appearing().Times(3)
                .WithProperty("keyCache").WithValues(auditColumnCacheKeyString, auditColumnCacheKeyString, auditColumnCacheKeyString).And
                .WithProperty("columnName").WithValues("Id", "Name", "Created");

            LogInMemorySink
                .Should()
                .HaveMessage("New db value created:{GetAuditColumnIdAsync}:{keyCache}:{missingDbColumnName}")
                .Appearing().Times(3)
                .WithProperty("keyCache").WithValues(auditColumnCacheKeyString, auditColumnCacheKeyString, auditColumnCacheKeyString).And
                .WithProperty("missingDbColumnName").WithValues("Id", "Name", "Created");

            LogInMemorySink
                .Should()
                .HaveMessage("Value saved to cache:{GetAuditColumnIdAsync}:{keyCache}")
                .Appearing().Once()
                .WithProperty("keyCache").WithValue(auditColumnCacheKeyString);

            LogInMemorySink
                .Should()
                .NotHaveMessage("Value from cache:{GetAuditColumnIdAsync}:{keyCache}");

            // ------- Test 2 ------------
            // Arrange 2
            item.Name = testNameNew;
            LogInMemorySink.Dispose();

            // Act 2
            await TestBasicDbContext.SaveChangesAsync();

            // Assert 2
            LogInMemorySink
                .Should()
                .HaveMessage("Value from cache:{GetAuditColumnIdAsync}:{keyCache}")
                .Appearing().Once()
                .WithProperty("keyCache").WithValue(auditColumnCacheKeyString);

            LogInMemorySink
                .Should()
                .NotHaveMessage("Missing value in cache:{GetAuditColumnIdAsync}:{keyCache}:{columnName}");

            LogInMemorySink
                .Should()
                .NotHaveMessage("New db value created:{GetAuditColumnIdAsync}:{keyCache}:{missingDbColumnName}");

            LogInMemorySink
                .Should()
                .NotHaveMessage("Value saved to cache:{GetAuditColumnIdAsync}:{keyCache}");

            // ------- Test 3 ------------
            // Arrange 3
            await Mediator.Send(new CacheRemoveCommand(auditColumnCacheKey));
            item.Name = testNameOld;
            LogInMemorySink.Dispose();

            // Act 3 -!!! Only column Name was changed.
            await TestBasicDbContext.SaveChangesAsync();

            // Assert 3
            LogInMemorySink
                .Should()
                .NotHaveMessage("Value from cache:{GetAuditColumnIdAsync}:{keyCache}");

            // !!! Only column Name was changed.
            LogInMemorySink
                .Should()
                .HaveMessage("Missing value in cache:{GetAuditColumnIdAsync}:{keyCache}:{columnName}")
                .Appearing().Once()
                .WithProperty("keyCache").WithValue(auditColumnCacheKeyString).And
                .WithProperty("columnName").WithValue("Name");

            // All columns are taken from db+
            LogInMemorySink
                .Should()
                .HaveMessage("Value added from DB to cache:{GetAuditColumnIdAsync}:{keyCache}:{ColumnName}")
                .Appearing().Times(3)
                .WithProperty("keyCache").WithValues(auditColumnCacheKeyString, auditColumnCacheKeyString, auditColumnCacheKeyString).And
                .WithProperty("ColumnName").WithValues("Id", "Name", "Created");

            LogInMemorySink
                .Should()
                .NotHaveMessage("New db value created:{GetAuditColumnIdAsync}:{keyCache}:{missingDbColumnName}");

            LogInMemorySink
                .Should()
                .HaveMessage("Value saved to cache:{GetAuditColumnIdAsync}:{keyCache}")
                .Appearing().Once()
                .WithProperty("keyCache").WithValue(auditColumnCacheKeyString);

            // ------- Test 4 ------------
            // Arrange 4
            // Assert delete audit column
            var columnEntities = AuditDbContext.AuditColumns.Where(a => a.ColumnName == "Name").ToList();
            var auditEntities = AuditDbContext.Audits.Where(a => a.AuditTableId == columnEntities.First().AuditTableId).ToList();
            AuditDbContext.Audits.RemoveRange(auditEntities);
            AuditDbContext.AuditColumns.RemoveRange(columnEntities);
            await AuditDbContext.SaveChangesAsync();

            await Mediator.Send(new CacheRemoveCommand(auditColumnCacheKey));
            item.Name = testNameNew;
            LogInMemorySink.Dispose();

            // Act 4
            await TestBasicDbContext.SaveChangesAsync();

            // Assert 4
            LogInMemorySink
                .Should()
                .NotHaveMessage("Value from cache:{GetAuditColumnIdAsync}:{keyCache}");

            // !!! Only column Name was changed.
            LogInMemorySink
                .Should()
                .HaveMessage("Missing value in cache:{GetAuditColumnIdAsync}:{keyCache}:{columnName}")
                .Appearing().Once()
                .WithProperty("keyCache").WithValue(auditColumnCacheKeyString).And
                .WithProperty("columnName").WithValue("Name");

            // All columns are taken from db
            LogInMemorySink
                .Should()
                .HaveMessage("Value added from DB to cache:{GetAuditColumnIdAsync}:{keyCache}:{ColumnName}")
                .Appearing().Times(2)
                .WithProperty("keyCache").WithValues(auditColumnCacheKeyString, auditColumnCacheKeyString).And
                .WithProperty("ColumnName").WithValues("Created", "Id");

            LogInMemorySink
                .Should()
                .HaveMessage("New db value created:{GetAuditColumnIdAsync}:{keyCache}:{missingDbColumnName}")
                .Appearing().Once()
                .WithProperty("missingDbColumnName").WithValue("Name").And
                .WithProperty("keyCache").WithValue(auditColumnCacheKeyString);

            LogInMemorySink
                .Should()
                .HaveMessage("Value saved to cache:{GetAuditColumnIdAsync}:{keyCache}")
                .Appearing().Once()
                .WithProperty("keyCache").WithValue(auditColumnCacheKeyString);
        });
    }
}