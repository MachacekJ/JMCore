using ACore.Server.Storages;
using ACore.Tests.Implementations.Modules.TestModule.Storages.Models;
using ACore.Extensions;

namespace ACore.Tests.Implementations.Modules.TestModule.CQRS.TestPKGuid;

public class TestPKGuidSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestPKGuidSaveCommand, bool>(storageResolver)
{
  public override async Task<bool> Handle(TestPKGuidSaveCommand request, CancellationToken cancellationToken)
  {
    var en = new TestPKGuidEntity();
    en.CopyPropertiesFrom(request.Data);
    
    List<Task> task = [..AllTestStorageWriteContexts().Select(context 
      => context.AddAsync(en))];
    await Task.WhenAll(task);
    return true;
  }
}