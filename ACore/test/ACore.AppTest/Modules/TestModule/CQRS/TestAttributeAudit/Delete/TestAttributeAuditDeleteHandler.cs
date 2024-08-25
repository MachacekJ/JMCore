using ACore.AppTest.Modules.TestModule.Storages.SQL.Models;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit.Delete;

public class TestAttributeAuditDeleteHandler<T>(IStorageResolver storageResolver) 
  : TestModuleRequestHandler<TestAttributeAuditDeleteCommand<T>, bool>(storageResolver)
  where T : IConvertible

{
  public override async Task<bool> Handle(TestAttributeAuditDeleteCommand<T> request, CancellationToken cancellationToken)
  {
    await WriteStorage().Delete<TestAttributeAuditEntity, T>(request.Data.Id);
    return true;
  }
}