using System.Reflection;
using ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbGet;
using ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbSave;
using ACore.Server.Modules.SettingsDbModule.Storage;
using ACore.Server.Storages.Definitions.EF.Base.Scripts;
using ACore.Server.Storages.Definitions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Storages.Definitions.EF.Base;

public abstract partial class DbContextBase
{
  private string StorageVersionBaseSettingKey => $"StorageVersion_{Enum.GetName(typeof(StorageTypeEnum), StorageDefinition.Type)}_{nameof(ISettingsDbModuleStorage)}";
  private string StorageVersionKey => $"StorageVersion_{Enum.GetName(typeof(StorageTypeEnum), StorageDefinition.Type)}_{ModuleName}";

  public async Task UpSchema()
  {
    var allVersions = UpdateScripts.AllVersions.ToList();

    var lastVersion = new Version("0.0.0.0");

    // Get the latest implemented version, if any.
    if (!await DbIsEmpty())
    {
      var ver = await mediator.Send(new SettingsDbGetQuery(StorageDefinition.Type, StorageVersionKey));
      if (ver is { IsSuccess: true, ResultValue: not null })
        lastVersion = new Version(ver.ResultValue);
    }

    var updatedToVersion = new Version("0.0.0.0");

    if (allVersions.Count != 0)
    {
      if (EFStorageDefinition.IsTransactionEnabled)
      {
        await using var transaction = await Database.BeginTransactionAsync();
        try
        {
          updatedToVersion = await UpdateSchema(allVersions, lastVersion);
          await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
          await transaction.RollbackAsync();
          throw new Exception("UpdateDbAsync is rollback", ex);
        }
      }
      else
      {
        updatedToVersion = await UpdateSchema(allVersions, lastVersion);
      }
    }

    if (this is ISettingsDbModuleStorage aa)
    {
      await aa.Setting_SaveAsync(StorageVersionKey, updatedToVersion.ToString(), true);
      return;
    }

    await mediator.Send(new SettingsDbSaveCommand(StorageDefinition.Type, StorageVersionKey, updatedToVersion.ToString(), true));
  }

  private async Task<Version> UpdateSchema(List<DbVersionScriptsBase> allVersions, Version lastVersion)
  {
    var updatedToVersion = new Version("0.0.0.0");

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

      version.AfterScriptRunCode(this, _options, Logger);
      updatedToVersion = version.Version;
    }

    return updatedToVersion;
  }

  private async Task<bool> DbIsEmpty()
  {
    var res = true;
    try
    {
      var isSettingTable = await mediator.Send(new SettingsDbGetQuery(StorageDefinition.Type, StorageVersionBaseSettingKey));
      res = isSettingTable is { IsSuccess: true, ResultValue: null };
    }
    catch
    {
      Logger.LogDebug("Setting table has not been found.");
    }

    return res;
  }
}