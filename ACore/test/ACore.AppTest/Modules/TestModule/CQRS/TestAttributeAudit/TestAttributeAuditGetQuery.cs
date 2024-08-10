using ACore.AppTest.Modules.TestModule.Models;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit;

public class TestAttributeAuditGetQuery<T>: TestModuleRequest<TestAttributeAuditData<T>[]>
{
  
}