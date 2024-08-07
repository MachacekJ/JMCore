using ACore.AppTest.Modules.TestModule.Models;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit;

public class TestAttributeAuditSaveCommand(TestAttributeAuditData data): TestModuleRequest<int>
{
  public TestAttributeAuditData Data => data;
}