using System.Data;
using System.Reflection;
using ACore.Configuration;
using ACore.Extensions;
using ACore.Server.Configuration.CQRS.OptionsGet;
using ACore.Server.Modules.ICAMModule.CQRS.ICAMGetCurrentUser;
using ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbGet;
using ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbSave;
using ACore.Server.Modules.SettingsDbModule.Storage;
using ACore.Server.Storages.Attributes;
using ACore.Server.Storages.EF.Helpers;
using ACore.Server.Storages.Models;
using ACore.Server.Storages.Models.PK;
using ACore.Server.Storages.Scripts;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace ACore.Server.Storages.EF;

public abstract class DbContextBase(DbContextOptions options, IMediator mediator, ILogger<DbContextBase> logger)
  : DbContext(options), IStorage
{
  private readonly DbContextOptions _options = options;
  protected readonly ILogger<DbContextBase> Logger = logger ?? throw new ArgumentException($"{nameof(logger)} is null.");
  private readonly Dictionary<string, object> _registeredDbSets = [];

  private string StorageVersionBaseSettingKey => $"StorageVersion_{Enum.GetName(typeof(StorageTypeEnum), StorageDefinition.Type)}_{nameof(ISettingsDbModuleStorage)}";
  private string StorageVersionKey => $"StorageVersion_{Enum.GetName(typeof(StorageTypeEnum), StorageDefinition.Type)}_{ModuleName}";

  protected abstract DbScriptBase UpdateScripts { get; }
  public abstract StorageTypeDefinition StorageDefinition { get; }
  protected abstract string ModuleName { get; }


  protected async Task Save<TEntity, TPK>(TEntity data, string? hashToCheck = null)
    where TEntity : PKEntity<TPK>
  {
    ArgumentNullException.ThrowIfNull(data);
    
    TEntity existsEntity;

    var id = data.Id;
    if (id == null)
      ArgumentNullException.ThrowIfNull(id);

    var auditHelper = new AuditHelper<TEntity, TPK>(mediator, Model, StorageDefinition, data);
    await auditHelper.Initialize();

    var dbSet = GetDbSet<TEntity>();
    var isNew = EntityIdHelper.IsNew(id);

    if (!isNew)
    {
      existsEntity = await GetEntityById<TEntity, TPK>(id) ?? throw new Exception($"{typeof(TEntity).Name}:{id} doesn't exist.");
      //check db entity concurrency with hash
      if (data.GetType().IsHashCheck())
      {
        if (hashToCheck == null)
          throw new ArgumentNullException($"For update entity '{typeof(TEntity).Name}:{id}' is required a hash.");

        // Gets salt from global app settings.
        var saltForHash = (await mediator.Send(new AppOptionQuery<string>(OptionQueryEnum.HashSalt))).ResultValue ?? throw new Exception($"Mediator for {nameof(AppOptionQuery<string>)}.{Enum.GetName(OptionQueryEnum.HashSalt)} returned null value.");

        if (string.IsNullOrEmpty(saltForHash))
          Logger.LogWarning($"Please configure salt for hash. Check application settings and paste hash string to section '{nameof(ACoreOptions)}.{nameof(ACoreOptions.SaltForHash)}'");

        //Check consistency of entity.
        if (hashToCheck != existsEntity.HashObject(saltForHash))
          throw new DBConcurrencyException($"Entity '{typeof(TEntity).Name}' with id '{id.ToString()}' has been changed.");
      }

      await auditHelper.UpdateDbAction(existsEntity);
      data.Adapt(existsEntity);
    }
    else
    {
      existsEntity = data;
      SetNewId(existsEntity);
      await dbSet.AddAsync(existsEntity);
    }

    await SaveChangesAsync();

    if (isNew)
      await auditHelper.InsertDbAction(existsEntity);
  }

  protected async Task Delete<TEntity, TPK>(TPK id)
    where TEntity : PKEntity<TPK>
  {

    //var auditHelper = new AuditHelper2(mediator, Model, StorageDefinition);
    var entityToDelete = await GetEntityById<TEntity, TPK>(id) ?? throw new Exception($"{typeof(TEntity).Name}:{id} doesn't exist.");
    if (id == null)
      throw new Exception($"{typeof(TEntity).Name}:{id} doesn't exist.");

    var auditHelper = new AuditHelper<TEntity, TPK>(mediator, Model, StorageDefinition, entityToDelete);
    //  var audit = await auditHelper.CreateAuditEntryItem<TEntity>(id, await GetUserId(), EntityState.Deleted);
    await auditHelper.Initialize();

    var dbSet = GetDbSet<TEntity>();
    dbSet.Remove(entityToDelete);

    // if (audit != null)
    // {
    //   auditHelper.UpdateDeleteAudit(audit.Value, entityToDelete, id);
    //   // foreach (var p in entityToDelete.AllPropertiesValues())
    //   // {
    //   //   var colName = GetColumnName<TEntity>(p.propName, audit.Value.dbEntityType);
    //   //   if (colName != null)
    //   //     audit.Value.auditEntryItem.AddColumnEntry(colName, p.dataType, true, p.value, null);
    //   // }
    //
    //   await mediator.Send(new AuditSaveCommand(audit.Value.auditEntryItem));
    // }

    await SaveChangesAsync();
    await auditHelper.DeleteDbAction();
  }

  protected void RegisterDbSet<T>(DbSet<T>? dbSet) where T : class
  {
    if (dbSet == null)
      throw new ArgumentException($"{nameof(dbSet)} is null.");

    _registeredDbSets.Add(GetEntityTypeName<T>(), dbSet);
  }

  private static string GetEntityTypeName<T>()
    => typeof(T).FullName ?? throw new Exception($"{nameof(Type.FullName)} cannot be retrieved.");


  protected static void SetDatabaseNames<T>(Dictionary<string, StorageEntityNameDefinition> objectNameMapping, ModelBuilder modelBuilder) where T : class
  {
    if (objectNameMapping.TryGetValue(typeof(T).Name, out var auditColumnEntityObjectNames))
    {
      modelBuilder.Entity<T>().ToTable(auditColumnEntityObjectNames.TableName);
      foreach (var expression in auditColumnEntityObjectNames.GetColumns<T>())
      {
        modelBuilder.Entity<T>().Property(expression.Key).HasColumnName(expression.Value);
      }
    }
    else
    {
      throw new Exception($"Missing database name definition for entity: {typeof(T).Name}");
    }
  }

  public async Task UpdateDatabase()
  {
    var allVersions = UpdateScripts.AllVersions.ToList();

    var lastVersion = new Version("0.0.0.0");

    // Get the latest implemented version, if any.
    if (!await DbIsEmpty())
    {
      var ver = await mediator.Send(new SettingsDbGetQuery(StorageDefinition.Type, StorageVersionKey));
      if (ver is { IsSuccess: true, ResultValue: not null })
        lastVersion = new Version(ver.ResultValue);
    }

    var updatedToVersion = new Version("0.0.0.0");

    if (allVersions.Count != 0)
    {
      if (StorageDefinition.IsTransactionEnabled)
      {
        await using var transaction = await Database.BeginTransactionAsync();
        updatedToVersion = await UpdateDatabase(allVersions, lastVersion);
        try
        {
          await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
          await transaction.RollbackAsync();
          throw new Exception("UpdateDbAsync is rollback", ex);
        }
      }
      else
      {
        updatedToVersion = await UpdateDatabase(allVersions, lastVersion);
      }
    }

    if (this is ISettingsDbModuleStorage aa)
    {
      await aa.Setting_SaveAsync(StorageVersionKey, updatedToVersion.ToString(), true);
      return;
    }

    await mediator.Send(new SettingsDbSaveCommand(StorageDefinition.Type, StorageVersionKey, updatedToVersion.ToString(), true));
  }

  private async Task<Version> UpdateDatabase(List<DbVersionScriptsBase> allVersions, Version lastVersion)
  {
    var updatedToVersion = new Version("0.0.0.0");

    foreach (var version in allVersions.Where(a => a.Version > lastVersion))
    {
      foreach (var script in version.AllScripts)
      {
        try
        {
          Logger.LogInformation("SQL scripts version " + version.Version + ":" +
                                script);
          await Database.ExecuteSqlRawAsync(script);
          Logger.LogInformation("OK");
        }
        catch (Exception ex)
        {
          Logger.LogCritical(MethodBase.GetCurrentMethod()?.Name + " - Create tables in DB:", ex);

          throw new Exception("UpdateDB error for script ->" + script, ex);
        }
      }

      version.AfterScriptRunCode(this, _options, Logger);
      updatedToVersion = version.Version;
    }

    return updatedToVersion;
  }


  private async Task<bool> DbIsEmpty()
  {
    var res = true;
    try
    {
      var isSettingTable = await mediator.Send(new SettingsDbGetQuery(StorageDefinition.Type, StorageVersionBaseSettingKey));
      res = isSettingTable is { IsSuccess: true, ResultValue: null };
    }
    catch
    {
      Logger.LogDebug("Setting table has not been found.");
    }

    return res;
  }


  protected DbSet<T> GetDbSet<T>() where T : class
  {
    var entityName = GetEntityTypeName<T>();
    if (_registeredDbSets.TryGetValue(entityName, out var aa))
      return aa as DbSet<T> ?? throw new Exception($"DbSet '{entityName}' is not mutable type.");

    throw new Exception($"No registered {nameof(DbSet<T>)} has not been found. Please call the function {nameof(RegisterDbSet)} in ctor.");
  }

  private async Task<string> GetUserId()
  {
    var user = await mediator.Send(new ICAMGetCurrentUserQuery());
    if (user.IsFailure)
      throw new Exception(user.Error.ToString());
    ArgumentNullException.ThrowIfNull(user.ResultValue);
    return user.ResultValue.ToString();
  }

  #region id

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

    throw new Exception($"Unsupported type of primary key for entity '{typeof(TEntity).Name}.'");
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

      if (toCheck.BaseType != null)
        toCheck = toCheck.BaseType;
      else
        break;
    }

    return false;
  }

  #endregion

  #region Break Save inheritance

  public sealed override int SaveChanges(bool acceptAllChangesOnSuccess)
  {
    return base.SaveChanges(acceptAllChangesOnSuccess);
  }

  public sealed override int SaveChanges()
  {
    return base.SaveChanges();
  }

  public sealed override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
  {
    return base.SaveChangesAsync(cancellationToken);
  }

  public sealed override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
  {
    return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
  }

  #endregion
}