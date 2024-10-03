using ACore.Base.Cache;
using ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheGet;
using ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheRemove;
using ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheSave;
using ACore.Server.Modules.SettingsDbModule.Storage.Mongo.Models;
using ACore.Server.Storages;
using ACore.Server.Storages.Definitions;
using ACore.Server.Storages.Definitions.EF;
using ACore.Server.Storages.Definitions.EF.Base;
using ACore.Server.Storages.Definitions.EF.Base.Scripts;
using ACore.Server.Storages.Definitions.EF.MongoStorage;
using ACore.Server.Storages.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.EntityFrameworkCore.Extensions;

namespace ACore.Server.Modules.SettingsDbModule.Storage.Mongo;

internal class SettingsDbModuleMongoStorageImpl(DbContextOptions<SettingsDbModuleMongoStorageImpl> options, IMediator mediator, ILogger<SettingsDbModuleMongoStorageImpl> logger) : DbContextBase(options, mediator, logger), ISettingsDbModuleStorage
{
  private static readonly CacheKey CacheKeyTableSetting = CacheKey.Create(CacheCategories.Entity, nameof(SettingsPKMongoEntity));

  protected override DbScriptBase UpdateScripts => new Scripts.ScriptRegistrations();
  protected override EFStorageDefinition EFStorageDefinition => new MongoStorageDefinition();
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

    await mediator.Send(new MemoryCacheModuleRemoveKeyCommand(CacheKeyTableSetting));
  }

  private async Task<SettingsPKMongoEntity?> GetSettingsAsync(string key, bool exceptedValue = true)
  {
    List<SettingsPKMongoEntity>? allSettings;

    var allSettingsCache = await mediator.Send(new MemoryCacheModuleGetQuery(CacheKeyTableSetting));

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
      await mediator.Send(new MemoryCacheModuleSaveCommand(CacheKeyTableSetting, allSettings));
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