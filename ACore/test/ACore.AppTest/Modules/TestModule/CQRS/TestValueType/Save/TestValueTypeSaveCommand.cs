using ACore.AppTest.Modules.TestModule.CQRS.TestValueType.Models;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestValueType.Save;

public class TestValueTypeSaveCommand(TestValueTypeData data): TestModuleRequest
{
  public TestValueTypeData Data => data;
}