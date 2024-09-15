using ACore.Server.Storages;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestManualAudit.Delete;

internal class TestManualAuditDeleteHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestManualAuditDeleteCommand, bool>(storageResolver)
{
  public override async Task<Result<bool>> Handle(TestManualAuditDeleteCommand request, CancellationToken cancellationToken)
  {
    //await WriteStorages().Delete<TestManualAuditEntity, long>(request.Data.Id);
    return Result.Success(true);
  }
}