using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestManualAudit.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestManualAudit.Save;

public class TestManualAuditSaveCommand(TestManualAuditData data): TestModuleRequest<TestManualAuditData>
{
  public TestManualAuditData Data => data;
}