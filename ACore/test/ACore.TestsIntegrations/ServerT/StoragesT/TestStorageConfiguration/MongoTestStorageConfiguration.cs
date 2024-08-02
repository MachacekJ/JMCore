using ACore.Server.MongoStorage;
using ACore.Tests.Implementations.Modules.TestModule.Storages;
using ACore.Tests.Implementations.Modules.TestModule.Storages.Mongo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.TestsIntegrations.ServerT.StoragesT.TestStorageConfiguration;

public class MongoTestStorageConfiguration(string connectionString, string dbName, IEnumerable<string> requiredStorageModules) : MongoStorageConfiguration(connectionString, dbName, requiredStorageModules)
{
  private readonly string _connectionString = connectionString;
  private readonly string _dbName = dbName;

  public override void RegisterServices(IServiceCollection services)
  {
    base.RegisterServices(services);
    foreach (var requiredStorageModule in RequiredStorageModules)
    {
      switch (requiredStorageModule)
      {
        case nameof(ITestStorageModule):
          services.AddDbContext<TestMongoStorageImpl>(opt => opt.UseMongoDB(_connectionString, _dbName));
          services.AddSingleton<ITestStorageModule, TestMongoStorageImpl>();
          break;
      }
    }
  }

  public override async Task ConfigureServices(IServiceProvider serviceProvider)
  {
    await base.ConfigureServices(serviceProvider);
    foreach (var requiredStorageModule in RequiredStorageModules)
    {
      switch (requiredStorageModule)
      {
        case nameof(ITestStorageModule):
          await ConfigureEfSqlServiceLocal<ITestStorageModule, TestMongoStorageImpl>(serviceProvider);
          break;
      }
    }
  }
}