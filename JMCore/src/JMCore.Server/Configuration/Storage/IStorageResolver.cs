using JMCore.Server.Configuration.Storage.Models;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Server.Configuration.Storage;

public interface IStorageResolver
{
  void RegisterServices(IServiceCollection services);
  void RegisterStorage(IServiceCollection sc, StorageConfigurationBase storageModule);
  Task ConfigureStorages(IServiceProvider sp);
  T FirstStorageModuleImplementation<T>(StorageTypeEnum storageType = StorageTypeEnum.AllRegistered, StorageModeEnum storageMode = StorageModeEnum.ReadWrite);
  List<T> AllStorageModuleImplementations<T>(StorageTypeEnum storageType = StorageTypeEnum.AllRegistered, StorageModeEnum storageMode = StorageModeEnum.ReadWrite);
}