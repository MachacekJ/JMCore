﻿using System.Reflection;
using JMCore.Modules.CacheModule;
using JMCore.Modules.CacheModule.CQRS.CacheGet;
using JMCore.Modules.CacheModule.CQRS.CacheRemove;
using JMCore.Modules.CacheModule.CQRS.CacheSave;
using JMCore.Modules.CacheModule.CQRS.Models;
using JMCore.Server.Modules.AuditModule.EF;
using JMCore.Server.Modules.AuditModule.Storage;
using JMCore.Server.Modules.SettingModule.CQRS.SettingGet;
using JMCore.Server.Modules.SettingModule.CQRS.SettingSave;
using JMCore.Server.Modules.SettingModule.Storage;
using JMCore.Server.Modules.SettingModule.Storage.Models;
using JMCore.Server.Services.JMCache;
using JMCore.Server.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JMCore.Server.Storages.EF;

/// <summary>
/// This base class implements audit functionality and calls db <see cref="SaveChangesAsync"/> method.
/// Use <see cref="SaveChanges"/> only in special cases.
/// </summary>
public abstract class DbContextBase : DbContext, IBasicStorageModule, IStorage
{
  private static readonly JMCacheKey CacheKeyTableSetting = JMCacheKey.Create(JMCacheServerCategory.DbTable, nameof(SettingEntity));
  
  public abstract DbScriptBase UpdateScripts { get; }
  public abstract StorageTypeDefinition StorageDefinition { get; }
  protected abstract string ModuleName { get; }
  
  private string AuditSettingKey => $"StorageVersion_{Enum.GetName(typeof(StorageTypeEnum), StorageDefinition.Type)}_{nameof(IAuditStorageModule)}";
  private string StorageVersionBaseSettingKey => $"StorageVersion_{Enum.GetName(typeof(StorageTypeEnum), StorageDefinition.Type)}_{nameof(IBasicStorageModule)}";
  private string StorageVersionKey => $"StorageVersion_{Enum.GetName(typeof(StorageTypeEnum), StorageDefinition.Type)}_{ModuleName}";

  protected readonly ILogger<DbContextBase> Logger;
  private readonly IAuditDbService? _auditService;
  private readonly DbContextOptions _options;

  protected IMediator Mediator { get; }

  public DbSet<SettingEntity> Settings { get; set; }


  /// <summary>
  /// This base class implements audit functionality and calls db <see cref="SaveChangesAsync"/> method.
  /// Use <see cref="SaveChanges"/> only in special cases.
  /// </summary>
  protected DbContextBase(DbContextOptions options, IMediator mediator, ILogger<DbContextBase> logger, IAuditDbService? auditService = null) : base(options)
  {
    _options = options;
    _auditService = auditService;
    Logger = logger ?? throw new ArgumentException($"{nameof(logger)} is null.");
    Mediator = mediator ?? throw new ArgumentException($"{nameof(mediator)} is null.");
  }


  #region Audit

  public override int SaveChanges(bool acceptAllChangesOnSuccess)
  {
    Logger.LogError($"Don't use {nameof(SaveChanges)} in EF. Use {nameof(SaveChangesAsync)}.");
    if (_auditService == null || !IsAuditEnabledAsync().Result)
    {
      return SaveChangesAsync(CancellationToken.None).Result;
    }
    
    var entityAudits = _auditService.OnBeforeSaveChangesAsync(ChangeTracker).Result;
    var result = base.SaveChanges(acceptAllChangesOnSuccess);
    _auditService.OnAfterSaveChangesAsync(entityAudits).Wait();
    return result;
  }

  public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
  {
    if (_auditService == null || !(await IsAuditEnabledAsync()))
      return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

    var entityAudits = await _auditService.OnBeforeSaveChangesAsync(ChangeTracker);
    var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    await _auditService.OnAfterSaveChangesAsync(entityAudits);

    return result;
  }

  #endregion

  #region Settings

  public async Task<string?> Setting_GetAsync(string key, bool isRequired = true)
  {
    var vv = await GetSettingsAsync(key, isRequired);
    if (vv == null)
      return null;
    return vv.Value;
  }

  public async Task Setting_SaveAsync(string key, string value, bool isSystem = false)
  {
    var set = await Settings.FirstOrDefaultAsync(i => i.Key == key);
    if (set == null)
    {
      set = new SettingEntity
      {
        Key = key
      };
      Settings.Add(set);
    }

    set.Value = value;
    set.IsSystem = isSystem;

    await SaveChangesAsync();

    await Mediator.Send(new CacheModuleRemoveCommand(CacheKeyTableSetting));
  }

  private async Task<SettingEntity?> GetSettingsAsync(string key, bool exceptedValue = true)
  {
    List<SettingEntity> allSettings;

    var allSettingsCache = await Mediator.Send(new CacheModuleGetQuery(CacheKeyTableSetting));

    if (allSettingsCache != null)
    {
      if (allSettingsCache.Value == null)
      {
        var ex = new Exception("The key '" + key + "' is not represented in settings table.");
        Logger.LogCritical("GetSettingsValue->" + key, ex);
        throw ex;
      }

      allSettings = (allSettingsCache.Value as List<SettingEntity>)!;
    }
    else
    {
      allSettings = await Settings.ToListAsync();
      await Mediator.Send(new CacheModuleSaveCommand(CacheKeyTableSetting, allSettings));
    }

    if (allSettings == null)
      throw new ArgumentException($"{nameof(Settings)} entity table is null.");

    var vv = allSettings.FirstOrDefault(a => a.Key == key);
    if (vv == null && exceptedValue)
      throw new Exception($"Value for setting {nameof(key)} is not set. Check {nameof(Settings)} table.");

    return vv;
  }

  #endregion

  protected static void SetDatabaseNames<T>(Dictionary<Type, StorageEntityNameDefinition> objectNameMapping , ModelBuilder modelBuilder) where T: class
  {
    if (objectNameMapping.TryGetValue(typeof(T), out var auditColumnEntityObjectNames))
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
  
  public async Task UpdateDatabase<T>(T impl) where T : DbContextBase
  {
    var allVersions = UpdateScripts.AllVersions.ToList();

    var lastVersion = new Version("0.0.0.0");

    // Get the latest implemented version, if any.
    if (!await DbIsEmpty())
    {
      var ver = await Mediator.Send(new SettingGetQuery(StorageDefinition.Type, StorageVersionKey));
      if (ver != null)
        lastVersion = new Version(ver);
    }

    var updatedToVersion = new Version("0.0.0.0");

    if (allVersions.Count != 0)
    {
      if (StorageDefinition.IsTransactionEnabled)
      {
        await using var transaction = await Database.BeginTransactionAsync();
        updatedToVersion = await UpdateDatabase(impl, allVersions, lastVersion);
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
        updatedToVersion = await UpdateDatabase(impl, allVersions, lastVersion);
      }
    }

    await Mediator.Send(new SettingSaveCommand(StorageDefinition.Type, StorageVersionKey, updatedToVersion.ToString(), true));
  }

  private async Task<Version> UpdateDatabase<T>(T impl, List<DbVersionScriptsBase> allVersions, Version lastVersion) where T : DbContextBase
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

      version.AfterScriptRunCode(impl, _options, Logger);
      updatedToVersion = version.Version;
    }

    return updatedToVersion;
  }


  private async Task<bool> DbIsEmpty()
  {
    var res = true;
    try
    {
      var isSettingTable = await Mediator.Send(new SettingGetQuery(StorageDefinition.Type, StorageVersionBaseSettingKey));
      res = isSettingTable == null;
    }
    catch
    {
      Logger.LogDebug("Setting table has not been found.");
    }

    return res;
  }

  /// <summary>
  /// Resolves problem with this situation. We have settings table where is audit on and audit structure is not created yet.
  /// In this case is audit will be skipped.  
  /// </summary>
  private async Task<bool> IsAuditEnabledAsync()
  {
    if (_auditService == null)
      return false;

    // Check if db structure is already created.
    var isAuditTable = await Mediator.Send(new SettingGetQuery(StorageDefinition.Type, AuditSettingKey));

    return !string.IsNullOrEmpty(isAuditTable);
  }
}