using ACore.Extensions;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Modules.AuditModule.CQRS.AuditSave;
using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Modules.ICAMModule.CQRS.ICAMGetCurrentUser;
using ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbGet;
using ACore.Server.Storages.Models;
using ACore.Server.Storages.Models.PK;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ACore.Server.Storages.EF.Helpers;

public class AuditHelper<TEntity, TPK>(IMediator mediator, IModel model, StorageTypeDefinition storageDefinition, TEntity initData)
  where TEntity : PKEntity<TPK>
{
  // private bool? _isAuditEnabled;
  private string AuditSettingKey => $"StorageVersion_{Enum.GetName(typeof(StorageTypeEnum), storageDefinition.Type)}_{nameof(IAuditStorageModule)}";
  private AuditEntryItem? _preparedAuditEntryItem;
  private IEntityType? _dbEntityType;

  public async Task Initialize()
  {
    ArgumentNullException.ThrowIfNull(initData.Id);
    // ArgumentNullException.ThrowIfNull(id);

    if (!await IsAuditEnabledAsync())
      return;

    //_initDiff = Compare(data, null);

    _dbEntityType = model.FindEntityType(typeof(TEntity)) ?? throw new Exception($"Unknown db entity class '{typeof(TEntity).Name}'");
    var auditableAttribute = _dbEntityType.ClrType.IsAuditable();
    if (auditableAttribute == null)
      return;
    var tableName = GetTableName(_dbEntityType) ?? throw new Exception($"Unknown db table name for entity class '{typeof(TEntity).Name}'");
    var schemaName = _dbEntityType.GetSchema();
    var userId = await GetUserId();
    _preparedAuditEntryItem = new AuditEntryItem(tableName, schemaName, auditableAttribute.Version, initData.Id, EntityState.Detached, userId);
    _preparedAuditEntryItem.SetPK(initData.Id);
  }

  public async Task InsertDbAction(TEntity data)
  {
    if (_preparedAuditEntryItem == null)
      return;

    ArgumentNullException.ThrowIfNull(_dbEntityType);

    Compare(data, null, p =>
    {
      var colName = GetColumnName<TEntity>(p.propName, _dbEntityType);
      if (colName != null)
        _preparedAuditEntryItem.AddColumnEntry(colName, p.dataType, true, null, p.oldValue);
    });

    _preparedAuditEntryItem.SetPK(data.Id);
    _preparedAuditEntryItem.SetEntityState(EntityState.Added);
    await SaveAudit();
  }

  public async Task UpdateDbAction(TEntity oldData)
  {
    if (_preparedAuditEntryItem == null)
      return;

    ArgumentNullException.ThrowIfNull(_dbEntityType);

    Compare(oldData, initData, p =>
    {
      var colName = GetColumnName<TEntity>(p.propName, _dbEntityType);
      if (colName != null)
        _preparedAuditEntryItem.AddColumnEntry(colName, p.dataType, p.isChange, p.oldValue, p.isChange ? p.newValue : null);
    });

    _preparedAuditEntryItem.SetEntityState(EntityState.Modified);
    await SaveAudit();
  }

  public async Task DeleteDbAction()
  {
    if (_preparedAuditEntryItem == null)
      return;

    ArgumentNullException.ThrowIfNull(_dbEntityType);

    Compare(initData, null, p =>
    {
      var colName = GetColumnName<TEntity>(p.propName, _dbEntityType);
      if (colName != null)
        _preparedAuditEntryItem.AddColumnEntry(colName, p.dataType, true, p.oldValue, null);
    });
    
    _preparedAuditEntryItem.SetEntityState(EntityState.Deleted);
    await SaveAudit();
  }

  private async Task SaveAudit()
  {
    if (_preparedAuditEntryItem == null)
      return;

    await mediator.Send(new AuditSaveCommand(_preparedAuditEntryItem));
  }

  /// <summary>
  /// Resolves problem with this situation. We have settings table where is audit on and audit structure is not created yet.
  /// In this case is audit will be skipped.  
  /// </summary>
  private async Task<bool> IsAuditEnabledAsync()
  {
    // if (_isAuditEnabled != null)
    //   return _isAuditEnabled.Value;

    // Check if db structure is already created.
    var isAuditTable = await mediator.Send(new SettingsDbGetQuery(storageDefinition.Type, AuditSettingKey));

    if (isAuditTable.IsSuccess && string.IsNullOrEmpty(isAuditTable.ResultValue))
    {
      //   _isAuditEnabled = false;
      return false;
    }

    // _isAuditEnabled = true;
    return true;
  }

  private string? GetTableName(IReadOnlyEntityType dbEntityType)
  {
    var tableName = dbEntityType.GetTableName();
    if (storageDefinition.DataAnnotationTableNameKey == null)
      return tableName;

    var anno = dbEntityType.GetAnnotation(storageDefinition.DataAnnotationTableNameKey).Value?.ToString();
    if (anno != null)
      tableName = anno;

    return tableName;
  }

  private async Task<string> GetUserId()
  {
    var user = await mediator.Send(new ICAMGetCurrentUserQuery());
    if (user.IsFailure)
      throw new Exception(user.Error.ToString());
    ArgumentNullException.ThrowIfNull(user.ResultValue);
    return user.ResultValue.ToString();
  }


  private string? GetColumnName<T>(string propName, IEntityType dbEntityType)
  {
    if (propName.StartsWith('.'))
      propName = propName.Substring(1);

    var auditableAttribute = typeof(T).IsAuditable(propName); //auditConfiguration?.NotAuditProperty
    if (auditableAttribute == null)
      return null;

    var property = dbEntityType.GetProperties().SingleOrDefault(property => property.Name.Equals(propName, StringComparison.OrdinalIgnoreCase));
    if (property == null)
      return null;

    var columnName = property.GetColumnName();
    if (storageDefinition.DataAnnotationColumnNameKey == null)
      return columnName;

    var anno = property.GetAnnotation(storageDefinition.DataAnnotationColumnNameKey).Value?.ToString();
    if (anno != null)
      columnName = anno;

    return columnName;
  }

  private static void Compare(TEntity oldObj, TEntity? newObj, Action<(string propName, string dataType, bool isChange, object? oldValue, object? newValue)> updatingValue)
  {
    var newProperties = newObj == null ? null : ObjectExtensionMethods.GetProperties(newObj);
    var oldProperties = ObjectExtensionMethods.GetProperties(oldObj);


    foreach (var oldProperty in oldProperties)
    {
      var oldValue = oldProperty.GetValue(oldObj);

      if (newProperties == null)
      {
        updatingValue?.Invoke(new ValueTuple<string, string, bool, object?, object?>(oldProperty.Name, ObjectExtensionMethods.FullName(oldProperty.PropertyType), true, oldValue, null));
        continue;
      }

      foreach (var newProperty in newProperties)
      {
        if (oldProperty.Name != newProperty.Name)
          continue;

        if (oldProperty.PropertyType != newProperty.PropertyType)
          continue;

        
        var newValue = newProperty.GetValue(newObj);

        var isChange = (newValue == null && oldValue != null)
                       || (newValue != null && oldValue == null)
                       || (newValue != null && oldValue != null && !newValue.Equals(oldValue));

        updatingValue?.Invoke(new ValueTuple<string, string, bool, object?, object?>(newProperty.Name, ObjectExtensionMethods.FullName(oldProperty.PropertyType), isChange, oldValue, newValue));

        break;
      }
    }
  }
}