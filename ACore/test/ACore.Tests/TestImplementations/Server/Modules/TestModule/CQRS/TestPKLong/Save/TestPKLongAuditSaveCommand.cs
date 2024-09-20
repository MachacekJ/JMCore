using ACore.Base.CQRS.Models;
using ACore.Base.CQRS.Models.Results;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKLong.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKLong.Save;

public class TestPKLongAuditSaveCommand(TestPKLongAuditData data): TestModuleRequest<Result>
{
  public TestPKLongAuditData Data => data;
}