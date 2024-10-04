using ACore.Base.Modules.Configuration;
using ACore.Server.Storages.Configuration;

namespace ACore.Server.Configuration.Modules;

public class StorageModuleOptionBuilder : ModuleOptionsBuilder
{
  private StorageOptionBuilder? _storageOptionBuilder;
  protected void StoragesBase(Action<StorageOptionBuilder> action)
  {
    _storageOptionBuilder ??= StorageOptionBuilder.Empty();
    action(_storageOptionBuilder);
  }

  protected StorageOptions BuildStorage(StorageOptionBuilder? defaultStorages, string moduleName)
  {
    _storageOptionBuilder ??= defaultStorages
                              ?? throw new Exception($"No storage is defined for {moduleName}");

    return _storageOptionBuilder.Build();
  }
}