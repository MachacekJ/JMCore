using System.Reflection;
using ACore.Base.Cache;
using ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheRemove;
using ACore.Server;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Save;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using Serilog.Sinks.InMemory.Assertions;
using Xunit;

namespace ACore.Tests.Server.Modules.AuditModule;

/// <summary>
/// Check if cache for audit module works - table name etc.
/// Test is depending on log items. It can be tricky.
/// var serilog = new LoggerConfiguration().MinimumLevel.Debug()
/// </summary>
public class AuditCacheTests : AuditTestsBase
{
  private static readonly DateTime TestDateTime = DateTime.UtcNow;
  private const string TestName = "AuditTest";
  private const string AuditUserEntityName = "AuditUserEntity";
  private const string AuditTableEntityName = "AuditTableEntity";
  private const string AuditColumnEntityName = "AuditColumnEntity";

  private static CacheKey AuditUserCacheKey(string userId) => CacheKey.Create(CacheCategories.Entity, new CacheCategory(AuditUserEntityName), userId);
  private static CacheKey AuditTableCacheKey(string tableName, string? schema, int version) => CacheKey.Create(CacheCategories.Entity, new CacheCategory(AuditTableEntityName), $"{tableName}-{schema ?? string.Empty}--{version}");
  private static CacheKey AuditColumnCacheKey(int tableId) => CacheKey.Create(CacheCategories.Entity, new CacheCategory(AuditColumnEntityName), tableId.ToString());


  [Fact]
  public async Task UserCache()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      const string testNameOld = "AuditTest";
      const string testNameNew = "AuditTestNew";

      // Arrange 1
      var item = new TestAuditData<int>
      {
        Created = TestDateTime,
        Name = TestName,
        NotAuditableColumn = "Audit"
      };
      (UserProvider as TestAuditUserProvider)!.SetContext(TestAuditUserTypeEnum.Admin);
      var userId = UserProvider.GetUser().userId;
      var auditUserCacheKey = AuditUserCacheKey(userId);

      // Act 1
      LogInMemorySink.Dispose();
      // Action.
      await Mediator.Send(new TestAuditSaveCommand<int>(item));

      // Assert 1
      LogInMemorySink
        .Should()
        .HaveMessage("Value saved to cache:{GetAuditUserIdAsync}:{keyCache}:{userId}")
        .Appearing().Once()
        .WithProperty("keyCache").WithValue(auditUserCacheKey).And
        .WithProperty("userId").WithValue(userId);

      LogInMemorySink
        .Should()
        .HaveMessage("New db value created:{GetAuditUserIdAsync}:{keyCache}:{userId}")
        .Appearing().Once()
        .WithProperty("keyCache").WithValue(auditUserCacheKey).And
        .WithProperty("userId").WithValue(userId);

      // Arrange 2
      item.Name = testNameNew;

      // Act 2
      await Mediator.Send(new TestAuditSaveCommand<int>(item));

      // Assert 2
      LogInMemorySink
        .Should()
        .HaveMessage("Value from cache:{GetAuditUserIdAsync}:{keyCache}:{userId}")
        .Appearing().Once()
        .WithProperty("keyCache").WithValue(auditUserCacheKey).And
        .WithProperty("userId").WithValue(userId);

      LogInMemorySink
        .Should()
        .HaveMessage("New db value created:{GetAuditUserIdAsync}:{keyCache}:{userId}")
        .Appearing().Once()
        .WithProperty("keyCache").WithValue(auditUserCacheKey).And
        .WithProperty("userId").WithValue(userId);

      LogInMemorySink
        .Should()
        .HaveMessage("Value saved to cache:{GetAuditUserIdAsync}:{keyCache}:{userId}")
        .Appearing().Once()
        .WithProperty("keyCache").WithValue(auditUserCacheKey).And
        .WithProperty("userId").WithValue(userId);

      // Arrange 3
      await Mediator.Send(new MemoryCacheModuleRemoveKeyCommand(auditUserCacheKey));
      item.Name = testNameOld;

      // Act 3
      await Mediator.Send(new TestAuditSaveCommand<int>(item));

      // Assert 3
      LogInMemorySink
        .Should()
        .HaveMessage("Value from cache:{GetAuditUserIdAsync}:{keyCache}:{userId}")
        .Appearing().Once()
        .WithProperty("keyCache").WithValue(auditUserCacheKey).And
        .WithProperty("userId").WithValue(userId);

      LogInMemorySink
        .Should()
        .HaveMessage("New db value created:{GetAuditUserIdAsync}:{keyCache}:{userId}")
        .Appearing().Once()
        .WithProperty("keyCache").WithValue(auditUserCacheKey).And
        .WithProperty("userId").WithValue(userId);
      LogInMemorySink
        .Should()
        .HaveMessage("Value saved to cache:{GetAuditUserIdAsync}:{keyCache}:{userId}")
        .Appearing().Times(2)
        .WithProperty("keyCache").WithValues(auditUserCacheKey, auditUserCacheKey).And
        .WithProperty("userId").WithValues(userId, userId);
    });
  }

  [Fact]
  public async Task TableCache()
  {
    var tableName = nameof(TestAuditEntity);

    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      var auditTableCacheKey = AuditTableCacheKey(tableName, null, 1);
      var auditTableCacheKeyString = auditTableCacheKey.ToString();
      const string testNameOld = "AuditTest";
      const string testNameNew = "AuditTestNew";

      // Arrange 1
      var item = new TestAuditData<int>
      {
        Created = TestDateTime,
        Name = TestName,
        NotAuditableColumn = "Audit"
      };

      // Act 1
      LogInMemorySink.Dispose();

      await Mediator.Send(new TestAuditSaveCommand<int>(item));

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
      await Mediator.Send(new TestAuditSaveCommand<int>(item));

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
      await Mediator.Send(new MemoryCacheModuleRemoveKeyCommand(auditTableCacheKey));
      item.Name = testNameOld;

      // Act 3
      await Mediator.Send(new TestAuditSaveCommand<int>(item));

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
      var auditTableId = 1;
      var auditColumnCacheKey = AuditColumnCacheKey(auditTableId);
      var auditColumnCacheKeyString = auditColumnCacheKey.ToString();
      
      const string testNameNew = "AuditTestNew";

      // ------- Test 1 ------------
      // Arrange 1
      var item = new TestAuditData<int>
      {
        Created = TestDateTime,
        Name = TestName,
        NotAuditableColumn = "Audit"
      };
      LogInMemorySink.Dispose();

      // Act 1
      await Mediator.Send(new TestAuditSaveCommand<int>(item));

      // Assert 1
      LogInMemorySink
        .Should()
        .HaveMessage("Missing value in cache:{GetAuditColumnIdAsync}:{keyCache}:{columnName}")
        .Appearing().Times(6)
        .WithProperty("keyCache").WithValues(auditColumnCacheKeyString, auditColumnCacheKeyString, auditColumnCacheKeyString,
          auditColumnCacheKeyString, auditColumnCacheKeyString, auditColumnCacheKeyString).And
        .WithProperty("columnName").WithValues(
          nameof(TestAuditEntity.Name),
          nameof(TestAuditEntity.NullValue),
          nameof(TestAuditEntity.NullValue2),
          nameof(TestAuditEntity.NullValue3),
          nameof(TestAuditEntity.Created),
          nameof(TestAuditEntity.Id));

      LogInMemorySink
        .Should()
        .HaveMessage("New db value created:{GetAuditColumnIdAsync}:{keyCache}:{missingDbColumnName}")
        .Appearing().Times(6)
        .WithProperty("keyCache").WithValues(auditColumnCacheKeyString, auditColumnCacheKeyString, 
          auditColumnCacheKeyString, auditColumnCacheKeyString, auditColumnCacheKeyString, auditColumnCacheKeyString).And
        .WithProperty("missingDbColumnName").WithValues(nameof(TestAuditEntity.Id), nameof(TestAuditEntity.Name), nameof(TestAuditEntity.Created),
          nameof(TestAuditEntity.NullValue), nameof(TestAuditEntity.NullValue2), nameof(TestAuditEntity.NullValue3));

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
      await Mediator.Send(new TestAuditSaveCommand<int>(item));

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
    });
  }
}