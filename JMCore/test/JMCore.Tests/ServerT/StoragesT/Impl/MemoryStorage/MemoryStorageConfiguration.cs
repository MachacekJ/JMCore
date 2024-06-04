using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Modules.AuditModule;
using JMCore.Server.Storages.Modules.BasicModule;
using JMCore.Server.Storages.Modules.LocalizationModule;
using JMCore.Tests.ServerT.StoragesT.Impl.MemoryStorage.Modules;
using JMCore.Tests.ServerT.StoragesT.Impl.TestStorageModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.ServerT.StoragesT.Impl.MemoryStorage;

public class MemoryStorageConfiguration(IEnumerable<string> requiredStorageModules) : StorageConfigurationBase(requiredStorageModules)
{
  private readonly string _connectionString = "memory";
  public override StorageTypeEnum StorageType => StorageTypeEnum.Memory;

  public override void RegisterServices(IServiceCollection services)
  {
    services.AddDbContext<BasicEfStorageImpl>(opt => opt.UseInMemoryDatabase(_connectionString + nameof(IBasicStorageModule) + Guid.NewGuid()));
    services.AddSingleton<IBasicStorageModule, BasicEfStorageImpl>();
    foreach (var requiredStorageModule in RequiredStorageModules)
    {
      switch (requiredStorageModule)
      {
        case nameof(IBasicStorageModule):
          break;
        case nameof(IAuditStorageModule):
          services.AddDbContext<AuditEfStorageImpl>(opt => opt.UseInMemoryDatabase(_connectionString + nameof(IAuditStorageModule) + Guid.NewGuid()));
          services.AddSingleton<IAuditStorageModule, AuditEfStorageImpl>();
          break;
        case nameof(ILocalizationStorageModule):
          services.AddDbContext<LocalizationEfStorageImpl>(opt => opt.UseInMemoryDatabase(_connectionString + nameof(ILocalizationStorageModule) + Guid.NewGuid()));
          services.AddSingleton<ILocalizationStorageModule, LocalizationEfStorageImpl>();
          break;
        case nameof(ITestStorageModule):
          services.AddDbContext<TestEfStorageImpl>(opt => opt.UseInMemoryDatabase(_connectionString + nameof(ILocalizationStorageModule) + Guid.NewGuid()));
          services.AddSingleton<ITestStorageModule, TestEfStorageImpl>();
          break;
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
        case nameof(ILocalizationStorageModule):
          await ConfigureEfSqlServiceLocal<ILocalizationStorageModule, LocalizationEfStorageImpl>(serviceProvider);
          break;
        case nameof(ITestStorageModule):
          await ConfigureEfSqlServiceLocal<ITestStorageModule, TestEfStorageImpl>(serviceProvider);
          break;
        default:
          throw new Exception($"Required storage module '{requiredStorageModule}' is not implemented");
      }
    }
  }
}