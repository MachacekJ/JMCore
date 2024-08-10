using ACore.AppTest.Modules.TestModule.Models;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit;

public class TestAttributeAuditSaveCommand<T>(TestAttributeAuditData<T> data): TestModuleRequest<T>
{
  public TestAttributeAuditData<T> Data => data;
}