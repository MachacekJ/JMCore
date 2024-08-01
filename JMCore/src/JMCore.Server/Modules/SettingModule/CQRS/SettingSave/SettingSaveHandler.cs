using JMCore.Server.Storages;
using JMCore.Server.Storages.Configuration;

namespace JMCore.Server.Modules.SettingModule.CQRS.SettingSave;

public class SettingSaveHandler(IStorageResolver storageResolver) : SettingModuleRequestHandler<SettingSaveCommand>(storageResolver)
{
  public override async Task Handle(SettingSaveCommand request, CancellationToken cancellationToken)
  {
    List<Task> task = [..AllBasicStorageWriteContexts(request.StorageType).Select(context 
      => context.Setting_SaveAsync(request.Key, request.Value, request.IsSystem))];
    await Task.WhenAll(task);
  }
}