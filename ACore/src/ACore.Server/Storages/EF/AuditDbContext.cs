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

public abstract class AuditableDbContext(DbContextOptions options, IMediator mediator, ILogger<DbContextBase> logger)
  : DbContextBase(options, mediator, logger)
{
  public abstract override DbScriptBase UpdateScripts { get; }
  public abstract override StorageTypeDefinition StorageDefinition { get; }


  private readonly Dictionary<string, object> _registeredDbSets = [];

  protected void RegisterDbSet<T>(DbSet<T>? dbSet) where T : class
  {
    if (dbSet == null)
      throw new ArgumentException($"{nameof(dbSet)} is null.");

    _registeredDbSets.Add(GetEntityTypeName<T>(), dbSet);
  }

  protected async Task SaveWithAudit<TEntity, TPK>(TEntity data)
    where TEntity : class
  {
    ArgumentNullException.ThrowIfNull(data);

    TEntity existsEntity;

    var id = GetIdValue<TPK>(data);
    if (id == null)
      ArgumentNullException.ThrowIfNull(id);

    var audit = await CreateAuditEntryItem<TEntity>(id, EntityState.Modified);
    var dbSet = GetDbSet<TEntity>();
    var isNew = IsNew(id);

    if (!isNew)
    {
      existsEntity = await GetEntityById<TEntity, TPK>(id) ?? throw new Exception($"{typeof(TEntity).Name}:{id} doesn't exist.");
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
      SetNewId(existsEntity);
      await dbSet.AddAsync(existsEntity);
    }

    await SaveChangesAsync();
    id = (existsEntity as PKEntity<TPK> ?? throw new Exception("poifdsa fdsoujifnio ikgkjm.")).Id;
    //data.CopyPropertiesFrom(existsEntity);

    if (audit == null)
      return;

    if (isNew)
    {
      audit.Value.auditEntryItem.SetEntityState(EntityState.Added);
      audit.Value.auditEntryItem.SetPK(id);
      foreach (var p in existsEntity.AllPropertiesValues())
      {
        var colName = GetColumnName<TEntity>(p.propName, audit.Value.dbEntityType);
        if (colName != null)
          audit.Value.auditEntryItem.AddEntry(colName, null, p.value);
      }
    }

    await Mediator.Send(new AuditSaveCommand(audit.Value.auditEntryItem));
  }

  protected async Task DeleteWithAudit<TEntity, TPK>(TPK id)
    where TEntity : class
  {
    var entityToDelete = await GetEntityById<TEntity, TPK>(id) ?? throw new Exception($"{typeof(TEntity).Name}:{id} doesn't exist.");
    if (id == null)
      throw new Exception($"{typeof(TEntity).Name}:{id} doesn't exist.");

    var audit = await CreateAuditEntryItem<TEntity>(id, EntityState.Deleted);

    var dbSet = GetDbSet<TEntity>();
    dbSet.Remove(entityToDelete);

    if (audit != null)
    {
      foreach (var p in entityToDelete.AllPropertiesValues())
      {
        var colName = GetColumnName<TEntity>(p.propName, audit.Value.dbEntityType);
        if (colName != null)
          audit.Value.auditEntryItem.AddEntry(colName, p.value, null);
      }

      await Mediator.Send(new AuditSaveCommand(audit.Value.auditEntryItem));
    }

    await SaveChangesAsync();
  }

  protected DbSet<T> GetDbSet<T>() where T : class
  {
    var entityName = GetEntityTypeName<T>();
    if (_registeredDbSets.TryGetValue(entityName, out var aa))
      return aa as DbSet<T> ?? throw new Exception($"DbSet '{entityName}' is not mutable type.");

    throw new Exception($"No registered {nameof(DbSet<T>)} has not been found. Please call the function {nameof(RegisterDbSet)} in ctor.");
  }

  private async Task<TEntity?> GetEntityById<TEntity, TPK>(TPK id)
    where TEntity : class
  {
    var remap = GetDbSet<TEntity>();

    if (IsSubclassOfRawGeneric(typeof(PKIntEntity), typeof(TEntity)))
      return await remap.SingleOrDefaultAsync(e => (e as PKIntEntity).Id == Convert.ToInt32(id));

    if (IsSubclassOfRawGeneric(typeof(PKLongEntity), typeof(TEntity)))
      return await remap.SingleOrDefaultAsync(e => (e as PKLongEntity).Id == Convert.ToInt64(id));

    if (IsSubclassOfRawGeneric(typeof(PKGuidEntity), typeof(TEntity)))
      return await remap.SingleOrDefaultAsync(e => (e as PKGuidEntity).Id == (Guid)Convert.ChangeType(id, typeof(Guid)));

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

  private static string GetEntityTypeName<T>()
  {
    return typeof(T).FullName ?? throw new Exception("fgjtygh ioikj");
  }

  private T GetIdValue<T>(object obj)
  {
    if (obj is PKEntity<T> intV)
    {
      return (T)(Convert.ChangeType(intV.Id, typeof(T)) ?? throw new Exception("fdas piouy erwqghmnjk jisdf"));
    }


    throw new Exception("fdsafsad fdsaghikojhbnmns ghdh tjh");
    //obj.GetType().GetProperty("Id").GetValue(obj) ?? ;
  }

  private void SetNewId<TEntity>(TEntity obj)
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

  private static bool IsNew<TU>(TU id)
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

  private async Task<(AuditEntryItem auditEntryItem, IEntityType dbEntityType)?> CreateAuditEntryItem<TEntity>(object id, EntityState state)
  {
    if (!await IsAuditEnabledAsync())
      return null;

    var dbEntityType = Model.FindEntityType(typeof(TEntity)) ?? throw new Exception($"Unknown db entity class '{typeof(TEntity).Name}'");
    var auditableAttribute = dbEntityType.ClrType.IsAuditable();
    if (auditableAttribute == null)
      return null;
    var tableName = GetTableName(dbEntityType) ?? throw new Exception($"Unknown db table name for entity class '{typeof(TEntity).Name}'");
    var schemaName = dbEntityType.GetSchema();
    var audit = new AuditEntryItem(tableName, schemaName, auditableAttribute.Version, id, state);
    return (audit, dbEntityType);
  }

  private string? GetTableName(IReadOnlyEntityType dbEntityType)
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
    var auditableAttribute = typeof(T).IsAuditable(propName); //auditConfiguration?.NotAuditProperty
    if (auditableAttribute == null)
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