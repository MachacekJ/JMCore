using ACore.Base.CQRS.Models.Results;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Save;

public class TestValueTypeSaveHashCommand(TestValueTypeData data): TestModuleHashEntityCommandRequest<Result>(data.HashToCheck)
{
  public TestValueTypeData Data => data;
}