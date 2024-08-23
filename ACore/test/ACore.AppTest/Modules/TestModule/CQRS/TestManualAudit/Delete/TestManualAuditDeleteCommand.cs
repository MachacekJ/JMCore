using ACore.AppTest.Modules.TestModule.CQRS.TestManualAudit.Models;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestManualAudit.Delete;

public class TestManualAuditDeleteCommand(TestManualAuditData data): TestModuleRequest<bool>
{
  public TestManualAuditData Data => data;
}