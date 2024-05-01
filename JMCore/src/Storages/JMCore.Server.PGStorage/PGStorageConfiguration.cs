using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.PGStorage.BasicModule;
using JMCore.Server.Storages.Modules.BasicModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Server.PGStorage;

public class PGStorageConfiguration(string connectionString, IEnumerable<string> requiredStorageModules) : StorageConfigurationBase(requiredStorageModules)
{
  public override StorageTypeEnum StorageType => StorageTypeEnum.Postgres;

  public override void RegisterServices(IServiceCollection services)
  {
    services.AddDbContext<BasicEfStorage>(opt => opt.UseNpgsql(connectionString));
  }

  public override async Task ConfigureServices(IServiceProvider serviceProvider)
  {
    await ConfigureEfSqlServiceLocal<IBasicStorageModule, BasicEfStorage>(serviceProvider);
  }
}