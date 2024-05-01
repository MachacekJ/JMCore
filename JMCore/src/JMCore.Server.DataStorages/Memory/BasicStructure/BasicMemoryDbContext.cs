using JMCore.CQRS.JMCache.CacheGet;
using JMCore.CQRS.JMCache.CacheRemove;
using JMCore.CQRS.JMCache.CacheSave;
using JMCore.Server.DataStorages.PG.BasicStructure;
using JMCore.Server.DataStorages.PG.BasicStructure.Scripts;
using JMCore.Server.DB.Abstract;
using JMCore.Server.DB.Audit;
using JMCore.Server.DB.DbContexts.BasicStructure;
using JMCore.Server.DB.DbContexts.BasicStructure.Models;
using JMCore.Server.Services.JMCache;
using JMCore.Services.JMCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace JMCore.Server.DataStorages.Memory.BasicStructure;

public class BasicMemoryDbContext : DbContextBase, IBasicDbContext
{
  private static readonly JMCacheKey CacheKeyTableSetting = JMCacheKey.Create(JMCacheServerCategory.DbTable, nameof(ISettingEntity));

  public override DbScriptBase SqlScripts => new ScriptRegistrations();
  public override string DbContextName => GetType().Name;

  private readonly ILogger<BasicPGDbContext> _logger;

  public DbSet<PG.BasicStructure.Models.SettingEntity> Settings { get; set; }


  public BasicMemoryDbContext(DbContextOptions<BasicPGDbContext> options, IMediator mediator, ILogger<BasicPGDbContext> logger) : this(options, mediator, null, logger)
  {
  }

  public BasicMemoryDbContext(DbContextOptions<BasicPGDbContext> options, IMediator mediator, IAuditDbService? auditService, ILogger<BasicPGDbContext> logger) : base(options, mediator, logger, auditService)
  {
    _logger = logger;
  }

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
      set = new PG.BasicStructure.Models.SettingEntity();
      Settings.Add(set);
    }

    set.Value = value;
    set.IsSystem = isSystem;
    set.Key = key;

    await SaveChangesAsync();

    await Mediator.Send(new CacheRemoveCommand(CacheKeyTableSetting));
  }

  private async Task<ISettingEntity?> GetSettingsAsync(string key, bool exceptedValue = true)
  {
    List<PG.BasicStructure.Models.SettingEntity> allSettings;

    var allSettingsCache = await Mediator.Send(new CacheGetQuery(CacheKeyTableSetting));

    if (allSettingsCache != null)
    {
      if (allSettingsCache.Value == null)
      {
        var ex = new Exception("The key '" + key + "' is not represented in settings table.");
        _logger.LogCritical("GetSettingsValue->" + key, ex);
        throw ex;
      }

      allSettings = (allSettingsCache.Value as List<PG.BasicStructure.Models.SettingEntity>)!;
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
}