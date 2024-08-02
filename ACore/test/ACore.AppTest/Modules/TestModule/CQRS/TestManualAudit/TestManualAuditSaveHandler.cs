using ACore.AppTest.Modules.TestModule.Storages.Models;
using ACore.Extensions;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestManualAudit;

internal class TestManualAuditSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestManualAuditSaveCommand, bool>(storageResolver)
{
  public override async Task<bool> Handle(TestManualAuditSaveCommand request, CancellationToken cancellationToken)
  {
    var en = new TestManualAuditEntity();
    en.CopyPropertiesFrom(request.Data);
    
    List<Task> task = [..AllTestStorageWriteContexts().Select(context 
      => context.SaveAsync(en))];
    await Task.WhenAll(task);
    return true;
  }
}