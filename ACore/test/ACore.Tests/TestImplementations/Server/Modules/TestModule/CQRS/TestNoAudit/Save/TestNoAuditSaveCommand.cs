using ACore.Base.CQRS.Models.Results;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Save;

public class TestNoAuditSaveCommand(TestNoAuditData data, string? hash): TestModuleCommandRequest<Result>(hash)
{
  public TestNoAuditData Data => data;
}