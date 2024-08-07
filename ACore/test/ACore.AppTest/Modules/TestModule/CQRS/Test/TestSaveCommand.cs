using ACore.AppTest.Modules.TestModule.Models;

namespace ACore.AppTest.Modules.TestModule.CQRS.Test;

public class TestSaveCommand(TestData data): TestModuleRequest<int>
{
  public TestData Data => data;
}