using ACore.Base.CQRS.Models.Results;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Get;

public class TestValueTypeGetQuery: TestModuleQueryRequest<Result<TestValueTypeData[]>>
{
  
}