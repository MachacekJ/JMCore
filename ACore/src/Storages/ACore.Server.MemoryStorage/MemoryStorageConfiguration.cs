using ACore.Server.MemoryStorage.AuditModule;
using ACore.Server.MemoryStorage.LocalizationModule;
using ACore.Server.MemoryStorage.SettingModule;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Modules.LocalizationModule.Storage;
using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Storages.Configuration;
using ACore.Server.Storages.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Server.MemoryStorage;

public class MemoryStorageConfiguration(IEnumerable<string> requiredStorageModules) : StorageConfigurationBase(requiredStorageModules)
{
  protected const string ConnectionString = "memory";
  public override StorageTypeEnum StorageType => StorageTypeEnum.Memory;

  public override void RegisterServices(IServiceCollection services)
  {
    services.AddDbContext<BasicSqlMemoryEfStorageImpl>(opt => opt.UseInMemoryDatabase(ConnectionString + nameof(IBasicStorageModule) + Guid.NewGuid()));
    services.AddSingleton<IBasicStorageModule, BasicSqlMemoryEfStorageImpl>();
    foreach (var requiredStorageModule in RequiredStorageModules)
    {
      switch (requiredStorageModule)
      {
        case nameof(IBasicStorageModule):
          break;
        case nameof(IAuditStorageModule):
          services.AddDbContext<AuditSqlMemoryStorageImpl>(opt => opt.UseInMemoryDatabase(ConnectionString + nameof(IAuditStorageModule) + Guid.NewGuid()));
          services.AddSingleton<IAuditStorageModule, AuditSqlMemoryStorageImpl>();
          break;
        case nameof(ILocalizationStorageModule):
          services.AddDbContext<LocalizationMemoryEfStorageImpl>(opt => opt.UseInMemoryDatabase(ConnectionString + nameof(ILocalizationStorageModule) + Guid.NewGuid()));
          services.AddSingleton<ILocalizationStorageModule, LocalizationMemoryEfStorageImpl>();
          break;
      }
    }
  }

  public override async Task ConfigureServices(IServiceProvider serviceProvider)
  {
    await ConfigureEfSqlServiceLocal<IBasicStorageModule, BasicSqlMemoryEfStorageImpl>(serviceProvider);
    foreach (var requiredStorageModule in RequiredStorageModules)
    {
      switch (requiredStorageModule)
      {
        case nameof(IBasicStorageModule):
          break;
        case nameof(IAuditStorageModule):
          await ConfigureEfSqlServiceLocal<IAuditStorageModule, AuditSqlMemoryStorageImpl>(serviceProvider);
          break;
        case nameof(ILocalizationStorageModule):
          await ConfigureEfSqlServiceLocal<ILocalizationStorageModule, LocalizationMemoryEfStorageImpl>(serviceProvider);
          break;
      }
    }
  }
}