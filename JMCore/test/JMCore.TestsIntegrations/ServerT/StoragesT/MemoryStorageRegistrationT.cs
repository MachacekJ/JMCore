using JMCore.Server.Configuration.Storage;
using JMCore.Server.Storages.Modules.AuditModule;
using JMCore.Server.Storages.Modules.BasicModule;
using JMCore.Tests.ServerT.StoragesT.Impl.MemoryStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.TestsIntegrations.ServerT.StoragesT;

public class MemoryStorageRegistrationT: IStorageRegistrationT
{
  public void RegisterServices(ServiceCollection sc, IConfigurationRoot configuration, IStorageResolver storageResolver)
  {
    storageResolver.RegisterStorage(sc, new MemoryStorageConfiguration(new[]
    {
      nameof(IBasicStorageModule),
      nameof(IAuditStorageModule)
    }));
  }

  public void GetServices(IServiceProvider sp)
  {

  }

  public void FinishedTest()
  {
   
  }
}