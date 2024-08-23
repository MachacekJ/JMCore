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

internal abstract class SettingModuleSqlStorageImpl(DbContextOptions options, IMediator mediator, IAuditConfiguration? auditConfiguration, ILogger<SettingModuleSqlStorageImpl> logger)
  : AuditableDbContext(options, mediator, logger, auditConfiguration), ISettingStorageModule
{
  private static readonly JMCacheKey CacheKeyTableSetting = JMCacheKey.Create(JMCacheServerCategory.DbTable, nameof(SettingEntity));

  protected abstract int IdIntGenerator<T>() where T : class;

  protected override string ModuleName => nameof(ISettingStorageModule);

  public DbSet<SettingEntity> Settings { get; set; }

  #region Settings

  public async Task<string?> Setting_GetAsync(string key, bool isRequired = true)
    => (await GetSettingsAsync(key, isRequired))?.Value;

  public async Task Setting_SaveAsync(string key, string value, bool isSystem = false)
  {
    var set = await Settings.FirstOrDefaultAsync(i => i.Key == key)
              ?? new SettingEntity
              {
                Key = key
              };

    set.Value = value;
    set.IsSystem = isSystem;

    var res = await SaveInternalWithAudit(set, set.Id, async (a) => await Settings.AddAsync(a), (i) => i.Id = IdIntGenerator<SettingEntity>());
    set.Id = res;

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

  public override async Task<TEntity?> Get<TEntity, TPK>(TPK id) where TEntity : class
  {
    if (id == null)
      throw new ArgumentNullException($"{nameof(id)} is null.");

    var res = typeof(TEntity) switch
    {
      { } entityType when entityType == typeof(SettingEntity) => await Settings.SingleOrDefaultAsync(i => i.Id == Convert.ToInt32(id)) as TEntity,
      _ => throw new Exception($"Unknown entity data type {typeof(TEntity).Name} with primary key {id}.")
    };
    return res ?? throw new ArgumentNullException(nameof(res), @"Save function returned null value.");
  }

  protected SettingModuleSqlStorageImpl(DbContextOptions options, IMediator mediator, ILogger<SettingModuleSqlStorageImpl> logger) : this(options, mediator, null, logger)
  {
  }
}