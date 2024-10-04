using ACore.Base.CQRS.Results;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKLong.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKLong.Save;

public class TestPKLongSaveCommand(TestPKLongData data): TestModuleRequest<Result>
{
  public TestPKLongData Data => data;
}