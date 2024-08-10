using ACore.AppTest.Modules.TestModule.Models;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit;

public class TestAttributeAuditDeleteCommand<T>(TestAttributeAuditData<T> data): TestModuleRequest<bool>
{
  public TestAttributeAuditData<T> Data => data;
}