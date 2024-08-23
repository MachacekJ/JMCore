﻿using ACore.Modules.CacheModule.CQRS.CacheGet;
using ACore.Modules.CacheModule.CQRS.CacheRemove;
using ACore.Modules.CacheModule.CQRS.CacheSave;
using ACore.Modules.CacheModule.CQRS.Models;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.SettingModule.Storage.Mongo.Models;
using ACore.Server.Services.JMCache;
using ACore.Server.Storages.EF;
using ACore.Server.Storages.Models;
using ACore.Server.Storages.Scripts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.EntityFrameworkCore.Extensions;

namespace ACore.Server.Modules.SettingModule.Storage.Mongo;

internal class SettingModuleMongoStorageImpl : AuditableDbContext, ISettingStorageModule
{
  private static readonly JMCacheKey CacheKeyTableSetting = JMCacheKey.Create(JMCacheServerCategory.DbTable, nameof(SettingMongoEntity));

  public override DbScriptBase UpdateScripts => new Scripts.ScriptRegistrations();
  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Mongo);
  protected override string ModuleName => nameof(ISettingStorageModule);

  public DbSet<SettingMongoEntity> Settings { get; set; }

  #region Settings

  public async Task<string?> Setting_GetAsync(string key, bool isRequired = true)
    => (await GetSettingsAsync(key, isRequired))?.Value;


  public async Task Setting_SaveAsync(string key, string value, bool isSystem = false)
  {
    var setting = await Settings.FirstOrDefaultAsync(i => i.Key == key);
    if (setting == null)
    {
      setting = new SettingMongoEntity
      {
        Key = key
      };
      Settings.Add(setting);
    }

    setting.Value = value;
    setting.IsSystem = isSystem;

    await SaveChangesAsync();

    await Mediator.Send(new CacheModuleRemoveCommand(CacheKeyTableSetting));
  }

  private async Task<SettingMongoEntity?> GetSettingsAsync(string key, bool exceptedValue = true)
  {
    List<SettingMongoEntity>? allSettings;

    var allSettingsCache = await Mediator.Send(new CacheModuleGetQuery(CacheKeyTableSetting));

    if (allSettingsCache != null)
    {
      if (allSettingsCache.Value == null)
      {
        var ex = new Exception("The key '" + key + "' is not represented in settings table.");
        Logger.LogCritical("GetSettingsValue->" + key, ex);
        throw ex;
      }

      allSettings = allSettingsCache.Value as List<SettingMongoEntity>;
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

  public SettingModuleMongoStorageImpl(DbContextOptions<SettingModuleMongoStorageImpl> options, IMediator mediator, ILogger<SettingModuleMongoStorageImpl> logger) : base(options, mediator, logger, null)
  {
  }

  public SettingModuleMongoStorageImpl(DbContextOptions<SettingModuleMongoStorageImpl> options, IMediator mediator, IAuditConfiguration auditConfiguration, ILogger<SettingModuleMongoStorageImpl> logger) : base(options, mediator, logger, auditConfiguration)
  {
  }
  
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<SettingMongoEntity>().ToCollection(CollectionNames.ObjectNameMapping[nameof(SettingMongoEntity)].TableName);
    SetDatabaseNames<SettingMongoEntity>(modelBuilder);
  }
  
  private static void SetDatabaseNames<T>(ModelBuilder modelBuilder) where T : class => SetDatabaseNames<T>(CollectionNames.ObjectNameMapping, modelBuilder);
  
}