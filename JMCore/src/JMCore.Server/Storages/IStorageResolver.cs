using JMCore.Server.Storages.Configuration;
using JMCore.Server.Storages.Models;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Server.Storages;

public interface IStorageResolver
{
  void RegisterServices(IServiceCollection services);
  void RegisterStorage(IServiceCollection sc, StorageConfigurationBase storageModule);
  Task ConfigureStorages(IServiceProvider sp);
  T FirstStorageModuleImplementation<T>(StorageTypeEnum storageType = StorageTypeEnum.AllRegistered, StorageModeEnum storageMode = StorageModeEnum.ReadWrite);
  List<T> AllStorageModuleImplementations<T>(StorageTypeEnum storageType = StorageTypeEnum.AllRegistered, StorageModeEnum storageMode = StorageModeEnum.ReadWrite);
}