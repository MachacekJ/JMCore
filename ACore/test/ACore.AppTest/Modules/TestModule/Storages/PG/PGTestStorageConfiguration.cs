using ACore.Server.PGStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.AppTest.Modules.TestModule.Storages.PG;

public class PGTestStorageConfiguration(string connectionString, IEnumerable<string> requiredStorageModules) : PGStorageConfiguration(connectionString, requiredStorageModules)
{
  private readonly string _connectionString = connectionString;

  public override void RegisterServices(IServiceCollection services)
  {
    base.RegisterServices(services);
    foreach (var requiredStorageModule in RequiredStorageModules)
    {
      if (requiredStorageModule == AppTestModulesNames.TestModule)
      {
        services.AddTestServiceModule();
        services.AddDbContext<TestPGEfStorageImpl>(opt => opt.UseNpgsql(_connectionString));
        services.AddSingleton<ITestStorageModule, TestPGEfStorageImpl>();
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
        await ConfigureEfSqlServiceLocal<ITestStorageModule, TestPGEfStorageImpl>(serviceProvider);
      }
    }
  }
}