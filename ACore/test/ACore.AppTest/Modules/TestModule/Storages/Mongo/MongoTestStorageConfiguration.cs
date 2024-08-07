using ACore.AppTest.Modules.TestModule.Storages.EF;
using ACore.Server.MongoStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.AppTest.Modules.TestModule.Storages.Mongo;

public class MongoTestStorageConfiguration(string connectionString, string dbName, IEnumerable<string> requiredStorageModules) : MongoStorageConfiguration(connectionString, dbName, requiredStorageModules)
{
  private readonly string _connectionString = connectionString;
  private readonly string _dbName = dbName;

  public override void RegisterServices(IServiceCollection services)
  {
    base.RegisterServices(services);
    foreach (var requiredStorageModule in RequiredStorageModules)
    {
      if (requiredStorageModule == AppTestModulesNames.TestModule)
      {
        services.AddTestServiceModule();
        services.AddDbContext<EfTestMongoStorageImpl>(opt => opt.UseMongoDB(_connectionString, _dbName));
        services.AddSingleton<IEFTestStorageModule, EfTestMongoStorageImpl>();
      }
    }
  }

  public override async Task ConfigureServices(IServiceProvider serviceProvider)
  {
    await base.ConfigureServices(serviceProvider);
    foreach (var requiredStorageModule in RequiredStorageModules)
    {
      if (requiredStorageModule == AppTestModulesNames.TestModule)
      {
        serviceProvider.UseTestServiceModule();
        await ConfigureEfSqlServiceLocal<IEFTestStorageModule, EfTestMongoStorageImpl>(serviceProvider);
      }
    }
  }
}