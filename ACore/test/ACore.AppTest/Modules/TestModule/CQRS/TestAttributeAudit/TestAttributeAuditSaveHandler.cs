using ACore.AppTest.Modules.TestModule.Storages.EF.Models;
using ACore.Extensions;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit;

internal class TestAttributeAuditSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestAttributeAuditSaveCommand, int>(storageResolver)
{
  public override async Task<int> Handle(TestAttributeAuditSaveCommand request, CancellationToken cancellationToken)
  {
    var en = new TestAttributeAuditEntity();
    en.CopyPropertiesFrom(request.Data);
    return await WriteStorage().Save<TestAttributeAuditEntity, int>(en);
  }
}