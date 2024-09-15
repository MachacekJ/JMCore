using ACore.Base.CQRS.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Save;

public class TestValueTypeSaveCommand(TestValueTypeData data): TestModuleRequest<Result>
{
  public TestValueTypeData Data => data;
}