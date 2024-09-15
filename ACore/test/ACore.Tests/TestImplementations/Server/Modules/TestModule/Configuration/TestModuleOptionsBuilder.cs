using ACore.Base.Modules;
using ACore.Server.Configuration.Modules;
using ACore.Server.Modules.SettingsDbModule.Storage;
using ACore.Server.Storages.Configuration;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.Configuration;

public class TestModuleOptionsBuilder: StorageModuleOptionBuilder, IModuleOptionsBuilder
{
  private bool _isActive;
  public static TestModuleOptionsBuilder Empty() => new();


  public TestModuleOptions Build(StorageOptionBuilder? defaultStorages)
  {
    return new TestModuleOptions
    {
      Storages = BuildStorage(defaultStorages, nameof(ISettingsDbModuleStorage)),
      IsActive = _isActive
    };
  }

  public void Activate()
  {
    _isActive = true;
  }
}