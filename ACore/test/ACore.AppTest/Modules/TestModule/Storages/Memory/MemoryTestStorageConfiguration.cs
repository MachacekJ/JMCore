using ACore.AppTest.Modules.TestModule.Storages.EF;
using ACore.Server.MemoryStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.AppTest.Modules.TestModule.Storages.Memory;

public class MemoryTestStorageConfiguration(IEnumerable<string> requiredStorageModules) : MemoryStorageConfiguration(requiredStorageModules)
{
  public override void RegisterServices(IServiceCollection services)
  {
    base.RegisterServices(services);
    foreach (var requiredStorageModule in RequiredStorageModules)
    {
      switch (requiredStorageModule)
      {
        case nameof(IEFTestStorageModule):
          services.AddDbContext<MemoryTestStorageImpl>(opt => opt.UseInMemoryDatabase(ConnectionString + nameof(IEFTestStorageModule) + Guid.NewGuid()));
          services.AddSingleton<IEFTestStorageModule, MemoryTestStorageImpl>();
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
        case nameof(IEFTestStorageModule):
          await ConfigureEfSqlServiceLocal<IEFTestStorageModule, MemoryTestStorageImpl>(serviceProvider);
          break;
      }
    }
  }
  
}