using ACore.Base.CQRS.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAttributeAudit.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAttributeAudit.Get;

public class TestAttributeAuditGetQuery<T>: TestModuleRequest<Result<TestAttributeAuditData<T>[]>>;