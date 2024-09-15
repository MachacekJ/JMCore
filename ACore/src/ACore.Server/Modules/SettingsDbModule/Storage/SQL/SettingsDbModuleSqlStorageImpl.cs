using ACore.Base.Cache;
using ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheGet;
using ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheRemove;
using ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheSave;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.SettingsDbModule.Storage.SQL.Models;
using ACore.Server.Storages.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.SettingsDbModule.Storage.SQL;

internal abstract class SettingsDbModuleSqlStorageImpl : AuditableDbContext, ISettingsDbModuleStorage
{
  private static readonly CacheKey CacheKeyTableSetting = CacheKey.Create(CacheCategories.Entity, nameof(SettingsEntity));

  protected override string ModuleName => nameof(ISettingsDbModuleStorage);

  public DbSet<SettingsEntity> Settings { get; set; }

  #region Settings

  public async Task<string?> Setting_GetAsync(string key, bool isRequired = true)
    => (await GetSettingsAsync(key, isRequired))?.Value;

  public async Task Setting_SaveAsync(string key, string value, bool isSystem = false)
  {
    var set = await Settings.FirstOrDefaultAsync(i => i.Key == key)
              ?? new SettingsEntity
              {
                Key = key
              };

    set.Value = value;
    set.IsSystem = isSystem;

    await SaveWithAudit<SettingsEntity, int>(set);

    await Mediator.Send(new MemoryCacheModuleRemoveKeyCommand(CacheKeyTableSetting));
  }

  private async Task<SettingsEntity?> GetSettingsAsync(string key, bool exceptedValue = true)
  {
    List<SettingsEntity>? allSettings;

    var allSettingsCacheResult = await Mediator.Send(new MemoryCacheModuleGetQuery(CacheKeyTableSetting));

    if (allSettingsCacheResult.IsSuccess && allSettingsCacheResult.ResultValue != null)
    {
      if (allSettingsCacheResult.ResultValue.ObjectValue == null)
      {
        var ex = new Exception("The key '" + key + "' is not represented in settings table.");
        Logger.LogCritical("GetSettingsValue->" + key, ex);
        throw ex;
      }

      allSettings = allSettingsCacheResult.ResultValue.ObjectValue as List<SettingsEntity>;
    }
    else
    {
      allSettings = await Settings.ToListAsync();
      await Mediator.Send(new MemoryCacheModuleSaveCommand(CacheKeyTableSetting, allSettings));
    }

    if (allSettings == null)
      throw new ArgumentException($"{nameof(Settings)} entity table is null.");

    var vv = allSettings.FirstOrDefault(a => a.Key == key);
    if (vv == null && exceptedValue)
      throw new Exception($"Value for setting {nameof(key)} is not set. Check {nameof(Settings)} table.");

    return vv;
  }

  #endregion

  protected SettingsDbModuleSqlStorageImpl(DbContextOptions options, IMediator mediator, ILogger<SettingsDbModuleSqlStorageImpl> logger) : base(options, mediator, logger)
  {
    RegisterDbSet(Settings);
  }
}