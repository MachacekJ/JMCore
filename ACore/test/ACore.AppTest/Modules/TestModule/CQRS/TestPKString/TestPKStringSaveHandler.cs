using ACore.AppTest.Modules.TestModule.Storages.Models;
using ACore.Extensions;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestPKString;

internal class TestPKStringSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestPKStringSaveCommand, bool>(storageResolver)
{
  public override async Task<bool> Handle(TestPKStringSaveCommand request, CancellationToken cancellationToken)
  {
    var en = new TestPKStringEntity();
    en.CopyPropertiesFrom(request.Data);
    
    List<Task> task = [..AllTestStorageWriteContexts().Select(context 
      => context.SaveAsync(en))];
    await Task.WhenAll(task);
    return true;
  }
}