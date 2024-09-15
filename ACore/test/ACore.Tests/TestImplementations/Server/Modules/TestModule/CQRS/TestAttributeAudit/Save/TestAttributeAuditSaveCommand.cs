using ACore.Base.CQRS.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAttributeAudit.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAttributeAudit.Save;

public class TestAttributeAuditSaveCommand<T>(TestAttributeAuditData<T> data): TestModuleRequest<Result>
{
  public TestAttributeAuditData<T> Data => data;
}

