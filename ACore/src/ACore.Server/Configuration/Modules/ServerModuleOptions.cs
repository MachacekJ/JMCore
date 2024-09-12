using ACore.Server.Storages.Configuration;

namespace ACore.Server.Configuration.Modules;

public class ServerModuleOptions
{
  public StorageOptions? Storages { get; init; }
}