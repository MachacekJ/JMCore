using ACore.Server.Configuration;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Configuration;

namespace ACore.Tests.TestImplementations.Server.Configuration;

public class ACoreTestOptions
{
  public ACoreServerOptions ACoreServerOptions { get; init; } = new();
  public TestModuleOptions TestModuleOptions { get; init; } = new();
}