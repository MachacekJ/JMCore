using JMCore.Server.PGStorage;
using JMCore.Tests.Implementations.Storages.TestModule.Storages;
using JMCore.Tests.Implementations.Storages.TestModule.Storages.PG.TestStorageModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.TestsIntegrations.ServerT.StoragesT.TestStorageConfiguration;

public class PGTestStorageConfiguration(string connectionString, IEnumerable<string> requiredStorageModules) : PGStorageConfiguration(connectionString, requiredStorageModules)
{
  private readonly string _connectionString = connectionString;

  public override void RegisterServices(IServiceCollection services)
  {
    base.RegisterServices(services);
    foreach (var requiredStorageModule in RequiredStorageModules)
    {
      switch (requiredStorageModule)
      {
        case nameof(ITestStorageModule):
          services.AddDbContext<TestPGEfStorageImpl>(opt => opt.UseNpgsql(_connectionString));
          services.AddSingleton<ITestStorageModule, TestPGEfStorageImpl>();
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
          await ConfigureEfSqlServiceLocal<ITestStorageModule, TestPGEfStorageImpl>(serviceProvider);
          break;
      }
    }
  }
}