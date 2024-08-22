using ACore.Extensions;
using ACore.Modules.CacheModule.CQRS.CacheGet;
using ACore.Modules.CacheModule.CQRS.CacheSave;
using ACore.Modules.CacheModule.CQRS.Models;
using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Modules.AuditModule.Storage.SQL.Models;
using ACore.Server.Services.JMCache;
using ACore.Server.Storages.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace ACore.Server.Modules.AuditModule.Storage.SQL;

internal abstract class AuditSqlStorageImpl(DbContextOptions options, IMediator mediator, ILogger<AuditSqlStorageImpl> logger)
  : DbContextBase(options, mediator, logger), IAuditStorageModule
{
  private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(60);

  public static JMCacheKey AuditColumnCacheKey(int tableId) => JMCacheKey.Create(JMCacheServerCategory.DbTable, $"{nameof(AuditColumnEntity)}-{tableId}");
  public static JMCacheKey AuditUserCacheKey(string userId) => JMCacheKey.Create(JMCacheServerCategory.DbTable, $"{nameof(AuditUserEntity)}-{userId}");
  public static JMCacheKey AuditTableCacheKey(string tableName, string? schema) => JMCacheKey.Create(JMCacheServerCategory.DbTable, $"{nameof(AuditTableEntity)}-{tableName}-{schema ?? string.Empty}");

  public override DbScriptBase UpdateScripts => new ScriptRegistrations();
  protected override string ModuleName => nameof(IAuditStorageModule);

  public DbSet<AuditEntity> Audits { get; set; }
  internal DbSet<AuditColumnEntity> AuditColumns { get; set; }
  public DbSet<AuditUserEntity> AuditUsers { get; set; }
  public DbSet<AuditTableEntity> AuditTables { get; set; }
  public DbSet<AuditValueEntity> AuditValues { get; set; }

  public async Task<AuditEntryItem[]> AuditItemsAsync<T>(string tableName, T pkValue, string? schemaName = null)
  {
    var en = await SelectVwAudits(tableName, schemaName).Where(i => i.Audit.PKValue == Convert.ToInt64(pkValue)).ToArrayAsync();
    var res = new List<AuditEntryItem>();
    foreach (var grItem in en.GroupBy(e =>
               new
               {
                 tableName = e.Audit.AuditTable.TableName,
                 schemaName = e.Audit.AuditTable.SchemaName,
                 pk = e.Audit.PKValue,
                 pkString = e.Audit.PKValueString,
                 entityState = e.Audit.EntityState,
                 user= e.Audit.User,
                 created = e.Audit.DateTime
               }))
    {

      var primaryKeyValue = (grItem.Key.pk == null) ? grItem.Key.pkString : grItem.Key.pk as object;
      if (primaryKeyValue == null)
        throw new Exception("Primary key is null");
        
      var aab = new AuditEntryItem(grItem.Key.tableName, grItem.Key.schemaName, primaryKeyValue, grItem.Key.entityState)
      {
        Created = grItem.Key.created
      };
      aab.SetUser((grItem.Key.user.UserId, grItem.Key.user.UserName ?? string.Empty));
      foreach (var col in grItem.ToArray())
      {
        aab.AddEntry(col.AuditColumn.ColumnName, col.GetOldValueObject(), col.GetNewValueObject());
      }
      res.Add(aab);
    }

    return res.ToArray();
  }
 
  public async Task SaveAuditAsync(AuditEntryItem auditEntryItem)
  {
    var valuesTable = auditEntryItem.ChangedColumns
      .Select(change => AuditSqlValueItem.CreateValue(Logger, change))
      .OfType<AuditSqlValueItem>().ToList();

    var auditTableId = await GetAuditTableIdAsync(auditEntryItem.TableName, auditEntryItem.SchemaName);
    var auditColumnIds = await GetAuditColumnIdAsync(auditTableId, valuesTable
      .ToDictionary(k => k.AuditColumnName, v => v.AuditColumnDataType.FullName ?? string.Empty));

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

  private async Task<Dictionary<string, int>> GetAuditColumnIdAsync(int tableId, Dictionary<string, string> columnNameWithTypes)
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
    foreach (var columnName in columnNameWithTypes)
    {
      if (res.ContainsKey(columnName.Key))
        continue;

      missingColumnNamesInCache.Add(columnName.Key);
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