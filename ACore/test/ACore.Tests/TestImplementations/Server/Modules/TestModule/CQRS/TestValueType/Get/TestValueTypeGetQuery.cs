using ACore.Base.CQRS.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Get;

public class TestValueTypeGetQuery: TestModuleRequest<Result<TestValueTypeData[]>>
{
  
}