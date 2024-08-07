using System.Reflection;
using ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit;
using ACore.AppTest.Modules.TestModule.Models;
using ACore.Modules.CacheModule.CQRS.CacheRemove;
using ACore.Server.Modules.AuditModule.Storage;
using FluentAssertions;
using Serilog.Sinks.InMemory.Assertions;
using Xunit;

namespace ACore.Tests.Server.Modules.TestModule;

/// <summary>
/// Check if cache for audit module works - table name etc.
/// Test is depending on log items. It can be tricky.
/// var serilog = new LoggerConfiguration().MinimumLevel.Debug()
/// </summary>
public class AuditCacheTests : AuditAttributeBaseTests
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
      var item = new TestAttributeAuditData
      {
        Created = testDateTime,
        Name = testNameOld,
        NotAuditableColumn = testNameOld
      };
      (UserProvider as TestAuditUserProvider)!.SetContext(TestAuditUserTypeEnum.Admin);
      var userId = UserProvider.GetUser().userId;
      var auditUserCacheKey = AuditSqlStorageImpl.AuditUserCacheKey(userId);
      var auditUserCacheKeyString = auditUserCacheKey.ToString();

      // Act 1
      LogInMemorySink.Dispose();
      var res = await Mediator.Send(new TestAttributeAuditSaveCommand(item));
      res.Should().BeGreaterThan(0);


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
      var res2 = await Mediator.Send(new TestAttributeAuditSaveCommand(item));
      res.Should().BeGreaterThan(0);

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
      await Mediator.Send(new CacheModuleRemoveCommand(auditUserCacheKey));
      item.Name = testNameOld;

      // Act 3
      var res3 = await Mediator.Send(new TestAttributeAuditSaveCommand(item));
      res.Should().BeGreaterThan(0);

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
    var tableName = "TestAttributeAuditEntity";
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      var auditTableCacheKey = AuditSqlStorageImpl.AuditTableCacheKey(tableName, null);
      var auditTableCacheKeyString = auditTableCacheKey.ToString();
      var testDateTime = DateTime.Now;
      const string testNameOld = "AuditTest";
      const string testNameNew = "AuditTestNew";

      // Arrange 1
      var item = new TestAttributeAuditData
      {
        Created = testDateTime,
        Name = testNameOld,
        NotAuditableColumn = testNameOld
      };

      // Act 1
      LogInMemorySink.Dispose();
      var res = await Mediator.Send(new TestAttributeAuditSaveCommand(item));
      res.Should().BeGreaterThan(0);

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
      var res2 = await Mediator.Send(new TestAttributeAuditSaveCommand(item));
      res2.Should().Be(res);

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
      await Mediator.Send(new CacheModuleRemoveCommand(auditTableCacheKey));
      item.Name = testNameOld;

      // Act 3
      var res3 = await Mediator.Send(new TestAttributeAuditSaveCommand(item));
      res3.Should().Be(res);

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
    var tableName = "TestAttributeAuditEntity";
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
     // var auditSqlStorageImpl = AuditStorageModule as AuditSqlStorageImpl ?? throw new ArgumentNullException($"{nameof(AuditStorageModule)} doesn't implement {nameof(AuditSqlStorageImpl)}'");
     var auditTableId = 1;// await auditSqlStorageImpl.GetAuditTableIdAsync(tableName, null);
      var auditColumnCacheKey = AuditSqlStorageImpl.AuditColumnCacheKey(auditTableId);
      var auditColumnCacheKeyString = auditColumnCacheKey.ToString();

      var testDateTime = DateTime.Now;
      const string testNameOld = "AuditTest";
      const string testNameNew = "AuditTestNew";

      // ------- Test 1 ------------
      // Arrange 1
      var item = new TestAttributeAuditData
      {
        Created = testDateTime,
        Name = testNameOld,
        NotAuditableColumn = testNameOld
      };
      LogInMemorySink.Dispose();

      // Act 1
      var res = await Mediator.Send(new TestAttributeAuditSaveCommand(item));
      res.Should().BeGreaterThan(0);

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
      var res2 = await Mediator.Send(new TestAttributeAuditSaveCommand(item));
      res2.Should().Be(res);

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
      await Mediator.Send(new CacheModuleRemoveCommand(auditColumnCacheKey));
      item.Name = testNameOld;
      LogInMemorySink.Dispose();

      // Act 3 -!!! Only column Name was changed.
      var res3 = await Mediator.Send(new TestAttributeAuditSaveCommand(item));
      res3.Should().Be(res);

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
      // var columnEntities = auditSqlStorageImpl.AuditColumns.Where(a => a.ColumnName == "Name").ToList();
      // var auditEntities = auditSqlStorageImpl.Audits.Where(a => a.AuditTableId == columnEntities.First().AuditTableId).ToList();
      // auditSqlStorageImpl.Audits.RemoveRange(auditEntities);
      // auditSqlStorageImpl.AuditColumns.RemoveRange(columnEntities);
      // await auditSqlStorageImpl.SaveChangesAsync();

      await Mediator.Send(new CacheModuleRemoveCommand(auditColumnCacheKey));
      item.Name = testNameNew;
      LogInMemorySink.Dispose();

      // Act 4
      var res4 = await Mediator.Send(new TestAttributeAuditSaveCommand(item));
      res4.Should().Be(res);

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