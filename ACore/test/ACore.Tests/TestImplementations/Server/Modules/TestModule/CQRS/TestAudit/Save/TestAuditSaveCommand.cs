using ACore.Base.CQRS.Models.Results;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Save;

public class TestAuditSaveCommand<T>(TestAuditData<T> data): TestModuleRequest<Result>
{
  public TestAuditData<T> Data => data;
}

