using JMCore.Extensions;
using JMCore.Server.Storages;
using JMCore.Tests.Implementations.Modules.TestModule.Storages.Models;

namespace JMCore.Tests.Implementations.Modules.TestModule.CQRS.TestPKGuid;

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