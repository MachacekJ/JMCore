using ACore.AppTest.Modules.TestModule.Storages.SQL.Models;
using ACore.Extensions;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestManualAudit.Save;

internal class TestManualAuditSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestManualAuditSaveCommand, long>(storageResolver)
{
  public override async Task<long> Handle(TestManualAuditSaveCommand request, CancellationToken cancellationToken)
  {
    var en = new TestManualAuditEntity();
    en.CopyPropertiesFrom(request.Data);
    return await WriteStorage().Save<TestManualAuditEntity, long>(en);
  }
}