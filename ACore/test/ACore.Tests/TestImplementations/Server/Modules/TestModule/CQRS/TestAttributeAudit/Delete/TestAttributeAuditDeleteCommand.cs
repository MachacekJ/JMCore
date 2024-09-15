using ACore.Base.CQRS.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAttributeAudit.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAttributeAudit.Delete;

public class TestAttributeAuditDeleteCommand<T>(T id): TestModuleRequest<Result>
{
  public T Id => id;
}