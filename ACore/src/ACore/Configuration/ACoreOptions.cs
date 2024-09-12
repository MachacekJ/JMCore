using ACore.Modules.MemoryCacheModule.Configuration;

namespace ACore.Configuration;

public class ACoreOptions
{
  public MemoryCacheModuleOptions MemoryCacheModuleOptions { get; init; } = new();
}