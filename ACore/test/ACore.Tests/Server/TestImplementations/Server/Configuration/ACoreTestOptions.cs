using ACore.Server.Configuration;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Configuration;

namespace ACore.Tests.Server.TestImplementations.Server.Configuration;

public class ACoreTestOptions
{
  public ACoreServerOptions ACoreServerOptions { get; init; } = new();
  public TestModuleOptions TestModuleOptions { get; init; } = new();
}