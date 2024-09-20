using ACore.Base.CQRS.Configuration;
using ACore.Server.Storages.Configuration;

namespace ACore.Server.Configuration.Modules;

public class StorageModuleOptions(string moduleName, bool isActive) : ModuleOptions(moduleName, isActive)
{
  public StorageOptions? Storages { get; init; }
}