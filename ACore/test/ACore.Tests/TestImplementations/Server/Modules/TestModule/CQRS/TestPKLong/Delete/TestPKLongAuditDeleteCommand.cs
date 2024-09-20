using ACore.Base.CQRS.Models;
using ACore.Base.CQRS.Models.Results;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKLong.Delete;

public class TestPKLongAuditDeleteCommand(long id): TestModuleRequest<Result>
{
  public long Id => id;
}