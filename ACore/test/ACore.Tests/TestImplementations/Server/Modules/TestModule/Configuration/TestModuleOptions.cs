using ACore.Base.Modules;
using ACore.Server.Configuration.Modules;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.Configuration;

public class TestModuleOptions : StorageModuleOptions, IModuleOptions{
  public bool IsActive { get; init; }
  public string ModuleName => nameof(TestModule);
}