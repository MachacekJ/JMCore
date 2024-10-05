using ACore.Base.CQRS.Results;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Models;

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Save;

public class TestNoAuditSaveCommand(TestNoAuditData data, string? hash): TestModuleCommandRequest<Result>(hash)
{
  public TestNoAuditData Data => data;
}