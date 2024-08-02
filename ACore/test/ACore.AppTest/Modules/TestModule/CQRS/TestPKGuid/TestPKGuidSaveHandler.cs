using ACore.AppTest.Modules.TestModule.Storages.Models;
using ACore.Extensions;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestPKGuid;

internal class TestPKGuidSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestPKGuidSaveCommand, bool>(storageResolver)
{
  public override async Task<bool> Handle(TestPKGuidSaveCommand request, CancellationToken cancellationToken)
  {
    var en = new TestPKGuidEntity();
    en.CopyPropertiesFrom(request.Data);
    
    List<Task> task = [..AllTestStorageWriteContexts().Select(context 
      => context.SaveAsync(en))];
    await Task.WhenAll(task);
    return true;
  }
}