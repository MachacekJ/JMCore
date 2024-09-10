using ACore.Server.Storages.Models;

namespace ACore.Server.Storages;

public interface IStorageResolver
{
  Task ConfigureStorage<TStorage>(StorageImplementation implementation)
    where TStorage : IStorage;
  
  T FirstReadOnlyStorage<T>(StorageTypeEnum storageType = StorageTypeEnum.AllRegistered) where T : IStorage;
  IEnumerable<T> WriteStorages<T>(StorageTypeEnum storageType = StorageTypeEnum.AllRegistered) where T : IStorage;
}