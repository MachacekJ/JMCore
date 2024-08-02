using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.MongoStorage.AuditModule;
using ACore.Server.MongoStorage.SettingModule;
using ACore.Server.Storages.Configuration;
using ACore.Server.Storages.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Server.MongoStorage;

public class MongoStorageConfiguration(string connectionString, string dbName, IEnumerable<string> requiredStorageModules) : StorageConfigurationBase(requiredStorageModules)
{
  public override StorageTypeEnum StorageType => StorageTypeEnum.Mongo;

  public override void RegisterServices(IServiceCollection services)
  {
    services.AddDbContext<BasicSqlMongoEfStorageImpl>(opt => opt.UseMongoDB(connectionString, dbName));
    services.AddSingleton<IBasicStorageModule, BasicSqlMongoEfStorageImpl>();
    foreach (var requiredStorageModule in RequiredStorageModules)
    {
      switch (requiredStorageModule)
      {
        case nameof(IBasicStorageModule):
          break;
        case nameof(IAuditStorageModule):
          services.AddDbContext<AuditMongoStorageImpl>(opt => opt.UseMongoDB(connectionString, dbName));
          services.AddSingleton<IAuditStorageModule, AuditMongoStorageImpl>();
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
    await ConfigureEfSqlServiceLocal<IBasicStorageModule, BasicSqlMongoEfStorageImpl>(serviceProvider);
    foreach (var requiredStorageModule in RequiredStorageModules)
    {
      switch (requiredStorageModule)
      {
        case nameof(IBasicStorageModule):
          break;
        case nameof(IAuditStorageModule):
          await ConfigureEfSqlServiceLocal<IAuditStorageModule, AuditMongoStorageImpl>(serviceProvider);
          break;
        // case nameof(ILocalizationStorageModule):
        //   await ConfigureEfSqlServiceLocal<ILocalizationStorageModule, LocalizationEfStorageImpl>(serviceProvider);
        //   break;
      }
    }
  }
}