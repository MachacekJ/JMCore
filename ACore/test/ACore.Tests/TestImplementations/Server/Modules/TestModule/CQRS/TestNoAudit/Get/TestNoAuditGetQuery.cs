using ACore.Base.CQRS.Models.Results;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Get;

public class TestNoAuditGetQuery : TestModuleQueryRequest<Result<Dictionary<string, TestNoAuditData>>>;