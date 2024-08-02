using ACore.Server.Storages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.TestsIntegrations.BaseInfrastructure.Storages;

public interface IStorageRegistrationT
{
    void RegisterServices(ServiceCollection sc, IConfigurationRoot configuration, IEnumerable<string> requiredBaseStorageModules, IStorageResolver storageResolver);
    void GetServices(IServiceProvider sp);
    void FinishedTest();
}