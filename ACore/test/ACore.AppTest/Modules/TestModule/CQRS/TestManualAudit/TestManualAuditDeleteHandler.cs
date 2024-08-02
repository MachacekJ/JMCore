using ACore.AppTest.Modules.TestModule.Storages.Models;
using ACore.Extensions;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestManualAudit;

internal class TestManualAuditDeleteHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestManualAuditDeleteCommand, bool>(storageResolver)
{
  public override async Task<bool> Handle(TestManualAuditDeleteCommand request, CancellationToken cancellationToken)
  {
    var en = new TestManualAuditEntity();
    en.CopyPropertiesFrom(request.Data);
    
    List<Task> task = [..AllTestStorageWriteContexts().Select(context 
      => context.DeleteAsync(en))];
    await Task.WhenAll(task);
    return true;
  }
}