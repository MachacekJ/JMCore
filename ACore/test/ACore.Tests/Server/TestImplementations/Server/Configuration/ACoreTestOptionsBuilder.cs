using ACore.Server.Configuration;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Configuration;

namespace ACore.Tests.Server.TestImplementations.Server.Configuration;

public class ACoreTestOptionsBuilder
{
  private readonly ACoreServerOptionBuilder _aCoreServerOptionsBuilder = ACoreServerOptionBuilder.Empty();
  private readonly TestModuleOptionsBuilder _testModuleOptionBuilder = TestModuleOptionsBuilder.Empty();

  public static ACoreTestOptionsBuilder Empty() => new();
  
  private ACoreTestOptionsBuilder()
  {
  }
  
  public ACoreTestOptionsBuilder ACoreServer(Action<ACoreServerOptionBuilder>? action = null)
  {
    action?.Invoke(_aCoreServerOptionsBuilder);
    return this;
  }

  public ACoreTestOptionsBuilder AddTestModule(Action<TestModuleOptionsBuilder>? action = null)
  {
    action?.Invoke(_testModuleOptionBuilder);
    _testModuleOptionBuilder.Activate();
    return this;
  }
  
  public ACoreTestOptions Build()
  {
    return new ACoreTestOptions
    {
      ACoreServerOptions = _aCoreServerOptionsBuilder.Build(),
      TestModuleOptions = _testModuleOptionBuilder.Build(_aCoreServerOptionsBuilder.DefaultStorageOptionBuilder)
    };
  }
}