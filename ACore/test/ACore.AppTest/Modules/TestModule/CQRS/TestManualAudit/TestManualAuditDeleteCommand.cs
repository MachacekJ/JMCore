using ACore.AppTest.Modules.TestModule.CQRS.Models;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestManualAudit;

public class TestManualAuditDeleteCommand(TestManualAuditData data): TestModuleRequest<bool>
{
  public TestManualAuditData Data => data;
}