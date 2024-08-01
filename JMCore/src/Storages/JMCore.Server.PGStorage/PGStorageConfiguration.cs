using System.Data.Common;
using JMCore.Server.Modules.AuditModule.Storage;
using JMCore.Server.Modules.SettingModule.Storage;
using JMCore.Server.PGStorage.AuditModule;
using JMCore.Server.PGStorage.SettingModule;
using JMCore.Server.Storages.Configuration;
using JMCore.Server.Storages.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Server.PGStorage;

public class PGStorageConfiguration(string connectionString, IEnumerable<string> requiredStorageModules) : StorageConfigurationBase(requiredStorageModules)
{
  public override StorageTypeEnum StorageType => StorageTypeEnum.Postgres;

  public override void RegisterServices(IServiceCollection services)
  {
    services.AddDbContext<BasicSqlPGEfStorageImpl>(opt =>
    {
      opt.UseNpgsql(connectionString);
      // opt.EnableSensitiveDataLogging();
      // opt.AddInterceptors(new SlowQueryDetectionHelper());
    });
    services.AddSingleton<IBasicStorageModule, BasicSqlPGEfStorageImpl>();
    foreach (var requiredStorageModule in RequiredStorageModules)
    {
      switch (requiredStorageModule)
      {
        case nameof(IBasicStorageModule):
          break;
        case nameof(IAuditStorageModule):
          services.AddDbContext<AuditSqlPGStorageImpl>(opt => opt.UseNpgsql(connectionString));
          services.AddSingleton<IAuditStorageModule, AuditSqlPGStorageImpl>();
          break;
        // case nameof(ILocalizationStorageModule):
        //   services.AddDbContext<LocalizationEfStorageImpl>(opt => opt.UseNpgsql(connectionString));
        //   services.AddSingleton<ILocalizationStorageModule, LocalizationEfStorageImpl>();
        //   break;
      }
    }
  }

  public override async Task ConfigureServices(IServiceProvider serviceProvider)
  {
    await ConfigureEfSqlServiceLocal<IBasicStorageModule, BasicSqlPGEfStorageImpl>(serviceProvider);
    foreach (var requiredStorageModule in RequiredStorageModules)
    {
      switch (requiredStorageModule)
      {
        case nameof(IBasicStorageModule):
          break;
        case nameof(IAuditStorageModule):
          await ConfigureEfSqlServiceLocal<IAuditStorageModule, AuditSqlPGStorageImpl>(serviceProvider);
          break;
        // case nameof(ILocalizationStorageModule):
        //   await ConfigureEfSqlServiceLocal<ILocalizationStorageModule, LocalizationEfStorageImpl>(serviceProvider);
        //   break;
      }
    }
  }
}

public  class SlowQueryDetectionHelper : DbCommandInterceptor
{
  private const int slowQueryThresholdInMilliSecond = 5000;
  public override ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
  {
    if (eventData.Duration.TotalMilliseconds > slowQueryThresholdInMilliSecond)
    {
      // Log.Warning($"Slow Query Detected. {command.CommandText}  TotalMilliSeconds: {eventData.Duration.TotalMilliseconds}");
    }
    return base.ReaderExecutedAsync(command, eventData, result,cancellationToken);
  }
  public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
  {
    if (eventData.Duration.TotalMilliseconds > slowQueryThresholdInMilliSecond)
    {
      //  Log.Warning($"Slow Query Detected. {command.CommandText}  TotalMilliSeconds: {eventData.Duration.TotalMilliseconds}");
    }
    return base.ReaderExecuted(command, eventData, result);
  }

  public override int NonQueryExecuted(DbCommand command, CommandExecutedEventData eventData, int result)
  {
    return base.NonQueryExecuted(command, eventData, result);
  }

  public override ValueTask<object?> ScalarExecutedAsync(DbCommand command, CommandExecutedEventData eventData, object? result, CancellationToken cancellationToken = new CancellationToken())
  {
    return base.ScalarExecutedAsync(command, eventData, result, cancellationToken);
  }

  public override ValueTask<int> NonQueryExecutedAsync(DbCommand command, CommandExecutedEventData eventData, int result, CancellationToken cancellationToken = new CancellationToken())
  {
    return base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
  }

  public override object? ScalarExecuted(DbCommand command, CommandExecutedEventData eventData, object? result)
  {
    if (eventData.Duration.TotalMilliseconds > slowQueryThresholdInMilliSecond)
    {
      //  Log.Warning($"Slow Query Detected. {command.CommandText}  TotalMilliSeconds: {eventData.Duration.TotalMilliseconds}");
    }
    return base.ScalarExecuted(command, eventData, result);
  }
}