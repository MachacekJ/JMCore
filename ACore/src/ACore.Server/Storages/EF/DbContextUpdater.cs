// using System.Reflection;
// using ACore.Server.CQRS.DB.BasicStructure.SettingGet;
// using ACore.Server.CQRS.DB.BasicStructure.SettingSave;
// using ACore.Server.Storages.BasicModule;
// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;
//
// namespace ACore.Server.Storages.Base.EF;
//
// /// <summary>
// /// Not all class can inherit from <see cref="Microsoft.EntityFrameworkCore.DbContext"/> eg. IdentityDbContext.
// /// This class help to implement <see cref="IBasicStorageModule"/>
// /// </summary>
// public class DbContextUpdater(DbContext db, IMediator mediator, ILogger logger, string dBContextName, string storageName, DbScriptBase sQlScripts)
// {
//   private string DbContextVersionKey => DbContextBase.DbContextVersionKeyPrefix + dBContextName;
//
//   public async Task UpdateDbAsync(Func<Task> afterUpdateAsync)
//   {
//     var allVersions = sQlScripts.AllVersions.ToList();
//
//     var lastVersion = new Version("0.0.0.0");
//     if (!await DbIsEmpty())
//     {
//       var ver = await mediator.Send(new SettingGetQuery(DbContextVersionKey, ));
//       if (ver != null)
//         lastVersion = new Version(ver);
//     }
//
//     var maxVersion = new Version("0.0.0.0");
//
//     if (allVersions.Any())
//     {
//       await using var transaction = await db.Database.BeginTransactionAsync();
//       foreach (var version in allVersions.Where(a => a.Version > lastVersion))
//       {
//         foreach (var script in version.AllScripts)
//         {
//           try
//           {
//             // script can contains "GO" and splitting is needed to success executing
//             //  foreach (string commandText in GetCommandScripts(script))
//             // {
//             logger.LogInformation("SQL scripts version " + version.Version + ":" +
//                                    script);
//             await db.Database.ExecuteSqlRawAsync(script);
//             logger.LogInformation("OK");
//             //  }
//           }
//           catch (Exception ex)
//           {
//             logger.LogCritical(MethodBase.GetCurrentMethod()?.Name + " - Create tables in DB:", ex);
//
//             throw new Exception("UpdateDB error for script ->" + script, ex);
//           }
//         }
//
//         if (maxVersion < version.Version)
//           maxVersion = version.Version;
//       }
//
//       try
//       {
//         await transaction.CommitAsync();
//       }
//       catch (Exception ex)
//       {
//         await transaction.RollbackAsync();
//         throw new Exception("UpdateDbAsync is rollback", ex);
//       }
//     }
//
//     if (maxVersion > new Version("0.0.0.0"))
//       await mediator.Send(new SettingSaveCommand(DbContextVersionKey, maxVersion.ToString(), true));
//
//     await afterUpdateAsync();
//   }
//
//   private async Task<bool> DbIsEmpty()
//   {
//     var res = true;
//     try
//     {
//       var isSettingTable = await mediator.Send(new SettingGetQuery(DbContextBase.DbContextVersionKeyPrefix + nameof(IBasicStorageModule)));
//       res = isSettingTable == null;
//     }
//     catch
//     {
//       logger.LogDebug("Setting table has not been found.");
//     }
//
//     return res;
//   }
// }