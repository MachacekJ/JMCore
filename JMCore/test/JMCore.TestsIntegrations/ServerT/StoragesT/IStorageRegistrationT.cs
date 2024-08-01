using JMCore.Server.Storages;
using JMCore.Server.Storages.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.TestsIntegrations.ServerT.StoragesT;

public interface IStorageRegistrationT
{
    void RegisterServices(ServiceCollection sc, IConfigurationRoot configuration, IEnumerable<string> requiredBaseStorageModules, IStorageResolver storageResolver);
    void GetServices(IServiceProvider sp);
    void FinishedTest();
}