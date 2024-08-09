using ACore.Extensions;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.AuditModule.EF;
using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Modules.SettingModule.CQRS.SettingGet;
using ACore.Server.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Storages.EF;

public abstract class AuditableDbContext : DbContextBase
{
  private bool? _isAuditEnabled;
  private readonly IAuditDbService? _auditService;
  private readonly IAuditConfiguration? _auditConfiguration;

  protected AuditableDbContext(DbContextOptions options, IMediator mediator, ILogger<DbContextBase> logger, IAuditDbService? auditService, IAuditConfiguration? auditConfiguration) : base(options, mediator, logger)
  {
    _auditService = auditService;
    _auditConfiguration = auditConfiguration;
  }

  public abstract Task<TEntity?> Get<TEntity, TPK>(TPK id) where TEntity : class;
  public abstract override DbScriptBase UpdateScripts { get; }

  private string AuditSettingKey => $"StorageVersion_{Enum.GetName(typeof(StorageTypeEnum), StorageDefinition.Type)}_{nameof(IAuditStorageModule)}";

  public abstract override StorageTypeDefinition StorageDefinition { get; }

  protected async Task<TU> SaveInternalWithAudit<TEntity, TU>(TEntity data, TU id, Func<TEntity, Task> addItem, Func<TEntity, TU> setId) where TEntity : StorageEntity<TU>
  {
    TEntity existsEntity;
    if (id == null)
      ArgumentNullException.ThrowIfNull(id);

    var audit = await GetAuditI<TEntity>(id, EntityState.Modified);

    var isNew = typeof(TU) switch
    {
      { } entityType when entityType == typeof(int) => (int)Convert.ChangeType(id, typeof(int)) == 0,
      { } entityType when entityType == typeof(long) => (long)Convert.ChangeType(id, typeof(long)) == 0,
      { } entityType when entityType == typeof(string) => string.IsNullOrEmpty((string)Convert.ChangeType(id, typeof(string))),
      { } entityType when entityType == typeof(Guid) => (Guid)Convert.ChangeType(id, typeof(Guid)) == Guid.Empty,
      _ => throw new Exception("Unknown primary data type {}")
    };

    if (!isNew)
    {
      existsEntity = await Get<TEntity, TU>(id) ?? throw new Exception($"{typeof(TEntity).Name}:{id} doesn't exist.");
      existsEntity.CopyPropertiesFrom(data, (p) =>
      {
        if (audit == null)
          return;

        var colName = GetColumnName<TEntity>(audit.Value.dbEntityType, p.propName);
        if (colName != null)
          audit.Value.auditEntryItem.AddEntry(colName, p.oldValue, p.newValue);
      });
    }
    else
    {
      existsEntity = data;
      setId(existsEntity);
      await addItem(existsEntity);
    }

    await SaveChangesAsync();
    id = existsEntity.Id;

    if (audit == null)
      return id;

    if (isNew)
    {
      audit.Value.auditEntryItem.EntityState = EntityState.Added;
      audit.Value.auditEntryItem.SetPK(id);
      foreach (var p in existsEntity.AllProperties())
      {
        var colName = GetColumnName<TEntity>(audit.Value.dbEntityType, p.propName);
        if (colName != null)
          audit.Value.auditEntryItem.AddEntry(colName, null, p.value);
      }
    }

    _auditService?.SaveAuditAsync(audit.Value.auditEntryItem);
    
    return id;
  }

  protected async Task DeleteInternalWithAudit<TEntity, TPK>(TPK id, Action<TEntity> deleteItem) where TEntity : class
  {
    var en = await Get<TEntity, TPK>(id) ?? throw new Exception($"{typeof(TEntity).Name}:{id} doesn't exist.");
    var audit = await GetAuditI<TEntity>(id, EntityState.Deleted);

    deleteItem(en);

    if (audit != null)
    {
      foreach (var p in en.AllProperties())
      {
        var colName = GetColumnName<TEntity>(audit.Value.dbEntityType, p.propName);
        if (colName != null)
          audit.Value.auditEntryItem.AddEntry(colName, p.value, null);
      }
      _auditService?.SaveAuditAsync(audit.Value.auditEntryItem);
    }

    await SaveChangesAsync();
  }

  private async Task<(AuditEntryItem auditEntryItem, IEntityType dbEntityType)?> GetAuditI<TEntity>(object id, EntityState state)
  {
    if (_auditService == null || !await IsAuditEnabledAsync() || !typeof(TEntity).IsAuditable(_auditConfiguration?.AuditEntities))
      return null;

    var dbEntityType = Model.FindEntityType(typeof(TEntity)) ?? throw new Exception($"Unknown db entity class '{typeof(TEntity).Name}'");
    var tableName = dbEntityType.GetTableName() ?? throw new Exception($"Unknown db table name for entity class '{typeof(TEntity).Name}'");
    var schemaName = dbEntityType.GetSchema();
    var audit = new AuditEntryItem(tableName, schemaName, id, _auditService.AuditUserProvider)
      { EntityState = state };
    return (audit, dbEntityType);
  }

  private string? GetColumnName<T>(IEntityType dbEntityType, string propName)
  {
    var property = dbEntityType.GetProperties().SingleOrDefault(property => property.Name.Equals(propName, StringComparison.OrdinalIgnoreCase));
    var columnName = property?.GetColumnName();
    return columnName != null && typeof(T).IsAuditable(propName, _auditConfiguration?.NotAuditProperty) ? columnName : null;
  }

  /// <summary>
  /// Resolves problem with this situation. We have settings table where is audit on and audit structure is not created yet.
  /// In this case is audit will be skipped.  
  /// </summary>
  private async Task<bool> IsAuditEnabledAsync()
  {
    if (_isAuditEnabled != null)
      return _isAuditEnabled.Value;

    if (_auditService == null)
    {
      _isAuditEnabled = false;
      return false;
    }

    // Check if db structure is already created.
    var isAuditTable = await Mediator.Send(new SettingGetQuery(StorageDefinition.Type, AuditSettingKey));

    if (!string.IsNullOrEmpty(isAuditTable))
    {
      _isAuditEnabled = true;
      return true;
    }

    return false;
  }
}