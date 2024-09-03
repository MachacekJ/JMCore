using ACore.AppTest.Modules.TestModule.CQRS.TestPKString.Models;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestPKString.Save;

public class TestPKStringSaveCommand(TestPKStringData data): TestModuleRequest
{
  public TestPKStringData Data => data;
}