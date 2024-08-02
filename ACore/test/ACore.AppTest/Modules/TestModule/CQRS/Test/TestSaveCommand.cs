using ACore.AppTest.Modules.TestModule.CQRS.Models;

namespace ACore.AppTest.Modules.TestModule.CQRS.Test;

public class TestSaveCommand(TestData data): TestModuleRequest<bool>
{
  public TestData Data => data;
}