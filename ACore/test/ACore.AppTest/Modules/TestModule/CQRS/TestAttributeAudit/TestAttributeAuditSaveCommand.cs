using ACore.AppTest.Modules.TestModule.CQRS.Models;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit;

public class TestAttributeAuditSaveCommand(TestAttributeAuditData data): TestModuleRequest<bool>
{
  public TestAttributeAuditData Data => data;
}