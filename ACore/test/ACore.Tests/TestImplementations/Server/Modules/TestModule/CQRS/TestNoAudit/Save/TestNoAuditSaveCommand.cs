using ACore.Base.CQRS.Models;
using ACore.Base.CQRS.Models.Results;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Save;

public class TestNoAuditSaveCommand(TestNoAuditData data): TestModuleRequest<Result>
{
  public TestNoAuditData Data => data;
}