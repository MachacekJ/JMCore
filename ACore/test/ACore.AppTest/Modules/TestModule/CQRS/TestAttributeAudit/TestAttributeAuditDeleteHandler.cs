using ACore.AppTest.Modules.TestModule.Storages.Models;
using ACore.Extensions;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit;

internal class TestAttributeAuditDeleteHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestAttributeAuditDeleteCommand, bool>(storageResolver)
{
  public override async Task<bool> Handle(TestAttributeAuditDeleteCommand request, CancellationToken cancellationToken)
  {
    var en = new TestAttributeAuditEntity();
    en.CopyPropertiesFrom(request.Data);
    
    List<Task> task = [..AllTestStorageWriteContexts().Select(context 
      => context.DeleteAsync(en))];
    await Task.WhenAll(task);
    return true;
  }
}