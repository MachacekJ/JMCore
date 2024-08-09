using ACore.Server.Storages.Definitions.Models;

namespace ACore.Server.Storages.Services.StorageResolvers;

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


    if (implementation.Implementation is not TStorage)
      throw new Exception($"Cannot find any implementation of {name}.");
    
    if (_implementations.TryGetValue(name, out var list))
    {
      // Only one database mode type (write/read) is allowed for particular storage type. e.g.  2 database for storage in reading mode is not allowed.  
      if (implementation.Mode.HasFlag(StorageModeEnum.Read) &&
          list.Any(e => e.Mode.HasFlag(StorageModeEnum.Read)))
        throw new Exception($"For the type {Enum.GetName(StorageModeEnum.Read)} only one storage '{name}' is allowed.");
      
      if (implementation.Mode.HasFlag(StorageModeEnum.Write) &&
          list.Any(e => e.Mode.HasFlag(StorageModeEnum.Write)))
        throw new Exception($"For the type {Enum.GetName(StorageModeEnum.Write)} only one storage '{name}' is allowed.");
      
      list.Add(implementation);
    }
    else
      _implementations.Add(name, [implementation]);

    try
    {
      await implementation.Implementation.UpSchema();
    }
    catch (Exception e)
    {
      throw new Exception($"Cannot configure '{name}' storage", e);
    }
  }

  public T FirstReadOnlyStorage<T>(StorageTypeEnum storageType = StorageTypeEnum.All) where T : IStorage
    => AllStorages<T>(StorageModeEnum.Read, storageType).First();

  public IEnumerable<T> WriteStorages<T>(StorageTypeEnum storageType = StorageTypeEnum.All) where T : IStorage
  {
    return AllStorages<T>(StorageModeEnum.Write, storageType);
  }

  private List<T> AllStorages<T>(StorageModeEnum mode, StorageTypeEnum storageType = StorageTypeEnum.All) where T : IStorage
  {
    if (!_implementations.TryGetValue(typeof(T).Name, out var storageImplementations))
      throw new Exception($"Storage module '{typeof(T).Name}' has no registered implementation.");

    var storageImplementationByMode = storageImplementations.Where(e => e.Mode.HasFlag(mode)).Select(storageModule => storageModule.Implementation).OfType<T>().ToList();

    if (storageType != StorageTypeEnum.All)
      storageImplementationByMode = storageImplementationByMode.Where(a => a.StorageDefinition.Type == storageType).ToList();

    if (storageImplementationByMode.Count > 0)
      return storageImplementationByMode;

    throw new Exception($"Storage module '{typeof(T).Name}' has no registered implementation.");
  }
}