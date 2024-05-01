using System.Reflection;
using JMCore.Server.CQRS.DB.BasicStructure.SettingGet;
using JMCore.Server.CQRS.DB.BasicStructure.SettingSave;
using JMCore.Server.Storages.DbContexts.BasicStructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JMCore.Server.Storages.Abstract;

/// <summary>
/// Not all class can inherit from <see cref="Microsoft.EntityFrameworkCore.DbContext"/> eg. IdentityDbContext.
/// This class help to implement <see cref="IBasicDbContext"/>
/// </summary>
public class DbContextBaseHelper
{
  private readonly IMediator _mediator;
  private readonly ILogger _logger;
  private readonly DbContext _db;
  private readonly string _dBContextName;
  private readonly DbScriptBase _sQlScripts;

  private string DbContextVersionKey => DbContextBase.DbContextVersionKeyPrefix + _dBContextName;

  public DbContextBaseHelper(DbContext db, IMediator mediator, ILogger logger, string dBContextName, DbScriptBase sQlScripts)
  {
    _db = db;
    _mediator = mediator;
    _logger = logger;
    _dBContextName = dBContextName;
    _sQlScripts = sQlScripts;
  }

  public async Task UpdateDbAsync(Func<Task> afterUpdateAsync)
  {
    var allVersions = _sQlScripts.AllVersions;

    var lastVersion = new Version("0.0.0.0");
    if (!await DbIsEmpty())
    {
      var ver = await _mediator.Send(new SettingGetQuery(DbContextVersionKey));
      if (ver != null)
        lastVersion = new Version(ver);
    }

    var maxVersion = new Version("0.0.0.0");

    await using (var transaction = await _db.Database.BeginTransactionAsync())
    {
      foreach (var version in allVersions.Where(a => a.Version > lastVersion))
      {
        foreach (var script in version.AllScripts)
        {
          try
          {
            // script can contains "GO" and splitting is needed to success executing
            foreach (string commandText in GetCommandScripts(script))
            {
              _logger.LogInformation("SQL scripts version " + version.Version + ":" +
                                     commandText);
              await _db.Database.ExecuteSqlRawAsync(commandText);
              _logger.LogInformation("OK");
            }
          }
          catch (Exception ex)
          {
            _logger.LogCritical(MethodBase.GetCurrentMethod()?.Name + " - Create tables in DB:", ex);

            throw new Exception("UpdateDB error for script ->" + script, ex);
          }
        }

        if (maxVersion < version.Version)
          maxVersion = version.Version;
      }

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

    if (maxVersion > new Version("0.0.0.0"))
      await _mediator.Send(new SettingSaveCommand(DbContextVersionKey, maxVersion.ToString(), true));

    await afterUpdateAsync();
  }

  private async Task<bool> DbIsEmpty()
  {
    var res = true;
    try
    {
      var isSettingTable = await _mediator.Send(new SettingGetQuery(DbContextBase.DbContextVersionKeyPrefix + nameof(BasicDbContext)));
      res = isSettingTable == null;
    }
    catch
    {
      _logger.LogDebug("Setting table has not been found.");
    }

    return res;
  }

  private static IEnumerable<string> GetCommandScripts(string script)
  {
    var commandTexts = script.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);
    foreach (var commandText in commandTexts)
    {
      yield return commandText;
    }
  }
}