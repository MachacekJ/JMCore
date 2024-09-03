using System.Reflection;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Modules.SettingModule.CQRS.SettingGet;
using ACore.Server.Modules.SettingModule.CQRS.SettingSave;
using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Storages.Models;
using ACore.Server.Storages.Scripts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Storages.EF;

public abstract class DbContextBase(DbContextOptions options, IMediator mediator, ILogger<DbContextBase> logger)
  : DbContext(options), IStorage
{
  private string AuditSettingKey => $"StorageVersion_{Enum.GetName(typeof(StorageTypeEnum), StorageDefinition.Type)}_{nameof(IAuditStorageModule)}";

  private bool? _isAuditEnabled;

  public abstract DbScriptBase UpdateScripts { get; }
  public abstract StorageTypeDefinition StorageDefinition { get; }
  protected abstract string ModuleName { get; }

  private string StorageVersionBaseSettingKey => $"StorageVersion_{Enum.GetName(typeof(StorageTypeEnum), StorageDefinition.Type)}_{nameof(ISettingStorageModule)}";
  private string StorageVersionKey => $"StorageVersion_{Enum.GetName(typeof(StorageTypeEnum), StorageDefinition.Type)}_{ModuleName}";

  protected readonly ILogger<DbContextBase> Logger = logger ?? throw new ArgumentException($"{nameof(logger)} is null.");
  private readonly DbContextOptions _options = options;

  protected IMediator Mediator { get; } = mediator ?? throw new ArgumentException($"{nameof(mediator)} is null.");


  protected static void SetDatabaseNames<T>(Dictionary<string, StorageEntityNameDefinition> objectNameMapping, ModelBuilder modelBuilder) where T : class
  {
    if (objectNameMapping.TryGetValue(typeof(T).Name, out var auditColumnEntityObjectNames))
    {
      modelBuilder.Entity<T>().ToTable(auditColumnEntityObjectNames.TableName);
      foreach (var expression in auditColumnEntityObjectNames.GetColumns<T>())
      {
        modelBuilder.Entity<T>().Property(expression.Key).HasColumnName(expression.Value);
      }
    }
    else
    {
      throw new Exception($"Missing database name definition for entity: {typeof(T).Name}");
    }
  }

  public async Task UpdateDatabase()
  {
    var allVersions = UpdateScripts.AllVersions.ToList();

    var lastVersion = new Version("0.0.0.0");

    // Get the latest implemented version, if any.
    if (!await DbIsEmpty())
    {
      var ver = await Mediator.Send(new SettingGetQuery(StorageDefinition.Type, StorageVersionKey));
      if (ver is { IsSuccess: true, ResultValue: not null })
        lastVersion = new Version(ver.ResultValue);
    }

    var updatedToVersion = new Version("0.0.0.0");

    if (allVersions.Count != 0)
    {
      if (StorageDefinition.IsTransactionEnabled)
      {
        await using var transaction = await Database.BeginTransactionAsync();
        updatedToVersion = await UpdateDatabase(allVersions, lastVersion);
        try
        {
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
        updatedToVersion = await UpdateDatabase(allVersions, lastVersion);
      }
    }

    if (this is ISettingStorageModule aa)
    {
      await aa.Setting_SaveAsync(StorageVersionKey, updatedToVersion.ToString(), true);
      return;
    }

    await Mediator.Send(new SettingSaveCommand(StorageDefinition.Type, StorageVersionKey, updatedToVersion.ToString(), true));
  }

  private async Task<Version> UpdateDatabase(List<DbVersionScriptsBase> allVersions, Version lastVersion)
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
      var isSettingTable = await Mediator.Send(new SettingGetQuery(StorageDefinition.Type, StorageVersionBaseSettingKey));
      res = isSettingTable is { IsSuccess: true, ResultValue: null };
    }
    catch
    {
      Logger.LogDebug("Setting table has not been found.");
    }

    return res;
  }

  /// <summary>
  /// Resolves problem with this situation. We have settings table where is audit on and audit structure is not created yet.
  /// In this case is audit will be skipped.  
  /// </summary>
  protected async Task<bool> IsAuditEnabledAsync()
  {
    if (_isAuditEnabled != null)
      return _isAuditEnabled.Value;

    // Check if db structure is already created.
    var isAuditTable = await Mediator.Send(new SettingGetQuery(StorageDefinition.Type, AuditSettingKey));

    if (isAuditTable.IsSuccess && string.IsNullOrEmpty(isAuditTable.ResultValue))
    {
      _isAuditEnabled = false;
      return false;
    }

    _isAuditEnabled = true;
    return true;
  }
}