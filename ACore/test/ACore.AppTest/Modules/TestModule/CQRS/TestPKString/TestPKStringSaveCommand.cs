using ACore.AppTest.Modules.TestModule.Models;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestPKString;

public class TestPKStringSaveCommand(TestPKStringData data): TestModuleRequest<string>
{
  public TestPKStringData Data => data;
}