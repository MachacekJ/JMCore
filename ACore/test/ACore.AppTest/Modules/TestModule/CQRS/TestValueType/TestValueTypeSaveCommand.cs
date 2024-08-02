using ACore.AppTest.Modules.TestModule.CQRS.Models;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestValueType;

public class TestValueTypeSaveCommand(TestValueTypeData data): TestModuleRequest<bool>
{
  public TestValueTypeData Data => data;
}