using ACore.Base.CQRS.Results;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Models;

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Save;

public class TestPKStringSaveCommand(TestPKStringData data): TestModuleRequest<Result>
{
  public TestPKStringData Data => data;
}