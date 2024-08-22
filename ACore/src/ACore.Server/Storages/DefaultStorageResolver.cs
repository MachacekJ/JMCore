using ACore.Server.Storages.Models;

namespace ACore.Server.Storages;

public class StorageImplementation(IStorage implementation, StorageModeEnum mode = StorageModeEnum.ReadWrite)
{
  public StorageModeEnum Mode => mode;
  public IStorage Implementation => implementation;
}

public class DefaultStorageResolver : IStorageResolver
{
  private readonly Dictionary<string, List<StorageImplementation>> _implementations = [];

  public async Task ConfigureStorage<TStorage>(StorageImplementation implementation)
    where TStorage : IStorage
  {
    var name = typeof(TStorage).Name;
    if (implementation == null)
      throw new Exception($"Cannot find any implementation of {name}.");

    if (_implementations.TryGetValue(name, out var list))
      list.Add(implementation);
    else
      _implementations.Add(name, [implementation]);

    try
    {
      await implementation.Implementation.UpdateDatabase();
    }
    catch (Exception e)
    {
      throw new Exception($"Cannot configure '{name}' storage", e);
    }
  }

  // public void RegisterServices(IServiceCollection services)
  // {
  //   
  // }

  // public void RegisterStorage(IServiceCollection sc, StorageConfigurationBase storageModule)
  // {
  //   if (storageModule.StorageMode.HasFlag(StorageModeEnum.Write))
  //   {
  //     var exists = _allStorageModules.SingleOrDefault(e => e.StorageType == storageModule.StorageType);
  //     if (exists != null)
  //       throw new Exception($"Storage type {Enum.GetName(typeof(StorageTypeEnum), storageModule.StorageType)} is already implemented for write context.");
  //   }
  //
  //   _allStorageModules.Add(storageModule);
  //   storageModule.RegisterServices(sc);
  // }
  //
  // public async Task ConfigureStorages(IServiceProvider sp)
  // {
  //   foreach (var storage in _allStorageModules)
  //   {
  //     await storage.ConfigureServices(sp);
  //   }
  // }

  public T FirstReadOnlyStorage<T>(StorageTypeEnum storageType = StorageTypeEnum.AllRegistered) where T : IStorage
    => AllStorages<T>(StorageModeEnum.Write, storageType).First();

  // public T FirstReadWriteStorage<T>(StorageTypeEnum storageType = StorageTypeEnum.AllRegistered, StorageModeEnum storageMode = StorageModeEnum.ReadWrite) where T : IStorage
  // {
  //   var storageImplementation = _implementations.Where(sm => storageType.HasFlag(sm.StorageType) && sm.StorageMode.HasFlag(storageMode))
  //     .Select(storageModule => storageModule.StorageModuleImplementation<T>()).OfType<T>()
  //     .FirstOrDefault();
  //
  //   if (storageImplementation != null)
  //     return storageImplementation;
  //
  //   throw new Exception($"Storage module '{typeof(T).Name}' has no registered implementation.");
  // }

  public List<T> AllWriteStorages<T>(StorageTypeEnum storageType = StorageTypeEnum.AllRegistered) where T : IStorage
  {
    return AllStorages<T>(StorageModeEnum.Write, storageType);
  }

  private List<T> AllStorages<T>(StorageModeEnum mode, StorageTypeEnum storageType = StorageTypeEnum.AllRegistered) where T : IStorage
  {
    if (_implementations.TryGetValue(typeof(T).Name, out var aa))
    {
      var ab = aa.Where(e => e.Mode.HasFlag(mode)).Select(storageModule => storageModule.Implementation).OfType<T>().ToList();

      if (storageType != StorageTypeEnum.AllRegistered)
        ab = ab.Where(a => a.StorageDefinition.Type == storageType).ToList();
      
      if (ab.Count > 0)
        return ab;
    }

    //var ab = _allStorageModules.Where(sm => storageType.HasFlag(sm.StorageType) && sm.StorageMode.HasFlag(storageMode)).Select(storageModule => storageModule.StorageModuleImplementation<T>()).OfType<T>().ToList();


    throw new Exception($"Storage module '{typeof(T).Name}' has no registered implementation.");
  }
}