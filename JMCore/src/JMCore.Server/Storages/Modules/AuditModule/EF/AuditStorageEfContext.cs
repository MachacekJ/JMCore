using JMCore.CQRS.JMCache.CacheGet;
using JMCore.CQRS.JMCache.CacheSave;
using JMCore.Server.Services.JMCache;
using JMCore.Server.Storages.Base.Audit.Models;
using JMCore.Server.Storages.Base.EF;
using JMCore.Server.Storages.Modules.AuditModule.Models;
using JMCore.Services.JMCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace JMCore.Server.Storages.Modules.AuditModule.EF;

public abstract class AuditStorageEfContext(DbContextOptions options, IMediator mediator, ILogger<AuditStorageEfContext> logger) : DbContextBase(options, mediator, logger), IAuditStorageModule
{
  public const int MaxStringSize = 10000;

  private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(60);

  public override DbScriptBase UpdateScripts => new ScriptRegistrations();
  protected override string ModuleName => nameof(IAuditStorageModule);
  
  public DbSet<AuditEntity> Audits { get; set; }
  public DbSet<AuditColumnEntity> AuditColumns { get; set; }
  public DbSet<AuditUserEntity> AuditUsers { get; set; }
  public DbSet<AuditTableEntity> AuditTables { get; set; }
  public DbSet<AuditValueEntity> AuditValues { get; set; }

  public async Task<IEnumerable<AuditVwAuditEntity>> AuditItemsAsync(string tableName, int pkValue, string? schemaName = null) => await SelectVwAudits().Where(i => i.TableName == tableName && i.PKValue == pkValue && i.SchemaName == schemaName).ToArrayAsync();
  public async Task<IEnumerable<AuditVwAuditEntity>> AuditItemsAsync(string tableName, string pkValue, string? schemaName = null) => await SelectVwAudits().Where(i => i.TableName == tableName && i.PKValueString == pkValue && i.SchemaName == schemaName).ToArrayAsync();
  public async Task<IEnumerable<AuditVwAuditEntity>> AllAuditItemsAsync(string tableName, string? schemaName = null) => await SelectVwAudits().Where(i => i.TableName == tableName && i.SchemaName == schemaName).ToArrayAsync();
  
  public async Task SaveAuditAsync(AuditEntry auditEntry)
  {
    var valuesTable = new List<AuditValue>();
    foreach (var oldValue in auditEntry.OldValues)
    {
      auditEntry.NewValues.TryGetValue(oldValue.Key, out var newValue);
      var auditValue = AuditValue.Value(Logger, oldValue.Key, oldValue.Value, newValue);
      if (auditValue != null)
        valuesTable.Add(auditValue);
    }

    foreach (var newValue in auditEntry.NewValues.Where(kv => !auditEntry.OldValues.ContainsKey(kv.Key)))
    {
      var auditValue = AuditValue.Value(Logger, newValue.Key, null, newValue.Value);
      if (auditValue != null)
        valuesTable.Add(auditValue);
    }

    var auditTableId = await GetAuditTableIdAsync(auditEntry.TableName, auditEntry.SchemaName);
    var auditColumnIds = await GetAuditColumnIdAsync(auditTableId, valuesTable.Select(a => a.AuditColumnName).ToList());

    var auditEntity = new AuditEntity
    {
      AuditTableId = auditTableId,
      PKValue = auditEntry.PkValue,
      PKValueString = auditEntry.PkValueString,
      AuditUserId = await GetAuditUserIdAsync(auditEntry.ByUser.userId, auditEntry.ByUser.userName),
      DateTime = DateTime.UtcNow,
      EntityState = auditEntry.EntityState,
      AuditValues = new ObservableCollectionListSource<AuditValueEntity>()
    };

    foreach (var value in valuesTable)
    {
      auditEntity.AuditValues.Add(new AuditValueEntity
      {
        AuditColumnId = auditColumnIds[value.AuditColumnName],
        OldValueString = value.OldValueString,
        NewValueString = value.NewValueString,
        OldValueInt = value.OldValueInt,
        NewValueInt = value.NewValueInt,
        OldValueLong = value.OldValueLong,
        NewValueLong = value.NewValueLong,
        OldValueBool = value.OldValueBool,
        NewValueBool = value.NewValueBool,
        OldValueGuid = value.OldValueGuid,
        NewValueGuid = value.NewValueGuid
      });
    }

    await Audits.AddAsync(auditEntity);
    await SaveChangesAsync();
  }

  public JMCacheKey AuditColumnCacheKey(int tableId) => JMCacheKey.Create(JMCacheServerCategory.DbTable, $"{nameof(AuditColumnEntity)}-{tableId}");
  public JMCacheKey AuditUserCacheKey(string userId) => JMCacheKey.Create(JMCacheServerCategory.DbTable, $"{nameof(AuditUserEntity)}-{userId}");
  public JMCacheKey AuditTableCacheKey(string tableName, string? schema) => JMCacheKey.Create(JMCacheServerCategory.DbTable, $"{nameof(AuditTableEntity)}-{tableName}-{schema ?? string.Empty}");

  public async Task<int> GetAuditUserIdAsync(string userId, string userName)
  {
    var keyCache = AuditUserCacheKey(userId);

    var cacheValue = await Mediator.Send(new CacheGetQuery(keyCache));
    if (cacheValue?.Value != null)
    {
      // This message is also used in unit test.
      Logger.LogDebug("Value from cache:{GetAuditUserIdAsync}:{keyCache}:{userId}", nameof(GetAuditUserIdAsync), keyCache, userId);
      return ((AuditUserEntity)cacheValue.Value).Id;
    }

    var userEntity = await AuditUsers.FirstOrDefaultAsync(u => u.UserId == userId);
    if (userEntity == null)
    {
      userEntity = new AuditUserEntity
      {
        UserId = userId,
        UserName = userName
      };
      await AuditUsers.AddAsync(userEntity);
      await SaveChangesAsync();
      // This message is also used in unit test.
      Logger.LogDebug("New db value created:{GetAuditUserIdAsync}:{keyCache}:{userId}", nameof(GetAuditUserIdAsync), keyCache, userId);
    }

    await Mediator.Send(new CacheSaveCommand(keyCache, userEntity, _cacheDuration));
    // This message is also used in unit test.
    Logger.LogDebug("Value saved to cache:{GetAuditUserIdAsync}:{keyCache}:{userId}", nameof(GetAuditUserIdAsync), keyCache, userId);
    return userEntity.Id;
  }

  public async Task<int> GetAuditTableIdAsync(string tableName, string? tableSchema)
  {
    var keyCache = AuditTableCacheKey(tableName, tableSchema);

    var cacheValue = await Mediator.Send(new CacheGetQuery(keyCache));
    if (cacheValue?.Value != null)
    {
      // This message is also used in unit test.
      Logger.LogDebug("Value from cache:{GetAuditTableIdAsync}:{keyCache}:{tableName}:{tableSchema}", nameof(GetAuditTableIdAsync), keyCache, tableName, tableSchema);
      return ((AuditTableEntity)cacheValue.Value).Id;
    }

    var tableEntity = await AuditTables.FirstOrDefaultAsync(u => u.TableName == tableName && u.SchemaName == tableSchema);
    if (tableEntity == null)
    {
      tableEntity = new AuditTableEntity
      {
        TableName = tableName,
        SchemaName = tableSchema
      };

      await AuditTables.AddAsync(tableEntity);
      await SaveChangesAsync();
      // This message is also used in unit test.
      Logger.LogDebug("New db value created:{GetAuditTableIdAsync}:{keyCache}:{tableName}:{tableSchema}", nameof(GetAuditTableIdAsync), keyCache, tableName, tableSchema);
    }

    await Mediator.Send(new CacheSaveCommand(keyCache, tableEntity, _cacheDuration));
    // This message is also used in unit test.
    Logger.LogDebug("Value saved to cache:{GetAuditTableIdAsync}:{keyCache}:{tableName}:{tableSchema}", nameof(GetAuditTableIdAsync), keyCache, tableName, tableSchema);
    return tableEntity.Id;
  }

  public async Task<Dictionary<string, int>> GetAuditColumnIdAsync(int tableId, List<string> columnNames)
  {
    var keyCache = AuditColumnCacheKey(tableId);
    var res = new Dictionary<string, int>();

    var cacheValue = await Mediator.Send(new CacheGetQuery(keyCache));
    if (cacheValue?.Value != null)
    {
      // This message is also used in unit test.
      Logger.LogDebug("Value from cache:{GetAuditColumnIdAsync}:{keyCache}", nameof(GetAuditColumnIdAsync), keyCache);
      res = (Dictionary<string, int>)cacheValue.Value;
    }

    // Check if I have all columns from cache.
    var missingColumnNamesInCache = new List<string>();
    foreach (var columnName in columnNames)
    {
      if (res.ContainsKey(columnName))
        continue;

      missingColumnNamesInCache.Add(columnName);
      // This message is also used in unit test.
      Logger.LogDebug("Missing value in cache:{GetAuditColumnIdAsync}:{keyCache}:{columnName}",
        nameof(GetAuditColumnIdAsync), keyCache, columnName);
    }

    if (missingColumnNamesInCache.Count == 0)
      return res; //All columns ids are saved in cache

    // I haven't a column in cache. Trying to get from DB.
    var missingDbColumns = new List<string>();
    var dbColumnEntitiesForTable = await AuditColumns.Where(a => a.AuditTableId == tableId).ToListAsync();

    // Refresh response according to db.
    res.Clear();
    if (dbColumnEntitiesForTable.Count > 0)
    {
      foreach (var auditColumnEntity in dbColumnEntitiesForTable)
      {
        res.Add(auditColumnEntity.ColumnName, auditColumnEntity.Id);
        // This message is also used in unit test.
        Logger.LogDebug("Value added from DB to cache:{GetAuditColumnIdAsync}:{keyCache}:{ColumnName}"
          , nameof(GetAuditColumnIdAsync), keyCache, auditColumnEntity.ColumnName);
      }

      foreach (var missingDbColumn in missingColumnNamesInCache)
      {
        // New audit column is appeared for exiting table, this column is not saved in db yet.
        if (dbColumnEntitiesForTable.SingleOrDefault(a => a.ColumnName == missingDbColumn) == null)
          missingDbColumns.Add(missingDbColumn);
      }
    }
    else
    {
      // New audit table is appeared.
      missingDbColumns.AddRange(missingColumnNamesInCache);
    }

    if (missingDbColumns.Count > 0)
    {
      // I haven't a column in db. The columns must be saved in DB.
      List<AuditColumnEntity> newDbColumnEntities = new List<AuditColumnEntity>();
      foreach (var missingDbColumnName in missingDbColumns)
      {
        var columnEntity = new AuditColumnEntity
        {
          ColumnName = missingDbColumnName,
          AuditTableId = tableId
        };
        newDbColumnEntities.Add(columnEntity);
        await AuditColumns.AddAsync(columnEntity);
        // This message is also used in unit test.
        Logger.LogDebug("New db value created:{GetAuditColumnIdAsync}:{keyCache}:{missingDbColumnName}",
          nameof(GetAuditColumnIdAsync), keyCache, missingDbColumnName);
      }

      await SaveChangesAsync();

      // I must add columns id to response.
      foreach (var savedColumnEntity in newDbColumnEntities)
      {
        res.Add(savedColumnEntity.ColumnName, savedColumnEntity.Id);
      }
    }

    await Mediator.Send(new CacheSaveCommand(keyCache, res, _cacheDuration));
    // This message is also used in unit test.
    Logger.LogDebug("Value saved to cache:{GetAuditColumnIdAsync}:{keyCache}", nameof(GetAuditColumnIdAsync), keyCache);

    return res;
  }

  private IQueryable<AuditVwAuditEntity> SelectVwAudits()
  {
    return AuditValues
      .Include(a => a.Audit)
      .Include(a => a.AuditColumn)
      .Include(a => a.Audit.AuditTable)
      .Include(a => a.Audit.User)
      .Select(a => new AuditVwAuditEntity()
      {
        Id = a.Id,
        AuditId = a.AuditId,
        TableName = a.Audit.AuditTable.TableName,
        SchemaName = a.Audit.AuditTable.SchemaName,
        PKValue = a.Audit.PKValue,
        PKValueString = a.Audit.PKValueString,
        UserName = a.Audit.User.UserName,
        DateTime = a.Audit.DateTime,
        EntityState = a.Audit.EntityState,
        ColumnName = a.AuditColumn.ColumnName,
        OldValueString = a.OldValueString,
        NewValueString = a.NewValueString,
        OldValueInt = a.OldValueInt,
        NewValueInt = a.NewValueInt,
        OldValueLong = a.OldValueLong,
        NewValueLong = a.NewValueLong,
        OldValueBool = a.OldValueBool,
        NewValueBool = a.NewValueBool,
        OldValueGuid = a.OldValueGuid,
        NewValueGuid = a.NewValueGuid
      });
  }

  private class AuditValue
  {
    private readonly ILogger<DbContextBase> _logger;
    public string AuditColumnName { get; }
    public string? OldValueString { get; }
    public string? NewValueString { get; }
    public int? OldValueInt { get; }
    public int? NewValueInt { get; }
    public long? OldValueLong { get; }
    public long? NewValueLong { get; }
    public bool? OldValueBool { get; }
    public bool? NewValueBool { get; }

    public Guid? OldValueGuid { get; set; }
    public Guid? NewValueGuid { get; set; }

    private AuditValue(ILogger<DbContextBase> logger, string columnName, object? oldValue, object? newValue)
    {
      _logger = logger;

      AuditColumnName = columnName;
      if (oldValue != null)
      {
        switch (oldValue)
        {
          case byte b:
            OldValueInt = b;
            break;
          case short s:
            OldValueInt = s;
            break;
          case int i:
            OldValueInt = i;
            break;
          case long l:
            OldValueLong = l;
            break;
          case bool bl:
            OldValueBool = bl;
            break;
          case Guid g:
            OldValueGuid = g;
            break;
          case DateTime dt:
            OldValueLong = dt.Ticks;
            break;
          case TimeSpan span:
            OldValueLong = span.Ticks;
            break;
          case string st:
            OldValueString = st;
            break;
          default:
            OldValueString = ToValueString(oldValue);
            break;
        }
      }

      if (newValue == null)
        return;

      switch (newValue)
      {
        case byte b:
          NewValueInt = b;
          break;
        case short s:
          NewValueInt = s;
          break;
        case int i:
          NewValueInt = i;
          break;
        case long l:
          NewValueLong = l;
          break;
        case bool bl:
          NewValueBool = bl;
          break;
        case Guid g:
          NewValueGuid = g;
          break;
        case DateTime dt:
          NewValueLong = dt.Ticks;
          break;
        case TimeSpan span:
          NewValueLong = span.Ticks;
          break;
        case string st:
          NewValueString = st;
          break;
        default:
          NewValueString = ToValueString(newValue);
          break;
      }
    }

    public static AuditValue? Value(ILogger<DbContextBase> logger, string columnName, object? oldValue, object? newValue)
    {
      var value = new AuditValue(logger, columnName, oldValue, newValue);
      if (value.OldValueInt == value.NewValueInt &&
          value.OldValueLong == value.NewValueLong &&
          value.OldValueBool == value.NewValueBool &&
          value.OldValueString == value.NewValueString &&
          value.OldValueGuid == value.NewValueGuid)
        return null;

      return value;
    }

    private string ToValueString(object value)
    {
      var valueString = Newtonsoft.Json.JsonConvert.SerializeObject(value);

      switch (value)
      {
        case decimal:
        case byte[]:
          break;
        default:
          throw new Exception($"Unknown type for audit. Type: {value.GetType()}; Value:{valueString}");
      }

      if (valueString.Length > MaxStringSize)
        // This message is used in unit test.
        _logger.LogError("The value exceeded the maximum character length '{MaxStringSize}'. Value:{Value}", MaxStringSize, valueString);

      return valueString;
    }
  }
}