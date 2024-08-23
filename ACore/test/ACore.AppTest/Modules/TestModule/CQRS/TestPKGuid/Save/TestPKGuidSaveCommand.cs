using ACore.AppTest.Modules.TestModule.CQRS.TestPKGuid.Models;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestPKGuid.Save;

public class TestPKGuidSaveCommand(TestPKGuidData data): TestModuleRequest<Guid>
{
  public TestPKGuidData Data => data;
}