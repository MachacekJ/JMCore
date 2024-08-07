using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Storages.Configuration;
using ACore.Server.Storages.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Server.Storages;

public class StorageResolver : IStorageResolver
{
  private readonly List<StorageConfigurationBase> _allStorageModules = [];

  public void RegisterServices(IServiceCollection services)
  {
    services.AddMediatR((c) => { c.RegisterServicesFromAssemblyContaining(typeof(IBasicStorageModule)); });
  }

  public void RegisterStorage(IServiceCollection sc, StorageConfigurationBase storageModule)
  {
    if (storageModule.StorageMode.HasFlag(StorageModeEnum.Write))
    {
      var exists = _allStorageModules.SingleOrDefault(e => e.StorageType == storageModule.StorageType);
      if (exists != null)
        throw new Exception($"Storage type {Enum.GetName(typeof(StorageTypeEnum), storageModule.StorageType)} is already implemented for write context.");
    }
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

  public T FirstReadOnlyStorage<T>(StorageTypeEnum storageType = StorageTypeEnum.AllRegistered, StorageModeEnum storageMode = StorageModeEnum.Read) where T : IStorage => FirstReadWriteStorage<T>(storageType, storageMode);

  public T FirstReadWriteStorage<T>(StorageTypeEnum storageType = StorageTypeEnum.AllRegistered, StorageModeEnum storageMode = StorageModeEnum.ReadWrite) where T : IStorage
  {
    var storageImplementation = _allStorageModules.Where(sm => storageType.HasFlag(sm.StorageType) && sm.StorageMode.HasFlag(storageMode))
      .Select(storageModule => storageModule.StorageModuleImplementation<T>()).OfType<T>()
      .FirstOrDefault();

    if (storageImplementation != null)
      return storageImplementation;

    throw new Exception($"Storage module '{typeof(T).Name}' has no registered implementation.");
  }

  public List<T> AllWriteStorages<T>(StorageTypeEnum storageType = StorageTypeEnum.AllRegistered, StorageModeEnum storageMode = StorageModeEnum.ReadWrite) where T : IStorage
  {
    var ab = _allStorageModules.Where(sm => storageType.HasFlag(sm.StorageType) && sm.StorageMode.HasFlag(storageMode)).Select(storageModule => storageModule.StorageModuleImplementation<T>()).OfType<T>().ToList();
    if (ab.Count > 0)
      return ab;

    throw new Exception($"Storage module '{typeof(T).Name}' has no registered implementation.");
  }
}