﻿using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.PGStorage.AuditModule;
using JMCore.Server.PGStorage.BasicModule;
using JMCore.Server.Storages.Modules.AuditModule;
using JMCore.Server.Storages.Modules.BasicModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Server.PGStorage;

public class PGStorageConfiguration(string connectionString, IEnumerable<string> requiredStorageModules) : StorageConfigurationBase(requiredStorageModules)
{
  public override StorageTypeEnum StorageType => StorageTypeEnum.Postgres;

  public override void RegisterServices(IServiceCollection services)
  {
    services.AddDbContext<BasicSqlPGEfStorageImpl>(opt => opt.UseNpgsql(connectionString));
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