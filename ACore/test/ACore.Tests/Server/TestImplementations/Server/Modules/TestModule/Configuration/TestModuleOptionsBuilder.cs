using ACore.Server.Configuration.Modules;
using ACore.Server.Modules.SettingsDbModule.Storage;
using ACore.Server.Storages.Configuration;

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Configuration;

public class TestModuleOptionsBuilder: StorageModuleOptionBuilder
{
  public static TestModuleOptionsBuilder Empty() => new();


  public TestModuleOptions Build(StorageOptionBuilder? defaultStorages)
  {
    return new TestModuleOptions(IsActive)
    {
      Storages = BuildStorage(defaultStorages, nameof(ISettingsDbModuleStorage))
    };
  }
}