using ACore.Base.Cache;
using ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheGet;
using ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheSave;
using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Modules.AuditModule.Storage.SQL.Models;
using ACore.Server.Storages;
using ACore.Server.Storages.Definitions.EF.Base;
using ACore.Server.Storages.Definitions.EF.Base.Scripts;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace ACore.Server.Modules.AuditModule.Storage.SQL;

internal abstract class AuditSqlStorageImpl(DbContextOptions options, IMediator mediator, ILogger<AuditSqlStorageImpl> logger)
  : DbContextBase(options, mediator, logger), IAuditStorageModule
{
  private static CacheKey AuditColumnCacheKey(int tableId) => CacheKey.Create(CacheCategories.Entity, new CacheCategory(nameof(AuditColumnEntity)), tableId.ToString());
  private static CacheKey AuditUserCacheKey(string userId) => CacheKey.Create(CacheCategories.Entity, new CacheCategory(nameof(AuditUserEntity)), userId);
  private static CacheKey AuditTableCacheKey(string tableName, string? schema, int version) => CacheKey.Create(CacheCategories.Entity, new CacheCategory(nameof(AuditTableEntity)), $"{tableName}-{schema ?? string.Empty}--{version}");

  protected override DbScriptBase UpdateScripts => new ScriptRegistrations();
  protected override string ModuleName => nameof(IAuditStorageModule);

  public DbSet<AuditEntity> Audits { get; set; }
  internal DbSet<AuditColumnEntity> AuditColumns { get; set; }
  public DbSet<AuditUserEntity> AuditUsers { get; set; }
  public DbSet<AuditTableEntity> AuditTables { get; set; }
  public DbSet<AuditValueEntity> AuditValues { get; set; }

  public async Task<AuditEntryItem[]> AuditItemsAsync<T>(string tableName, T pkValue, string? schemaName = null)
  {
    AuditValueEntity[] auditValueEntities;
    if (typeof(T) == typeof(int) || typeof(T) == typeof(long))
      auditValueEntities = await SelectVwAudits(tableName, schemaName).Where(i => i.Audit.PKValue == Convert.ToInt64(pkValue)).ToArrayAsync();
    else
      auditValueEntities = await SelectVwAudits(tableName, schemaName).Where(i => i.Audit.PKValueString == Convert.ToString(pkValue)).ToArrayAsync();

    var res = new List<AuditEntryItem>();
    foreach (var grItem in auditValueEntities.GroupBy(e =>
               new
               {
                 tableName = e.Audit.AuditTable.TableName,
                 schemaName = e.Audit.AuditTable.SchemaName,
                 version = e.Audit.AuditTable.Version,
                 pk = e.Audit.PKValue,
                 pkString = e.Audit.PKValueString,
                 entityState = e.Audit.EntityState,
                 user = e.Audit.User.UserId,
                 created = e.Audit.DateTime
               }))
    {
      var primaryKeyValue = (grItem.Key.pk == null) ? grItem.Key.pkString : grItem.Key.pk as object;
      if (primaryKeyValue == null)
        throw new Exception("Primary key is null");

      var auditEntryItem = new AuditEntryItem(grItem.Key.tableName, grItem.Key.schemaName, grItem.Key.version, primaryKeyValue, grItem.Key.entityState, grItem.Key.user)
      {
        Created = grItem.Key.created
      };

      foreach (var col in grItem.ToArray())
      {
        auditEntryItem.AddColumnEntry(col.AuditColumn.ColumnName, col.DataType, col.IsChanged, col.GetOldValueObject(), col.GetNewValueObject());
      }

      res.Add(auditEntryItem);
    }

    return res.ToArray();
  }

  public async Task SaveAuditAsync(AuditEntryItem auditEntryItem)
  {
    var valuesTable = auditEntryItem.ChangedColumns
      .Select(change => AuditSqlValueItem.CreateValue(Logger, change))
      .OfType<AuditSqlValueItem>().ToList();

    var auditTableId = await GetAuditTableIdAsync(auditEntryItem.TableName, auditEntryItem.SchemaName, auditEntryItem.Version);
    var auditColumnIds = await GetAuditColumnIdAsync(auditTableId, valuesTable
      .ToDictionary(k => k.AuditColumnName, v => v.DataType));

    var auditEntity = new AuditEntity
    {
      AuditTableId = auditTableId,
      PKValue = auditEntryItem.PkValue,
      PKValueString = auditEntryItem.PkValueString,
      AuditUserId = await GetAuditUserIdAsync(auditEntryItem.UserId),
      DateTime = DateTime.UtcNow,
      EntityState = auditEntryItem.EntityState,
      AuditValues = new ObservableCollectionListSource<AuditValueEntity>()
    };

    foreach (var value in valuesTable)
    {
      var valueEntity = value.Adapt<AuditValueEntity>();
      valueEntity.AuditColumnId = auditColumnIds[value.AuditColumnName];
      auditEntity.AuditValues.Add(valueEntity);
    }

    await Audits.AddAsync(auditEntity);
    await SaveChangesAsync();
  }

  private async Task<int> GetAuditUserIdAsync(string userId)
  {
    var keyCache = AuditUserCacheKey(userId);

    var cacheValue = await mediator.Send(new MemoryCacheModuleGetQuery(keyCache));
    if (cacheValue.ResultValue?.ObjectValue != null)
    {
      // This message is also used in unit test.
      Logger.LogDebug("Value from cache:{GetAuditUserIdAsync}:{keyCache}:{userId}", nameof(GetAuditUserIdAsync), keyCache, userId);
      return ((AuditUserEntity)cacheValue.ResultValue.ObjectValue).Id;
    }

    var userEntity = await AuditUsers.FirstOrDefaultAsync(u => u.UserId == userId);
    if (userEntity == null)
    {
      userEntity = new AuditUserEntity
      {
        UserId = userId
      };
      await AuditUsers.AddAsync(userEntity);
      await SaveChangesAsync();
      // This message is also used in unit test.
      Logger.LogDebug("New db value created:{GetAuditUserIdAsync}:{keyCache}:{userId}", nameof(GetAuditUserIdAsync), keyCache, userId);
    }

    await mediator.Send(new MemoryCacheModuleSaveCommand(keyCache, userEntity));
    // This message is also used in unit test.
    Logger.LogDebug("Value saved to cache:{GetAuditUserIdAsync}:{keyCache}:{userId}", nameof(GetAuditUserIdAsync), keyCache, userId);
    return userEntity.Id;
  }

  public async Task<int> GetAuditTableIdAsync(string tableName, string? tableSchema, int version)
  {
    var keyCache = AuditTableCacheKey(tableName, tableSchema, version);

    var cacheValue = await mediator.Send(new MemoryCacheModuleGetQuery(keyCache));
    if (cacheValue.ResultValue?.ObjectValue != null)
    {
      // This message is also used in unit test.
      Logger.LogDebug("Value from cache:{GetAuditTableIdAsync}:{keyCache}:{tableName}:{tableSchema}", nameof(GetAuditTableIdAsync), keyCache, tableName, tableSchema);
      return ((AuditTableEntity)cacheValue.ResultValue.ObjectValue).Id;
    }

    var tableEntity = await AuditTables.FirstOrDefaultAsync(u => u.TableName == tableName && u.SchemaName == tableSchema);
    if (tableEntity == null)
    {
      tableEntity = new AuditTableEntity
      {
        TableName = tableName,
        SchemaName = tableSchema,
        Version = version
      };

      await AuditTables.AddAsync(tableEntity);
      await SaveChangesAsync();
      // This message is also used in unit test.
      Logger.LogDebug("New db value created:{GetAuditTableIdAsync}:{keyCache}:{tableName}:{tableSchema}", nameof(GetAuditTableIdAsync), keyCache, tableName, tableSchema);
    }

    await mediator.Send(new MemoryCacheModuleSaveCommand(keyCache, tableEntity));
    // This message is also used in unit test.
    Logger.LogDebug("Value saved to cache:{GetAuditTableIdAsync}:{keyCache}:{tableName}:{tableSchema}", nameof(GetAuditTableIdAsync), keyCache, tableName, tableSchema);
    return tableEntity.Id;
  }

  private async Task<Dictionary<string, int>> GetAuditColumnIdAsync(int tableId, Dictionary<string, string> columnNameWithTypes)
  {
    var keyCache = AuditColumnCacheKey(tableId);
    var res = new Dictionary<string, int>();

    var cacheValue = await mediator.Send(new MemoryCacheModuleGetQuery(keyCache));
    if (cacheValue.ResultValue?.ObjectValue != null)
    {
      // This message is also used in unit test.
      Logger.LogDebug("Value from cache:{GetAuditColumnIdAsync}:{keyCache}", nameof(GetAuditColumnIdAsync), keyCache);
      res = (Dictionary<string, int>)cacheValue.ResultValue.ObjectValue;
    }

    // Check if I have all columns from cache.
    var missingColumnNamesInCache = new List<string>();
    foreach (var columnName in columnNameWithTypes)
    {
      if (res.ContainsKey(columnName.Key))
        continue;

      missingColumnNamesInCache.Add(columnName.Key);
      // This message is also used in unit test.
      Logger.LogDebug("Missing value in cache:{GetAuditColumnIdAsync}:{keyCache}:{columnName}",
        nameof(GetAuditColumnIdAsync), keyCache, columnName.Key);
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
        // New audit column is appeared for existing table, this column is not saved in db yet.
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
        if (!columnNameWithTypes.TryGetValue(missingDbColumnName, out var dataType))
          throw new Exception($"Column {missingDbColumnName} has no datatype in dictionary.");

        var columnEntity = new AuditColumnEntity
        {
          ColumnName = missingDbColumnName,
          DataType = dataType,
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

    await mediator.Send(new MemoryCacheModuleSaveCommand(keyCache, res));
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