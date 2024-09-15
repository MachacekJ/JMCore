using ACore.Base.CQRS.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.Test.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.Test.Get;

public class TestGetQuery: TestModuleRequest<Result<TestData[]>>
{
  
}