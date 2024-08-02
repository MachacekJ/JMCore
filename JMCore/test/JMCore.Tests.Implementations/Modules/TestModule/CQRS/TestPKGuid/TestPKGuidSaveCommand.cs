using JMCore.Tests.Implementations.Modules.TestModule.CQRS.Models;

namespace JMCore.Tests.Implementations.Modules.TestModule.CQRS.TestPKGuid;

public class TestPKGuidSaveCommand(TestPKGuidData data): TestModuleRequest<bool>
{
  public TestPKGuidData Data => data;
}