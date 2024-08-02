// using ACore.CQRS.JMCache.CacheGet;
// using ACore.CQRS.JMCache.CacheRemove;
// using ACore.CQRS.JMCache.CacheSave;
// using ACore.Server.MSSQLStorage.BasicStructure.Models;
// using ACore.Server.Services.JMCache;
// using ACore.Services.JMCache;
// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;
//
// #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
// // ReSharper disable UnusedAutoPropertyAccessor.Global
// // ReSharper disable UnusedAutoPropertyAccessor.Local
//
// namespace ACore.Server.MSSQLStorage.BasicStructure;
//
// public class BasicDbContext : DbContextBase, IBasicDbContext
// {
//     private static readonly JMCacheKey CacheKeyTableSetting = JMCacheKey.Create(JMCacheServerCategory.DbTable, nameof(SettingEntity));
//
//     public override DbScriptBase SqlScripts => new Scripts.ScriptRegistrations();
//     public override string DbContextName => GetType().Name;
//
//     private readonly ILogger<BasicDbContext> _logger;
//     
//     public DbSet<SettingEntity> Settings { get; set; }
//
//
//     public BasicDbContext(DbContextOptions<BasicDbContext> options, IMediator mediator, ILogger<BasicDbContext> logger) : this(options, mediator, null, logger)
//     {
//     }
//
//     public BasicDbContext(DbContextOptions<BasicDbContext> options, IMediator mediator, IAuditDbService? auditService, ILogger<BasicDbContext> logger) : base(options, mediator, logger, auditService)
//     {
//         _logger = logger;
//     }
//
//     public async Task<string?> Setting_GetAsync(string key, bool isRequired = true)
//     {
//         var vv = await GetSettingsAsync(key, isRequired);
//         if (vv == null)
//             return null;
//         return vv.Value;
//     }
//
//     public async Task Setting_SaveAsync(string key, string value, bool isSystem = false)
//     {
//         var set = await Settings.FirstOrDefaultAsync(i => i.Key == key);
//         if (set == null)
//         {
//             set = new SettingEntity();
//             Settings.Add(set);
//         }
//
//         set.Value = value;
//         set.IsSystem = isSystem;
//         set.Key = key;
//
//         await SaveChangesAsync();
//
//         await Mediator.Send(new CacheRemoveCommand(CacheKeyTableSetting));
//     }
//
//     private async Task<SettingEntity?> GetSettingsAsync(string key, bool exceptedValue = true)
//     {
//         List<SettingEntity> allSettings;
//
//         var allSettingsCache = await Mediator.Send(new CacheGetQuery(CacheKeyTableSetting));
//
//         if (allSettingsCache != null)
//         {
//             if (allSettingsCache.Value == null)
//             {
//                 var ex = new Exception("The key '" + key + "' is not represented in settings table.");
//                 _logger.LogCritical("GetSettingsValue->" + key, ex);
//                 throw ex;
//             }
//
//             allSettings = (allSettingsCache.Value as List<SettingEntity>)!;
//         }
//         else
//         {
//             allSettings = await Settings.ToListAsync();
//             await Mediator.Send(new CacheSaveCommand(CacheKeyTableSetting, allSettings));
//         }
//
//         if (allSettings == null)
//             throw new ArgumentException($"{nameof(Settings)} entity table is null.");
//
//         var vv = allSettings.FirstOrDefault(a => a.Key == key);
//         if (vv == null && exceptedValue)
//             throw new Exception($"Value for setting {nameof(key)} is not set. Check {nameof(Settings)} table.");
//
//         return vv;
//     }
// }