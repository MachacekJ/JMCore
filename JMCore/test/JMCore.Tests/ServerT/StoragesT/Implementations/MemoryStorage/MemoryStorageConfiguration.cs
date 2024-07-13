using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Modules.AuditModule;
using JMCore.Server.Storages.Modules.BasicModule;
using JMCore.Server.Storages.Modules.LocalizationModule;
using JMCore.Tests.ServerT.StoragesT.Implementations.MemoryStorage.Modules;
using JMCore.Tests.ServerT.StoragesT.Implementations.TestStorageModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.ServerT.StoragesT.Implementations.MemoryStorage;

public class MemoryStorageConfiguration(IEnumerable<string> requiredStorageModules) : StorageConfigurationBase(requiredStorageModules)
{
  private readonly string _connectionString = "memory";
  public override StorageTypeEnum StorageType => StorageTypeEnum.Memory;

  public override void RegisterServices(IServiceCollection services)
  {
    services.AddDbContext<BasicMemoryEfStorageImpl>(opt => opt.UseInMemoryDatabase(_connectionString + nameof(IBasicStorageModule) + Guid.NewGuid()));
    services.AddSingleton<IBasicStorageModule, BasicMemoryEfStorageImpl>();
    foreach (var requiredStorageModule in RequiredStorageModules)
    {
      switch (requiredStorageModule)
      {
        case nameof(IBasicStorageModule):
          break;
        case nameof(IAuditStorageModule):
          services.AddDbContext<AuditMemoryEfStorageImpl>(opt => opt.UseInMemoryDatabase(_connectionString + nameof(IAuditStorageModule) + Guid.NewGuid()));
          services.AddSingleton<IAuditStorageModule, AuditMemoryEfStorageImpl>();
          break;
        case nameof(ILocalizationStorageModule):
          services.AddDbContext<LocalizationMemoryEfStorageImpl>(opt => opt.UseInMemoryDatabase(_connectionString + nameof(ILocalizationStorageModule) + Guid.NewGuid()));
          services.AddSingleton<ILocalizationStorageModule, LocalizationMemoryEfStorageImpl>();
          break;
        case nameof(ITestStorageModule):
          services.AddDbContext<TestMemoryEfStorageImpl>(opt => opt.UseInMemoryDatabase(_connectionString + nameof(ITestStorageModule) + Guid.NewGuid()));
          services.AddSingleton<ITestStorageModule, TestMemoryEfStorageImpl>();
          break;
        default:
          throw new Exception($"Required storage module '{requiredStorageModule}' is not implemented");
      }
    }
  }

  public override async Task ConfigureServices(IServiceProvider serviceProvider)
  {
    await ConfigureEfSqlServiceLocal<IBasicStorageModule, BasicMemoryEfStorageImpl>(serviceProvider);
    foreach (var requiredStorageModule in RequiredStorageModules)
    {
      switch (requiredStorageModule)
      {
        case nameof(IBasicStorageModule):
          break;
        case nameof(IAuditStorageModule):
          await ConfigureEfSqlServiceLocal<IAuditStorageModule, AuditMemoryEfStorageImpl>(serviceProvider);
          break;
        case nameof(ILocalizationStorageModule):
          await ConfigureEfSqlServiceLocal<ILocalizationStorageModule, LocalizationMemoryEfStorageImpl>(serviceProvider);
          break;
        case nameof(ITestStorageModule):
          await ConfigureEfSqlServiceLocal<ITestStorageModule, TestMemoryEfStorageImpl>(serviceProvider);
          break;
        default:
          throw new Exception($"Required storage module '{requiredStorageModule}' is not implemented");
      }
    }
  }
}