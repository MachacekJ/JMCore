using ACore.AppTest.Modules.TestModule.CQRS.Test.Models;

namespace ACore.AppTest.Modules.TestModule.CQRS.Test.Save;

public class TestSaveCommand(TestData data): TestModuleRequest<TestData>
{
  public TestData Data => data;
}