using System.Data;
using System.Reflection;
using ACore.Configuration;
using ACore.Extensions;
using ACore.Server.Configuration.CQRS.OptionsGet;
using ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbGet;
using ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbSave;
using ACore.Server.Modules.SettingsDbModule.Storage;
using ACore.Server.Storages.Attributes;
using ACore.Server.Storages.CQRS.Notifications;
using ACore.Server.Storages.Definitions.EF.Base.Scripts;
using ACore.Server.Storages.Definitions.EF.Helpers;
using ACore.Server.Storages.Definitions.Models;
using ACore.Server.Storages.Definitions.Models.PK;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Guid = System.Guid;

namespace ACore.Server.Storages.Definitions.EF.Base;

public abstract class DbContextBase : DbContext, IStorage
{
  private readonly DbContextOptions _options;
  protected readonly ILogger<DbContextBase> Logger;
  private readonly Dictionary<string, object> _registeredDbSets = [];
  private readonly IMediator _mediator;

  private string StorageVersionBaseSettingKey => $"StorageVersion_{Enum.GetName(typeof(StorageTypeEnum), StorageDefinition.Type)}_{nameof(ISettingsDbModuleStorage)}";
  private string StorageVersionKey => $"StorageVersion_{Enum.GetName(typeof(StorageTypeEnum), StorageDefinition.Type)}_{ModuleName}";

  protected abstract DbScriptBase UpdateScripts { get; }

  public StorageDefinition StorageDefinition => EFStorageDefinition;

  protected abstract EFStorageDefinition EFStorageDefinition { get; }

  protected abstract string ModuleName { get; }

  protected DbContextBase(DbContextOptions options, IMediator mediator, ILogger<DbContextBase> logger) : base(options)
  {
    _mediator = mediator;
    _options = options;
    Logger = logger ?? throw new ArgumentException($"{nameof(logger)} is null.");
  }


  protected async Task Save<TEntity, TPK>(TEntity newData, string? hashToCheck = null)
    where TEntity : PKEntity<TPK>
  {
    ArgumentNullException.ThrowIfNull(newData);

    TEntity existsEntity;

    var id = newData.Id;
    if (id == null)
      ArgumentNullException.ThrowIfNull(id);

    var auditHelper = new AuditHelper<TEntity, TPK>(_mediator, Model, EFStorageDefinition, newData);
    await auditHelper.Initialize();

    var dbSet = GetDbSet<TEntity>();
    var isNew = EFStorageDefinition.IsNew(id);

    if (!isNew)
    {
      existsEntity = await GetEntityById<TEntity, TPK>(id) ?? throw new Exception($"{typeof(TEntity).Name}:{id} doesn't exist.");
      //check db entity concurrency with hash
      if (newData.GetType().IsHashCheck())
      {
        if (hashToCheck == null)
          throw new ArgumentNullException($"For update entity '{typeof(TEntity).Name}:{id}' is required a hash.");

        // Gets salt from global app settings.
        var saltForHash = (await _mediator.Send(new AppOptionQuery<string>(OptionQueryEnum.HashSalt))).ResultValue ?? throw new Exception($"Mediator for {nameof(AppOptionQuery<string>)}.{Enum.GetName(OptionQueryEnum.HashSalt)} returned null value.");

        if (string.IsNullOrEmpty(saltForHash))
          Logger.LogWarning($"Please configure salt for hash. Check application settings and paste hash string to section '{nameof(ACoreOptions)}.{nameof(ACoreOptions.SaltForHash)}'");

        //Check consistency of entity.
        if (hashToCheck != existsEntity.HashObject(saltForHash))
          throw new DBConcurrencyException($"Entity '{typeof(TEntity).Name}' with id '{id.ToString()}' has been changed.");
      }

      await auditHelper.UpdateDbAction(existsEntity);
      newData.Adapt(existsEntity);
    }
    else
    {
      existsEntity = newData;
      existsEntity.Id = EFStorageDefinition.NewId<TEntity, TPK>(dbSet);
      await dbSet.AddAsync(existsEntity);
    }

    await SaveChangesAsync();

    await _mediator.Publish(new EntitySaveNotification<TEntity, TPK>(existsEntity, newData));

    if (isNew)
      await auditHelper.InsertDbAction(existsEntity);
  }

  protected async Task Delete<TEntity, TPK>(TPK id)
    where TEntity : PKEntity<TPK>
  {
    var entityToDelete = await GetEntityById<TEntity, TPK>(id) ?? throw new Exception($"{typeof(TEntity).Name}:{id} doesn't exist.");
    if (id == null)
      throw new Exception($"{typeof(TEntity).Name}:{id} doesn't exist.");

    var auditHelper = new AuditHelper<TEntity, TPK>(_mediator, Model, EFStorageDefinition, entityToDelete);
    await auditHelper.Initialize();

    var dbSet = GetDbSet<TEntity>();
    dbSet.Remove(entityToDelete);

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
      var ver = await _mediator.Send(new SettingsDbGetQuery(StorageDefinition.Type, StorageVersionKey));
      if (ver is { IsSuccess: true, ResultValue: not null })
        lastVersion = new Version(ver.ResultValue);
    }

    var updatedToVersion = new Version("0.0.0.0");

    if (allVersions.Count != 0)
    {
      if (EFStorageDefinition.IsTransactionEnabled)
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

    await _mediator.Send(new SettingsDbSaveCommand(StorageDefinition.Type, StorageVersionKey, updatedToVersion.ToString(), true));
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
      var isSettingTable = await _mediator.Send(new SettingsDbGetQuery(StorageDefinition.Type, StorageVersionBaseSettingKey));
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

  #region id

  /// <summary>
  /// For some storage the id must be set manually.
  /// e.g. <see cref="StorageTypeEnum.Memory"/>
  /// </summary>
  // private void SetNewId<TEntity, TPK>(TEntity obj)
  //   where TEntity : PKEntity<TPK>
  // {

//     if (StorageDefinition.Type != StorageTypeEnum.Memory)
//       return;
//
//     switch (obj)
//     {
//       case PKIntEntity intV:
//         if (StorageDefinition.Type == StorageTypeEnum.Memory)
//         {
//           var db = GetDbSet<TEntity>();
//           var id = !db.Any() ? 1 : db.Max(i => (i as PKIntEntity).Id) + 1;
//           intV.Id = id;
//         }
//
//         break;
//       case PKLongEntity longV:
//         if (StorageDefinition.Type == StorageTypeEnum.Memory)
//         {
//           var db = GetDbSet<TEntity>();
// #pragma warning disable CS8602 // Dereference of a possibly null reference.
//           var id = !db.Any() ? 1 : db.Max(i => (i as PKLongEntity).Id) + 1;
// #pragma warning restore CS8602 // Dereference of a possibly null reference.
//           longV.Id = id;
//         }
//
//         break;
//       case PKGuidEntity gV:
//         gV.Id = Guid.NewGuid();
//         break;
//       case PKStringEntity stringEntity:
//         stringEntity.Id = Guid.NewGuid().ToString();
//         break;
//       case PKMongoEntity stringEntity:
//         stringEntity.Id = ObjectId.GenerateNewId();
//         break;
  //   }
  //}
#pragma warning disable CS8602 // Dereference of a possibly null reference.
  private async Task<TEntity?> GetEntityById<TEntity, TPK>(TPK id)
    where TEntity : PKEntity<TPK>
  {
    var remap = GetDbSet<TEntity>();

    if (typeof(TEntity).IsSubclassOfRawGeneric(typeof(PKIntEntity)))
      return await remap.SingleOrDefaultAsync(e => (e as PKIntEntity).Id == Convert.ToInt32(id));

    if (typeof(TEntity).IsSubclassOfRawGeneric(typeof(PKLongEntity)))
      return await remap.SingleOrDefaultAsync(e => (e as PKLongEntity).Id == Convert.ToInt64(id));

    if (typeof(TEntity).IsSubclassOfRawGeneric(typeof(PKGuidEntity)))
      return await remap.SingleOrDefaultAsync(e => (e as PKGuidEntity).Id == (Guid)(Convert.ChangeType(id, typeof(Guid)) ?? PKGuidEntity.EmptyId));

    if (typeof(TEntity).IsSubclassOfRawGeneric(typeof(PKStringEntity)))
      return await remap.SingleOrDefaultAsync(e => (e as PKStringEntity).Id == id.ToString());

    if (typeof(TEntity).IsSubclassOfRawGeneric(typeof(PKMongoEntity)))
      return await remap.SingleOrDefaultAsync(e => (e as PKMongoEntity).Id == (ObjectId)(Convert.ChangeType(id, typeof(ObjectId)) ?? PKMongoEntity.EmptyId));

    throw new Exception($"Unsupported type of primary key for entity '{typeof(TEntity).Name}.'");
  }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

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

  public sealed override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
  {
    return base.SaveChangesAsync(cancellationToken);
  }

  public sealed override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new())
  {
    return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
  }

  #endregion
}