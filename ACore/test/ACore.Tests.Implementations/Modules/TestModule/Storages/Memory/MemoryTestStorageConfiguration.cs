using ACore.Server.MemoryStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Implementations.Modules.TestModule.Storages.Memory;

public class MemoryTestStorageConfiguration(IEnumerable<string> requiredStorageModules) : MemoryStorageConfiguration(requiredStorageModules)
{
  public override void RegisterServices(IServiceCollection services)
  {
    base.RegisterServices(services);
    foreach (var requiredStorageModule in RequiredStorageModules)
    {
      switch (requiredStorageModule)
      {
        case nameof(ITestStorageModule):
          services.AddDbContext<TestMemoryEfStorageImpl>(opt => opt.UseInMemoryDatabase(ConnectionString + nameof(ITestStorageModule) + Guid.NewGuid()));
          services.AddSingleton<ITestStorageModule, TestMemoryEfStorageImpl>();
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
          await ConfigureEfSqlServiceLocal<ITestStorageModule, TestMemoryEfStorageImpl>(serviceProvider);
          break;
      }
    }
  }
  
}