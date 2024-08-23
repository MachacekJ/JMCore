using ACore.Extensions;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.AuditModule.CQRS.Audit.AuditSave;
using ACore.Server.Modules.AuditModule.Extensions;
using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Modules.SettingModule.CQRS.SettingGet;
using ACore.Server.Storages.Models;
using ACore.Server.Storages.Models.PK;
using ACore.Server.Storages.Scripts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Storages.EF;

public abstract class AuditableDbContext : DbContextBase
{
  private bool? _isAuditEnabled;
  //private readonly IAuditDbService? _auditService;
  private readonly IAuditConfiguration? _auditConfiguration;

  protected AuditableDbContext(DbContextOptions options, IMediator mediator, ILogger<DbContextBase> logger, IAuditConfiguration? auditConfiguration) : base(options, mediator, logger)
  {
   // _auditService = auditService;
    _auditConfiguration = auditConfiguration;
  }

  public abstract Task<TEntity?> Get<TEntity, TPK>(TPK id) where TEntity : class;
  public abstract override DbScriptBase UpdateScripts { get; }

  private string AuditSettingKey => $"StorageVersion_{Enum.GetName(typeof(StorageTypeEnum), StorageDefinition.Type)}_{nameof(IAuditStorageModule)}";

  public abstract override StorageTypeDefinition StorageDefinition { get; }

  protected async Task<TU> SaveInternalWithAudit<TEntity, TU>(TEntity data, TU id, Func<TEntity, Task> addItem, Func<TEntity, TU> setId) where TEntity : PKEntity<TU>
  {
    TEntity existsEntity;
    if (id == null)
      ArgumentNullException.ThrowIfNull(id);

    var audit = await CreateAuditEntryItem<TEntity>(id, EntityState.Modified);

    var isNew = IsNew(id);

    if (!isNew)
    {
      existsEntity = await Get<TEntity, TU>(id) ?? throw new Exception($"{typeof(TEntity).Name}:{id} doesn't exist.");
      existsEntity.CopyPropertiesFrom(data, (p) =>
      {
        if (audit == null)
          return;

        var colName = GetColumnName<TEntity>(p.propName, audit.Value.dbEntityType);
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
      audit.Value.auditEntryItem.SetEntityState(EntityState.Added);
      audit.Value.auditEntryItem.SetPK(id);
      foreach (var p in existsEntity.AllProperties())
      {
        var colName = GetColumnName<TEntity>(p.propName, audit.Value.dbEntityType);
        if (colName != null)
          audit.Value.auditEntryItem.AddEntry(colName, null, p.value);
      }
    }

    //if (_auditService != null)
    await Mediator.Send(new AuditSaveCommand(audit.Value.auditEntryItem));
   //   await _auditService.SaveAuditAsync(audit.Value.auditEntryItem);

    return id;
  }

  protected virtual bool IsNew<TU>(TU id)
  {
    if (id == null)
      ArgumentNullException.ThrowIfNull(id);

    return typeof(TU) switch
    {
      { } entityType when entityType == typeof(int) => (int)Convert.ChangeType(id, typeof(int)) == 0,
      { } entityType when entityType == typeof(long) => (long)Convert.ChangeType(id, typeof(long)) == 0,
      { } entityType when entityType == typeof(string) => string.IsNullOrEmpty((string)Convert.ChangeType(id, typeof(string))),
      { } entityType when entityType == typeof(Guid) => (Guid)Convert.ChangeType(id, typeof(Guid)) == Guid.Empty,
      _ => throw new Exception("Unknown primary data type {}")
    };
  }

  protected async Task DeleteInternalWithAudit<TEntity, TPK>(TPK id, Action<TEntity> deleteItem) where TEntity : class
  {
    var en = await Get<TEntity, TPK>(id) ?? throw new Exception($"{typeof(TEntity).Name}:{id} doesn't exist.");
    if (id == null)
      throw new Exception($"{typeof(TEntity).Name}:{id} doesn't exist.");
        
    var audit = await CreateAuditEntryItem<TEntity>(id, EntityState.Deleted);

    deleteItem(en);

    if (audit != null)
    {
      foreach (var p in en.AllProperties())
      {
        var colName = GetColumnName<TEntity>(p.propName, audit.Value.dbEntityType);
        if (colName != null)
          audit.Value.auditEntryItem.AddEntry(colName, p.value, null);
      }

      await Mediator.Send(new AuditSaveCommand(audit.Value.auditEntryItem));
     // _auditService?.SaveAuditAsync(audit.Value.auditEntryItem);
    }

    await SaveChangesAsync();
  }

  private async Task<(AuditEntryItem auditEntryItem, IEntityType dbEntityType)?> CreateAuditEntryItem<TEntity>(object id, EntityState state)
  {
    if (!await IsAuditEnabledAsync() || !typeof(TEntity).IsAuditable(_auditConfiguration?.AuditEntities))
      return null;

    var dbEntityType = Model.FindEntityType(typeof(TEntity)) ?? throw new Exception($"Unknown db entity class '{typeof(TEntity).Name}'");
    var tableName = GetTableName(dbEntityType) ?? throw new Exception($"Unknown db table name for entity class '{typeof(TEntity).Name}'");
    var schemaName = dbEntityType.GetSchema();
    var audit = new AuditEntryItem(tableName, schemaName, id, state);
    return (audit, dbEntityType);
  }

  private string? GetTableName(IEntityType dbEntityType)
  {
    var tableName = dbEntityType.GetTableName();
    if (StorageDefinition.DataAnnotationTableNameKey == null)
      return tableName;

    var anno = dbEntityType.GetAnnotation(StorageDefinition.DataAnnotationTableNameKey).Value?.ToString();
    if (anno != null)
      tableName = anno;

    return tableName;
  }

  private string? GetColumnName<T>(string propName, IEntityType dbEntityType)
  {
    if (!typeof(T).IsAuditable(propName, _auditConfiguration?.NotAuditProperty))
      return null;

    var property = dbEntityType.GetProperties().SingleOrDefault(property => property.Name.Equals(propName, StringComparison.OrdinalIgnoreCase));
    if (property == null)
      return null;

    var columnName = property.GetColumnName();
    if (StorageDefinition.DataAnnotationColumnNameKey == null)
      return columnName;

    var anno = property.GetAnnotation(StorageDefinition.DataAnnotationColumnNameKey).Value?.ToString();
    if (anno != null)
      columnName = anno;

    return columnName;
  }

  /// <summary>
  /// Resolves problem with this situation. We have settings table where is audit on and audit structure is not created yet.
  /// In this case is audit will be skipped.  
  /// </summary>
  private async Task<bool> IsAuditEnabledAsync()
  {
    if (_isAuditEnabled != null)
      return _isAuditEnabled.Value;

    // if (_auditService == null)
    // {
    //   _isAuditEnabled = false;
    //   return false;
    // }

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