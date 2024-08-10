using ACore.AppTest.Modules.TestModule.Storages.EF;
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
      if (requiredStorageModule == nameof(IEFTestStorageModule))
      {
        services.AddTestServiceModule();
        services.AddDbContext<PGEFTestStorageImpl>(opt => opt.UseNpgsql(_connectionString));
        services.AddSingleton<IEFTestStorageModule, PGEFTestStorageImpl>();
      }
    }
  }

  public override async Task ConfigureServices(IServiceProvider serviceProvider)
  {
    await base.ConfigureServices(serviceProvider);
    foreach (var requiredStorageModule in RequiredStorageModules)
    {
      if (requiredStorageModule == nameof(IEFTestStorageModule))
      {
        serviceProvider.UseTestServiceModule();
        await ConfigureEfSqlServiceLocal<IEFTestStorageModule, PGEFTestStorageImpl>(serviceProvider);
      }
    }
  }
}