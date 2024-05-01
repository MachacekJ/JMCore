using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.PGStorage.BasicModule;
using JMCore.Server.Storages.Modules;
using JMCore.Server.Storages.Modules.BasicModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Server.PGStorage;

public class PGStorageConfiguration(string connectionString, StorageNativeModuleTypeEnum coreNativeModuleName) : StorageConfigurationItem(coreNativeModuleName)
{
  public override StorageTypeEnum StorageType { get; } = StorageTypeEnum.Postgres;

  public override void RegisterServices(IServiceCollection services)
  {
    services.AddDbContext<BasicEfStorage>(opt => opt.UseNpgsql(connectionString));
  }

  public override async Task ConfigureServices(IServiceProvider serviceProvider)
  {
    await ConfigureEfSqlServiceLocal<IBasicStorageModule, BasicEfStorage>(serviceProvider);
  }
}