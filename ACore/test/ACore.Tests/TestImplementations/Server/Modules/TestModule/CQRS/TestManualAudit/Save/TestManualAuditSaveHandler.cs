using ACore.Server.Storages;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestManualAudit.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestManualAudit.Save;

internal class TestManualAuditSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestManualAuditSaveCommand, TestManualAuditData>(storageResolver)
{
  public override async Task<Result<TestManualAuditData>> Handle(TestManualAuditSaveCommand request, CancellationToken cancellationToken)
  {
    var en = TestManualAuditEntity.Create(request.Data);
    //await WriteStorages().Save<TestManualAuditEntity, long>(en);
    return Result.Success(en.ToData());
  }
}