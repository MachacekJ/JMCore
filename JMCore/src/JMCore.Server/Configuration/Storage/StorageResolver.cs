using JMCore.Server.Configuration.Storage.Models;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Server.Configuration.Storage;

public class StorageResolver : IStorageResolver
{
  private readonly List<StorageConfigurationItem> _allStorageModules = [];

  public void RegisterStorage(IServiceCollection sc, StorageConfigurationItem storageModule)
  {
    _allStorageModules.Add(storageModule);
    storageModule.RegisterServices(sc);
  }

  public async Task ConfigureStorages(IServiceProvider sp)
  {
    foreach (var storage in _allStorageModules)
    {
      await storage.ConfigureServices(sp);
    }
  }

  public T StorageModuleImplementation<T>(StorageTypeEnum storageType, StorageModeEnum storageMode = StorageModeEnum.ReadWrite)
  {
    var storageImplementation = _allStorageModules.Where(sm => storageType.HasFlag(sm.StorageType) && sm.StorageMode.HasFlag(storageMode))
      .Select(storageModule => storageModule.StorageModuleImplementation<T>()).OfType<T>()
      .FirstOrDefault();
    
    if (storageImplementation != null)
      return storageImplementation;

    throw new Exception($"Storage module '{typeof(T).Name}' has no registered implementation.");
  }
  
  public List<T> StorageModuleImplementations<T>(StorageTypeEnum storageType = StorageTypeEnum.AllRegistered, StorageModeEnum storageMode = StorageModeEnum.ReadWrite)
  {
    var ab = _allStorageModules.Where(sm =>  storageType.HasFlag(sm.StorageType) &&  sm.StorageMode.HasFlag(storageMode)).Select(storageModule => storageModule.StorageModuleImplementation<T>()).OfType<T>().ToList();
    if (ab.Count > 0)
      return ab;

    throw new Exception($"Storage module '{typeof(T).Name}' has no registered implementation.");
  }
}