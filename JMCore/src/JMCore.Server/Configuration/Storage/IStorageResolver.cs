using JMCore.Server.Configuration.Storage.Models;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Server.Configuration.Storage;

public interface IStorageResolver
{
  void RegisterStorage(IServiceCollection sc, StorageConfigurationItem storageModule);
  Task ConfigureStorages(IServiceProvider sp);
  T StorageModuleImplementation<T>(StorageTypeEnum storageType, StorageModeEnum storageMode = StorageModeEnum.ReadWrite);
  List<T> StorageModuleImplementations<T>(StorageTypeEnum storageType = StorageTypeEnum.AllRegistered, StorageModeEnum storageMode = StorageModeEnum.ReadWrite);
}