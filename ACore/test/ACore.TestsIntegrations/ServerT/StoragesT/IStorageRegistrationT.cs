using ACore.Server.Storages;
using ACore.Server.Storages.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.TestsIntegrations.ServerT.StoragesT;

public interface IStorageRegistrationT
{
    void RegisterServices(ServiceCollection sc, IConfigurationRoot configuration, IEnumerable<string> requiredBaseStorageModules, IStorageResolver storageResolver);
    void GetServices(IServiceProvider sp);
    void FinishedTest();
}