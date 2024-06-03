using JMCore.Server.Configuration.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.TestsIntegrations.ServerT.StoragesT;

public interface IStorageRegistrationT
{
    void RegisterServices(ServiceCollection sc, IConfigurationRoot configuration, IStorageResolver storageResolver);
    void GetServices(IServiceProvider sp);
    void FinishedTest();
}