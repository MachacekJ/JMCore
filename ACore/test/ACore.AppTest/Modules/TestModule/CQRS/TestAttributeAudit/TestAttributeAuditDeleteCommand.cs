using ACore.AppTest.Modules.TestModule.Models;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit;

public class TestAttributeAuditDeleteCommand(TestAttributeAuditData data): TestModuleRequest<bool>
{
  public TestAttributeAuditData Data => data;
}