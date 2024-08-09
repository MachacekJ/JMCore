using ACore.Base.CQRS.Results;

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestPKLong.Delete;

public class TestPKLongAuditDeleteCommand(long id): TestModuleRequest<Result>
{
  public long Id => id;
}