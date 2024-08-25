using ACore.Extensions;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.AuditModule.CQRS.Audit.AuditSave;
using ACore.Server.Modules.AuditModule.Extensions;
using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Storages.Models;
using ACore.Server.Storages.Models.PK;
using ACore.Server.Storages.Scripts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace ACore.Server.Storages.EF;

public abstract class AuditableDbContext(DbContextOptions options, IMediator mediator, ILogger<DbContextBase> logger, IAuditConfiguration? auditConfiguration)
  : DbContextBase(options, mediator, logger)
{
  public abstract override DbScriptBase UpdateScripts { get; }

  public abstract override StorageTypeDefinition StorageDefinition { get; }

  private Dictionary<string, object> _dbSets = [];

  protected void RegisterDbSet<T>(DbSet<T>? dbSet) where T : class
  {
    if (dbSet == null)
      throw new Exception("fdsga fdsaf asdf asdf as");

    _dbSets.Add(GetEntityTypeName<T>(), dbSet);
  }

  public async Task<TEntity?> Get<TEntity, TPK>(TPK id)
    where TEntity : class

  {
    var remap = GetDbSet<TEntity>();

    if (IsSubclassOfRawGeneric(typeof(PKIntEntity), typeof(TEntity)))
      return await remap.SingleOrDefaultAsync(e => (e as PKIntEntity).Id == Convert.ToInt32(id));

    if (IsSubclassOfRawGeneric(typeof(PKLongEntity), typeof(TEntity)))
      return await remap.SingleOrDefaultAsync(e => (e as PKLongEntity).Id == Convert.ToInt64(id));
    
    if (IsSubclassOfRawGeneric(typeof(PKGuidEntity), typeof(TEntity)))
      return await remap.SingleOrDefaultAsync(e => (e as PKGuidEntity).Id == (Guid)Convert.ChangeType(id, typeof(Guid)) );
    
    if (IsSubclassOfRawGeneric(typeof(PKStringEntity), typeof(TEntity)))
      return await remap.SingleOrDefaultAsync(e => (e as PKStringEntity).Id == id.ToString());
    
    if (IsSubclassOfRawGeneric(typeof(PKMongoEntity), typeof(TEntity)))
      return await remap.SingleOrDefaultAsync(e => (e as PKMongoEntity).Id == (ObjectId)Convert.ChangeType(id, typeof(ObjectId)));
    
    throw new Exception("tyuyh thyh ghyj tyihndfbfdsf dtyhyjhghjkopohsg");
  }

  private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
  {
    while (toCheck != null && toCheck != typeof(object))
    {
      var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
      if (generic == cur)
      {
        return true;
      }

      toCheck = toCheck.BaseType;
    }

    return false;
  }
  
  private DbSet<T> GetDbSet<T>() where T : class
  {
    var entityName = GetEntityTypeName<T>();
    if (_dbSets.TryGetValue(entityName, out var aa))
      return aa as DbSet<T> ?? throw new Exception($"DbSet '{entityName}' is not mutable type.");

    throw new Exception($"DbSet '{entityName}' is not registered.");
  }

  private static string GetEntityTypeName<T>()
  {
    return typeof(T).FullName ?? throw new Exception("fgjtygh ioikj");
  }

  private T GetId<T>(object obj)
  {
    if (obj is PKEntity<T> intV)
    {
      return (T)(Convert.ChangeType(intV.Id, typeof(T)) ?? throw new Exception("fdas piouy erwqghmnjk jisdf"));
    }


    throw new Exception("fdsafsad fdsaghikojhbnmns ghdh tjh");
    //obj.GetType().GetProperty("Id").GetValue(obj) ?? ;
  }

  protected async Task<TPK> SaveInternalWithAudit<TEntity, TPK>(TEntity data)
    where TEntity : class
  {
    TEntity existsEntity;

    var id = GetId<TPK>(data);

    if (id == null)
      ArgumentNullException.ThrowIfNull(id);

    var audit = await CreateAuditEntryItem<TEntity>(id, EntityState.Modified);

    var dbSet = GetDbSet<TEntity>();

    var isNew = IsNew(id);

    if (!isNew)
    {
      existsEntity = await Get<TEntity, TPK>(id) ?? throw new Exception($"{typeof(TEntity).Name}:{id} doesn't exist.");
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
      await dbSet.AddAsync(existsEntity);
    }

    await SaveChangesAsync();
    id = (existsEntity as PKEntity<TPK> ?? throw new Exception("poifdsa fdsoujifnio ikgkjm.")).Id;

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

  private void setId<TEntity>(TEntity obj)
    where TEntity : class
  {
    switch (obj)
    {
      case PKIntEntity intV:
        if (StorageDefinition.Type == StorageTypeEnum.Memory)
        {
          var db = GetDbSet<TEntity>();
          var id = !db.Any() ? 1 : db.Max(i => (i as PKIntEntity).Id) + 1;
          intV.Id = id;
        }
        break;
      case PKLongEntity longV:
        if (StorageDefinition.Type == StorageTypeEnum.Memory)
        {
          var db = GetDbSet<TEntity>();
          var id = !db.Any() ? 1 : db.Max(i => (i as PKLongEntity).Id) + 1;
          longV.Id = id;
        }
        break;
      case PKGuidEntity gV:
        gV.Id = Guid.NewGuid();
        break;
      case PKStringEntity stringEntity:
        stringEntity.Id = Guid.NewGuid().ToString();
        break;
      case PKMongoEntity stringEntity:
        stringEntity.Id = ObjectId.GenerateNewId();
        break;
    }
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

  protected async Task DeleteInternalWithAudit<TEntity, TPK>(TPK id)
    where TEntity : class

  {
    var en = await Get<TEntity, TPK>(id) ?? throw new Exception($"{typeof(TEntity).Name}:{id} doesn't exist.");
    if (id == null)
      throw new Exception($"{typeof(TEntity).Name}:{id} doesn't exist.");

    var audit = await CreateAuditEntryItem<TEntity>(id, EntityState.Deleted);

    var dbSet = GetDbSet<TEntity>();
    dbSet.Remove(en);

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
    if (!await IsAuditEnabledAsync() || !typeof(TEntity).IsAuditable(auditConfiguration?.AuditEntities))
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
    if (!typeof(T).IsAuditable(propName, auditConfiguration?.NotAuditProperty))
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
}