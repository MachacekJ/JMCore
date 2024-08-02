using ACore.AppTest.Modules.TestModule.CQRS.Models;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestPKGuid;

public class TestPKGuidSaveCommand(TestPKGuidData data): TestModuleRequest<bool>
{
  public TestPKGuidData Data => data;
}