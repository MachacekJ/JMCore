using ACore.Modules.CacheModule.CQRS.CacheGet;
using ACore.Modules.CacheModule.CQRS.CacheRemove;
using ACore.Modules.CacheModule.CQRS.CacheSave;
using ACore.Modules.CacheModule.CQRS.Models;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.SettingModule.Storage.SQL.Models;
using ACore.Server.Services.JMCache;
using ACore.Server.Storages.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.SettingModule.Storage.SQL;

internal abstract class SettingModuleStorageImpl(DbContextOptions options, IMediator mediator, IAuditConfiguration? auditConfiguration, ILogger<SettingModuleStorageImpl> logger)
  : AuditableDbContext(options, mediator, logger, auditConfiguration), ISettingStorageModule
{
  private static readonly JMCacheKey CacheKeyTableSetting = JMCacheKey.Create(JMCacheServerCategory.DbTable, nameof(SettingEntity));

  protected override string ModuleName => nameof(ISettingStorageModule);

  public DbSet<SettingEntity> Settings { get; set; }

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
    List<SettingEntity>? allSettings;
  
    var allSettingsCache = await Mediator.Send(new CacheModuleGetQuery(CacheKeyTableSetting));
  
    if (allSettingsCache != null)
    {
      if (allSettingsCache.Value == null)
      {
        var ex = new Exception("The key '" + key + "' is not represented in settings table.");
        Logger.LogCritical("GetSettingsValue->" + key, ex);
        throw ex;
      }
  
      allSettings = allSettingsCache.Value as List<SettingEntity>;
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
  
  public override Task<TEntity?> Get<TEntity, TPK>(TPK id) where TEntity : class
  {
    throw new NotImplementedException();
  }

  protected SettingModuleStorageImpl(DbContextOptions options, IMediator mediator, ILogger<SettingModuleStorageImpl> logger) : this(options, mediator, null, logger)
  {
  }
}