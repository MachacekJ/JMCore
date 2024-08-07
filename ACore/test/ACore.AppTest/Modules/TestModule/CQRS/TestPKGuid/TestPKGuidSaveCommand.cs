using ACore.AppTest.Modules.TestModule.Models;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestPKGuid;

public class TestPKGuidSaveCommand(TestPKGuidData data): TestModuleRequest<Guid>
{
  public TestPKGuidData Data => data;
}