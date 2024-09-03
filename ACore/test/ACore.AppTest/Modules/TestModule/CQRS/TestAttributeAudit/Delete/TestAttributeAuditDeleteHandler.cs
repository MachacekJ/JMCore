using ACore.AppTest.Modules.TestModule.Storages.SQL.Models;
using ACore.Models;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit.Delete;

public class TestAttributeAuditDeleteHandler<T>(IStorageResolver storageResolver) 
  : TestModuleRequestHandler<TestAttributeAuditDeleteCommand<T>, bool>(storageResolver)
  where T : IConvertible

{
  public override async Task<Result<bool>> Handle(TestAttributeAuditDeleteCommand<T> request, CancellationToken cancellationToken)
  {
   // (await WriteStorages()).Delete<TestAttributeAuditEntity, T>(request.Data.Id);
    return Result.Success(true);
  }
}