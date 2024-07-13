using JMCore.Server.Configuration.Storage;
using JMCore.Server.Storages.Modules.AuditModule;
using JMCore.Server.Storages.Modules.BasicModule;
using JMCore.Tests.ServerT.StoragesT.Implementations.MemoryStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.TestsIntegrations.ServerT.StoragesT;

public class MemoryStorageRegistrationT: IStorageRegistrationT
{
  public void RegisterServices(ServiceCollection sc, IConfigurationRoot configuration, IEnumerable<string> requiredBaseStorageModules, IStorageResolver storageResolver)
  {
    storageResolver.RegisterStorage(sc, new MemoryStorageConfiguration(requiredBaseStorageModules));
  }

  public void GetServices(IServiceProvider sp)
  {

  }

  public void FinishedTest()
  {
   
  }
}