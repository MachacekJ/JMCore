using ACore.AppTest.Modules.TestModule.CQRS.Models;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestPKString;

public class TestPKStringSaveCommand(TestPKStringData data): TestModuleRequest<bool>
{
  public TestPKStringData Data => data;
}