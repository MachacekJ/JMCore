using ACore.Base.CQRS.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.Test.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.Test.Save;

public class TestSaveCommand(TestData data): TestModuleRequest<Result>
{
  public TestData Data => data;
}