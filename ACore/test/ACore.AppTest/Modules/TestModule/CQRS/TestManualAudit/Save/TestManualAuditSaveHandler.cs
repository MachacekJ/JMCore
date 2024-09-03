using ACore.AppTest.Modules.TestModule.CQRS.TestManualAudit.Models;
using ACore.AppTest.Modules.TestModule.Storages.SQL.Models;
using ACore.Models;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestManualAudit.Save;

internal class TestManualAuditSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestManualAuditSaveCommand, TestManualAuditData>(storageResolver)
{
  public override async Task<Result<TestManualAuditData>> Handle(TestManualAuditSaveCommand request, CancellationToken cancellationToken)
  {
    var en = TestManualAuditEntity.Create(request.Data);
    //await WriteStorages().Save<TestManualAuditEntity, long>(en);
    return Result.Success(en.ToData());
  }
}