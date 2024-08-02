using ACore.AppTest.Modules.TestModule.Storages.Models;
using ACore.Extensions;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestValueType;

internal class TestValueTypeSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestValueTypeSaveCommand, bool>(storageResolver)
{
  public override async Task<bool> Handle(TestValueTypeSaveCommand request, CancellationToken cancellationToken)
  {
    var en = new TestValueTypeEntity();
    en.CopyPropertiesFrom(request.Data);
    
    List<Task> task = [..AllTestStorageWriteContexts().Select(context 
      => context.SaveAsync(en))];
    await Task.WhenAll(task);
    return true;
  }
}