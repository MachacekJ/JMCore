using ACore.Server.Storages.Models;

namespace ACore.Server.Storages;

public interface IStorageResolver
{
  Task ConfigureStorage<TStorage>(StorageImplementation implementation)
    where TStorage : IStorage;
  
  // void RegisterServices(IServiceCollection services);
  // void RegisterStorage(IServiceCollection sc, StorageConfigurationBase storageModule);
  // Task ConfigureStorages(IServiceProvider sp);
  T FirstReadOnlyStorage<T>(StorageTypeEnum storageType = StorageTypeEnum.AllRegistered) where T : IStorage;
//  T FirstReadWriteStorage<T>(StorageTypeEnum storageType = StorageTypeEnum.AllRegistered, StorageModeEnum storageMode = StorageModeEnum.ReadWrite) where T : IStorage;
  List<T> AllWriteStorages<T>(StorageTypeEnum storageType = StorageTypeEnum.AllRegistered) where T : IStorage;
}