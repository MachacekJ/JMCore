using ACore.Base.CQRS.Results;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Models;

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Get;

public class TestValueTypeGetQuery: TestModuleQueryRequest<Result<TestValueTypeData[]>>
{
  
}