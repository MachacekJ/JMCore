using ACore.AppTest.Modules.TestModule.Storages.Models;
using ACore.Extensions;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit;

internal class TestAttributeAuditSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestAttributeAuditSaveCommand, bool>(storageResolver)
{
  public override async Task<bool> Handle(TestAttributeAuditSaveCommand request, CancellationToken cancellationToken)
  {
    var en = new TestAttributeAuditEntity();
    en.CopyPropertiesFrom(request.Data);
    
    List<Task> task = [..AllTestStorageWriteContexts().Select(context 
      => context.SaveAsync(en))];
    await Task.WhenAll(task);
    return true;
  }
}