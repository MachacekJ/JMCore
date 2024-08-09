using ACore.Base.CQRS.Results;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Models;

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Save;

public class TestAuditSaveCommand<T>(TestAuditData<T> data): TestModuleRequest<Result>
{
  public TestAuditData<T> Data => data;
}

