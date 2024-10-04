using ACore.Base.CQRS.Results;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Get;

public class TestPKGuidGetQuery: TestModuleRequest<Result<TestPKGuidData[]>>
{
  
}