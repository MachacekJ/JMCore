using ACore.Server.Storages.Configuration;

namespace ACore.Server.Configuration.Modules;

public class ModuleOptions(bool isActive)
{
  public bool IsActive => isActive;
  public StorageOptions? Storages { get; init; }
}