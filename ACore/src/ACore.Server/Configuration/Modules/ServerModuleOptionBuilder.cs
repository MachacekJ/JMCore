using ACore.Server.Storages.Configuration;

namespace ACore.Server.Configuration.Modules;

public class ServerModuleOptionBuilder
{
  private StorageOptionBuilder? _storageOptionBuilder;
 // protected bool IsActive { get; private set; }
  protected void StoragesBase(Action<StorageOptionBuilder> action)
  {
    _storageOptionBuilder ??= StorageOptionBuilder.Empty();
    action(_storageOptionBuilder);
  }

  // public void Activate()
  // {
  //   IsActive = true;
  // }

  protected StorageOptions BuildStorage(StorageOptionBuilder? defaultStorages, string moduleName)
  {
    _storageOptionBuilder ??= defaultStorages
                              ?? throw new Exception($"No storage is defined for {moduleName}");

    return _storageOptionBuilder.Build();
  }
}