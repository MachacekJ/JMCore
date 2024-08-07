using ACore.AppTest.Modules.TestModule.Models;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestValueType;

public class TestValueTypeSaveCommand(TestValueTypeData data): TestModuleRequest<int>
{
  public TestValueTypeData Data => data;
}