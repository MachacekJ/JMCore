using JMCore.Server.Configuration.Storage.Models;
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
    services.AddDbContext<BasicEfStorageImpl>(opt => opt.UseNpgsql(connectionString));
    services.AddSingleton<IBasicStorageModule, BasicEfStorageImpl>();
    foreach (var requiredStorageModule in RequiredStorageModules)
    {
      switch (requiredStorageModule)
      {
        case nameof(IBasicStorageModule):
          break;
        case nameof(IAuditStorageModule):
          services.AddDbContext<AuditEfStorageImpl>(opt => opt.UseNpgsql(connectionString));
          services.AddSingleton<IAuditStorageModule, AuditEfStorageImpl>();
          break;
        // case nameof(ILocalizationStorageModule):
        //   services.AddDbContext<LocalizationEfStorageImpl>(opt => opt.UseNpgsql(connectionString));
        //   services.AddSingleton<ILocalizationStorageModule, LocalizationEfStorageImpl>();
        //   break;
        // case nameof(ITestStorageModule):
        //   services.AddDbContext<TestEfStorageImpl>(opt => opt.UseNpgsql(connectionString));
        //   services.AddSingleton<ITestStorageModule, TestEfStorageImpl>();
        // break;
        default:
          throw new Exception($"Required storage module '{requiredStorageModule}' is not implemented");
      }
    }
  }

  public override async Task ConfigureServices(IServiceProvider serviceProvider)
  {
    await ConfigureEfSqlServiceLocal<IBasicStorageModule, BasicEfStorageImpl>(serviceProvider);
    foreach (var requiredStorageModule in RequiredStorageModules)
    {
      switch (requiredStorageModule)
      {
        case nameof(IBasicStorageModule):
          break;
        case nameof(IAuditStorageModule):
          await ConfigureEfSqlServiceLocal<IAuditStorageModule, AuditEfStorageImpl>(serviceProvider);
          break;
        // case nameof(ILocalizationStorageModule):
        //   await ConfigureEfSqlServiceLocal<ILocalizationStorageModule, LocalizationEfStorageImpl>(serviceProvider);
        //   break;
        // case nameof(ITestStorageModule):
        //   await ConfigureEfSqlServiceLocal<ITestStorageModule, TestEfStorageImpl>(serviceProvider);
        //   break;
        default:
          throw new Exception($"Required storage module '{requiredStorageModule}' is not implemented");
      }
    }
  }
}