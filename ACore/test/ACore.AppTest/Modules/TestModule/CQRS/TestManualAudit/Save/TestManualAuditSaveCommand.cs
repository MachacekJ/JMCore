using ACore.AppTest.Modules.TestModule.CQRS.TestManualAudit.Models;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestManualAudit.Save;

public class TestManualAuditSaveCommand(TestManualAuditData data): TestModuleRequest<long>
{
  public TestManualAuditData Data => data;
}