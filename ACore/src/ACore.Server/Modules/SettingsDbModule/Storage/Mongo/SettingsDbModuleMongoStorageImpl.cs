using ACore.Base.Cache;
using ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheGet;
using ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheRemove;
using ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheSave;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.SettingsDbModule.Storage.Mongo.Models;
using ACore.Server.Storages.EF;
using ACore.Server.Storages.Models;
using ACore.Server.Storages.Scripts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.EntityFrameworkCore.Extensions;

namespace ACore.Server.Modules.SettingsDbModule.Storage.Mongo;

internal class SettingsDbModuleMongoStorageImpl(DbContextOptions<SettingsDbModuleMongoStorageImpl> options, IMediator mediator, ILogger<SettingsDbModuleMongoStorageImpl> logger) : AuditableDbContext(options, mediator, logger), ISettingsDbModuleStorage
{
  private static readonly CacheKey CacheKeyTableSetting = CacheKey.Create(CacheCategories.Entity, nameof(SettingsPKMongoEntity));

  public override DbScriptBase UpdateScripts => new Scripts.ScriptRegistrations();
  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Mongo);
  protected override string ModuleName => nameof(ISettingsDbModuleStorage);

  public DbSet<SettingsPKMongoEntity> Settings { get; set; }

  #region Settings

  public async Task<string?> Setting_GetAsync(string key, bool isRequired = true)
    => (await GetSettingsAsync(key, isRequired))?.Value;


  public async Task Setting_SaveAsync(string key, string value, bool isSystem = false)
  {
    var setting = await Settings.FirstOrDefaultAsync(i => i.Key == key);
    if (setting == null)
    {
      setting = new SettingsPKMongoEntity
      {
        Key = key
      };
      Settings.Add(setting);
    }

    setting.Value = value;
    setting.IsSystem = isSystem;

    await SaveChangesAsync();

    await Mediator.Send(new MemoryCacheModuleRemoveKeyCommand(CacheKeyTableSetting));
  }

  private async Task<SettingsPKMongoEntity?> GetSettingsAsync(string key, bool exceptedValue = true)
  {
    List<SettingsPKMongoEntity>? allSettings;

    var allSettingsCache = await Mediator.Send(new MemoryCacheModuleGetQuery(CacheKeyTableSetting));

    if (allSettingsCache != null)
    {
      if (allSettingsCache.ResultValue == null)
      {
        var ex = new Exception("The key '" + key + "' is not represented in settings table.");
        Logger.LogCritical("GetSettingsValue->" + key, ex);
        throw ex;
      }

      allSettings = allSettingsCache.ResultValue.ObjectValue as List<SettingsPKMongoEntity>;
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


  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<SettingsPKMongoEntity>().ToCollection(CollectionNames.ObjectNameMapping[nameof(SettingsPKMongoEntity)].TableName);
    SetDatabaseNames<SettingsPKMongoEntity>(modelBuilder);
  }
  
  private static void SetDatabaseNames<T>(ModelBuilder modelBuilder) where T : class => SetDatabaseNames<T>(CollectionNames.ObjectNameMapping, modelBuilder);
}