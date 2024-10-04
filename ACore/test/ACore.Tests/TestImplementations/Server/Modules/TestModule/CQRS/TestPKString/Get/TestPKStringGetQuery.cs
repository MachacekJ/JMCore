using ACore.Base.CQRS.Results;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Get;

public class TestPKStringGetQuery: TestModuleRequest<Result<TestPKStringData[]>>
{
  
}