using ACore.Base.CQRS.Results;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Models;

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Save;

public class TestValueTypeSaveCommand(TestValueTypeData data): TestModuleCommandRequest<Result>(null)
{
  public TestValueTypeData Data => data;
}