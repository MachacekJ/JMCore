using ACore.Modules.MemoryCacheModule.Configuration;

namespace ACore.Configuration;

public class ACoreOptions
{
  public string SaltForHash { get; init; } = string.Empty;
  public MemoryCacheModuleOptions MemoryCacheModuleOptions { get; init; } = new(false);
}