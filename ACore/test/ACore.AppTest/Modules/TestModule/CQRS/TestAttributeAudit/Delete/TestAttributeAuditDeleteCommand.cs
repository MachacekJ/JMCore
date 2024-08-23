using ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit.Models;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit.Delete;

public class TestAttributeAuditDeleteCommand<T>(TestAttributeAuditData<T> data): TestModuleRequest<bool>
{
  public TestAttributeAuditData<T> Data => data;
}