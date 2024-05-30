using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Server.MongoStorage;

public class MongoStorageConfiguration(string connectionString, string dbName, IEnumerable<string> requiredStorageModules) : StorageConfigurationBase(requiredStorageModules)
{
  public override StorageTypeEnum StorageType { get; } = StorageTypeEnum.Mongo;
  public override void RegisterServices(IServiceCollection services)
  {
  //  services.AddDbContext<BasicMongoStorageModule>(opt => opt.UseMongoDB(connectionString, dbName));
  }

  public override async Task ConfigureServices(IServiceProvider serviceProvider)
  {
   // await ConfigureServiceLocal<IBasicStorageModule, BasicMongoStorageModule>(serviceProvider);
  }
}