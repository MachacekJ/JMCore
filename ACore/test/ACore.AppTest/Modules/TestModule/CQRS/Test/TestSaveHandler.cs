using ACore.AppTest.Modules.TestModule.Storages.Models;
using ACore.Extensions;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.Test;

internal class TestSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestSaveCommand, bool>(storageResolver)
{
  public override async Task<bool> Handle(TestSaveCommand request, CancellationToken cancellationToken)
  {
    var en = new TestEntity();
    en.CopyPropertiesFrom(request.Data);
    
    List<Task> task = [..AllTestStorageWriteContexts().Select(context 
      => context.SaveAsync(en))];
    await Task.WhenAll(task);
    return true;
  }
}