using ACore.Server.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.TestsIntegrations.BaseInfrastructure.Storages;

public interface IStorageRegistrationT
{
    void RegisterServices(ServiceCollection sc, StorageModuleConfiguration config);
    void GetServices(IServiceProvider sp);
    void FinishedTest();
}