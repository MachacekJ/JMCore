using ACore.Modules.CacheModule.CQRS.CacheGet;
using ACore.Modules.CacheModule.CQRS.CacheSave;
using ACore.Modules.CacheModule.CQRS.Models;
using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Modules.AuditModule.Storage.Models;
using ACore.Server.Services.JMCache;
using ACore.Server.Storages.EF;
using ACore.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace ACore.Server.Modules.AuditModule.Storage;

public abstract class AuditSqlStorageImpl(DbContextOptions options, IMediator mediator, ILogger<AuditSqlStorageImpl> logger) : DbContextBase(options, mediator, logger), IAuditStorageModule
{
  private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(60);

  public static JMCacheKey AuditColumnCacheKey(int tableId) => JMCacheKey.Create(JMCacheServerCategory.DbTable, $"{nameof(AuditColumnEntity)}-{tableId}");
  public static JMCacheKey AuditUserCacheKey(string userId) => JMCacheKey.Create(JMCacheServerCategory.DbTable, $"{nameof(AuditUserEntity)}-{userId}");
  public static JMCacheKey AuditTableCacheKey(string tableName, string? schema) => JMCacheKey.Create(JMCacheServerCategory.DbTable, $"{nameof(AuditTableEntity)}-{tableName}-{schema ?? string.Empty}");

  public override DbScriptBase UpdateScripts => new ScriptRegistrations();
  protected override string ModuleName => nameof(IAuditStorageModule);

  public DbSet<AuditEntity> Audits { get; set; }
  public DbSet<AuditColumnEntity> AuditColumns { get; set; }
  public DbSet<AuditUserEntity> AuditUsers { get; set; }
  public DbSet<AuditTableEntity> AuditTables { get; set; }
  public DbSet<AuditValueEntity> AuditValues { get; set; }

  public override Task<T?> Get<T, TU>(TU id) where T : class
  {
    throw new NotImplementedException();
    // if (id == null)
    //   throw new ArgumentNullException($"{nameof(id)} is null.");
    //
    // var res = typeof(T) switch
    // {
    //   { } entityType when entityType == typeof(AuditEntity) => await Audits.FindAsync(Convert.ToInt64(id)) as T,
    //   { } entityType when entityType == typeof(AuditValueEntity) => await AuditValues.FindAsync(Convert.ToInt64(id)) as T,
    //   { } entityType when entityType == typeof(AuditColumnEntity) => await AuditColumns.FindAsync(Convert.ToInt32(id)) as T,
    //   { } entityType when entityType == typeof(AuditUserEntity) => await AuditUsers.FindAsync(Convert.ToInt32(id)) as T,
    //   { } entityType when entityType == typeof(AuditTableEntity) => await AuditTables.FindAsync(Convert.ToInt32(id)) as T,
    //   _ => throw new Exception($"Unknown entity data type {typeof(T).Name} with primary key {id}.")
    // };
    // return res ?? throw new ArgumentNullException(nameof(res), @"Save function returned null value.");
  }
  
  public async Task<IEnumerable<AuditValueEntity>> AuditItemsAsync(string tableName, int pkValue, string? schemaName = null) => await SelectVwAudits(tableName, schemaName).Where(i => i.Audit.PKValue == pkValue).ToArrayAsync();
  public async Task<IEnumerable<AuditValueEntity>> AuditItemsAsync(string tableName, string pkValue, string? schemaName = null) => await SelectVwAudits(tableName, schemaName).Where(i => i.Audit.PKValueString == pkValue).ToArrayAsync();
  public async Task<IEnumerable<AuditValueEntity>> AllTableAuditAsync(string tableName, string? schemaName = null) => await SelectVwAudits(tableName, schemaName).ToArrayAsync();

  public async Task SaveAuditAsync(AuditEntryItem auditEntryItem)
  {
    var valuesTable = new List<AuditSqlValueItem>();
    foreach (var oldValue in auditEntryItem.OldValues)
    {
      auditEntryItem.NewValues.TryGetValue(oldValue.Key, out var newValue);
      var auditValue = AuditSqlValueItem.CreateValue(Logger, oldValue.Key, oldValue.Value, newValue);
      if (auditValue != null)
        valuesTable.Add(auditValue);
    }

    foreach (var newValue in auditEntryItem.NewValues.Where(kv => !auditEntryItem.OldValues.ContainsKey(kv.Key)))
    {
      var auditValue = AuditSqlValueItem.CreateValue(Logger, newValue.Key, null, newValue.Value);
      if (auditValue != null)
        valuesTable.Add(auditValue);
    }

    var auditTableId = await GetAuditTableIdAsync(auditEntryItem.TableName, auditEntryItem.SchemaName);
    var auditColumnIds = await GetAuditColumnIdAsync(auditTableId, valuesTable.Select(a => a.AuditColumnName).ToList());

    var auditEntity = new AuditEntity
    {
      AuditTableId = auditTableId,
      PKValue = auditEntryItem.PkValue,
      PKValueString = auditEntryItem.PkValueString,
      AuditUserId = await GetAuditUserIdAsync(auditEntryItem.ByUser.userId, auditEntryItem.ByUser.userName),
      DateTime = DateTime.UtcNow,
      EntityState = auditEntryItem.EntityState,
      AuditValues = new ObservableCollectionListSource<AuditValueEntity>()
    };

    foreach (var value in valuesTable)
    {
      var valueEntity = new AuditValueEntity
      {
        AuditColumnId = auditColumnIds[value.AuditColumnName]
      };
      valueEntity.CopyPropertiesFrom(value);
      auditEntity.AuditValues.Add(valueEntity);
    }

    await Audits.AddAsync(auditEntity);
    await SaveChangesAsync();
    
    // await SaveInternal<AuditEntity, long>(auditEntity, auditEntity.Id,
    //   async (a) => await Audits.AddAsync(auditEntity),
    //   (i) => i.Id = IdIntGenerator<AuditEntity>());
  }

  private async Task<int> GetAuditUserIdAsync(string userId, string userName)
  {
    var keyCache = AuditUserCacheKey(userId);

    var cacheValue = await Mediator.Send(new CacheModuleGetQuery(keyCache));
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

    await Mediator.Send(new CacheModuleSaveCommand(keyCache, userEntity, _cacheDuration));
    // This message is also used in unit test.
    Logger.LogDebug("Value saved to cache:{GetAuditUserIdAsync}:{keyCache}:{userId}", nameof(GetAuditUserIdAsync), keyCache, userId);
    return userEntity.Id;
  }

  public async Task<int> GetAuditTableIdAsync(string tableName, string? tableSchema)
  {
    var keyCache = AuditTableCacheKey(tableName, tableSchema);

    var cacheValue = await Mediator.Send(new CacheModuleGetQuery(keyCache));
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

    await Mediator.Send(new CacheModuleSaveCommand(keyCache, tableEntity, _cacheDuration));
    // This message is also used in unit test.
    Logger.LogDebug("Value saved to cache:{GetAuditTableIdAsync}:{keyCache}:{tableName}:{tableSchema}", nameof(GetAuditTableIdAsync), keyCache, tableName, tableSchema);
    return tableEntity.Id;
  }

  private async Task<Dictionary<string, int>> GetAuditColumnIdAsync(int tableId, List<string> columnNames)
  {
    var keyCache = AuditColumnCacheKey(tableId);
    var res = new Dictionary<string, int>();

    var cacheValue = await Mediator.Send(new CacheModuleGetQuery(keyCache));
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

    await Mediator.Send(new CacheModuleSaveCommand(keyCache, res, _cacheDuration));
    // This message is also used in unit test.
    Logger.LogDebug("Value saved to cache:{GetAuditColumnIdAsync}:{keyCache}", nameof(GetAuditColumnIdAsync), keyCache);

    return res;
  }

  private IQueryable<AuditValueEntity> SelectVwAudits(string tableName, string? schemaName)
  {
    return AuditValues
      .Include(a => a.Audit)
      .Include(a => a.AuditColumn)
      .Include(a => a.Audit.AuditTable)
      .Include(a => a.Audit.User).Where(i => i.Audit.AuditTable.TableName == tableName && i.Audit.AuditTable.SchemaName == schemaName).Select(a => a);
  }
}