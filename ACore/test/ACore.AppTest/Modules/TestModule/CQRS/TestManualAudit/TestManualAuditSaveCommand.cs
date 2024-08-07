using ACore.AppTest.Modules.TestModule.Models;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestManualAudit;

public class TestManualAuditSaveCommand(TestManualAuditData data): TestModuleRequest<long>
{
  public TestManualAuditData Data => data;
}