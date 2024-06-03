using System.Reflection;
using JMCore.CQRS.JMCache.CacheGet;
using JMCore.CQRS.JMCache.CacheRemove;
using JMCore.CQRS.JMCache.CacheSave;
using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.CQRS.Storages.BasicModule.SettingGet;
using JMCore.Server.CQRS.Storages.BasicModule.SettingSave;
using JMCore.Server.Services.JMCache;
using JMCore.Server.Storages.Base.Audit.EF;
using JMCore.Server.Storages.Modules.AuditModule;
using JMCore.Server.Storages.Modules.BasicModule;
using JMCore.Server.Storages.Modules.BasicModule.Models;
using JMCore.Services.JMCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JMCore.Server.Storages.Base.EF;

/// <summary>
/// This base class implements audit functionality and calls db <see cref="SaveChangesAsync"/> method.
/// Use <see cref="SaveChanges"/> only in special cases.
/// </summary>
public abstract class DbContextBase : DbContext, IDbContextBase, IBasicStorageModule
{
  private static readonly JMCacheKey CacheKeyTableSetting = JMCacheKey.Create(JMCacheServerCategory.DbTable, nameof(SettingEntity));
  private string AuditSettingKey => $"StorageVersion_{Enum.GetName(typeof(StorageTypeEnum), StorageType)}_{nameof(IAuditStorageModule)}";
  private string StorageVersionBaseSettingKey => $"StorageVersion_{Enum.GetName(typeof(StorageTypeEnum), StorageType)}_{nameof(IBasicStorageModule)}";
  private string StorageVersionKey => $"StorageVersion_{Enum.GetName(typeof(StorageTypeEnum), StorageType)}_{ModuleName}";


  protected readonly ILogger<DbContextBase> Logger;
  private readonly IAuditDbService? _auditService;

  /// <summary>
  /// This base class implements audit functionality and calls db <see cref="SaveChangesAsync"/> method.
  /// Use <see cref="SaveChanges"/> only in special cases.
  /// </summary>
  protected DbContextBase(DbContextOptions options, IMediator mediator, ILogger<DbContextBase> logger, IAuditDbService? auditService = null) : base(options)
  {
    _auditService = auditService;
    Logger = logger ?? throw new ArgumentException($"{nameof(logger)} is null.");
    Mediator = mediator ?? throw new ArgumentException($"{nameof(mediator)} is null.");
  }

  public abstract DbScriptBase UpdateScripts { get; }
  public abstract StorageTypeEnum StorageType { get; }
  public abstract string ModuleName { get; }


  protected IMediator Mediator { get; }

  public DbSet<SettingEntity> Settings { get; set; }


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
      set = new SettingEntity();
      Settings.Add(set);
    }

    set.Value = value;
    set.IsSystem = isSystem;
    set.Key = key;

    await SaveChangesAsync();

    await Mediator.Send(new CacheRemoveCommand(CacheKeyTableSetting));
  }

  private async Task<SettingEntity?> GetSettingsAsync(string key, bool exceptedValue = true)
  {
    List<SettingEntity> allSettings;

    var allSettingsCache = await Mediator.Send(new CacheGetQuery(CacheKeyTableSetting));

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
      await Mediator.Send(new CacheSaveCommand(CacheKeyTableSetting, allSettings));
    }

    if (allSettings == null)
      throw new ArgumentException($"{nameof(Settings)} entity table is null.");

    var vv = allSettings.FirstOrDefault(a => a.Key == key);
    if (vv == null && exceptedValue)
      throw new Exception($"Value for setting {nameof(key)} is not set. Check {nameof(Settings)} table.");

    return vv;
  }

  public async Task Init()
  {
    var allVersions = UpdateScripts.AllVersions.ToList();

    var lastVersion = new Version("0.0.0.0");
    
    // Get the latest implemented version, if any.
    if (!await DbIsEmpty())
    {
      var ver = await Mediator.Send(new SettingGetQuery(StorageType, StorageVersionKey));
      if (ver != null)
        lastVersion = new Version(ver);
    }

    var maxVersion = new Version("0.0.0.0");

    if (allVersions.Any())
    {
      await using var transaction = await Database.BeginTransactionAsync();
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

        if (maxVersion < version.Version)
          maxVersion = version.Version;
      }

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

    //if (maxVersion > new Version("0.0.0.0"))
    await Mediator.Send(new SettingSaveCommand(StorageType, StorageVersionKey, maxVersion.ToString(), true));
  }

  private async Task<bool> DbIsEmpty()
  {
    var res = true;
    try
    {
      var isSettingTable = await Mediator.Send(new SettingGetQuery(StorageType, StorageVersionBaseSettingKey));
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

    // return true;
    var isAuditTable = await Mediator.Send(new SettingGetQuery(StorageType, AuditSettingKey));

    return !string.IsNullOrEmpty(isAuditTable);
  }
}