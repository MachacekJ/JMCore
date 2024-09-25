using ACore.Base.CQRS.Models.Results;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Get;

public class TestAuditGetQuery<T>: TestModuleRequest<Result<TestAuditData<T>[]>>;