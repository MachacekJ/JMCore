using ACore.AppTest.Modules.TestModule.Models;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit;

internal class TestAttributeAuditDeleteHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestAttributeAuditDeleteCommand, bool>(storageResolver)
{
  public override async Task<bool> Handle(TestAttributeAuditDeleteCommand request, CancellationToken cancellationToken)
  {
    await WriteStorage().Delete<TestAttributeAuditData, int>(request.Data.Id);
    return true;
  }
}