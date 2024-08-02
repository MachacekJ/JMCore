using ACore.Tests.Implementations.Modules.TestModule.CQRS.Models;

namespace ACore.Tests.Implementations.Modules.TestModule.CQRS.TestPKGuid;

public class TestPKGuidSaveCommand(TestPKGuidData data): TestModuleRequest<bool>
{
  public TestPKGuidData Data => data;
}