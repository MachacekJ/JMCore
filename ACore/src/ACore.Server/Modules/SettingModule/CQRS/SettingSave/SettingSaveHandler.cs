using ACore.Base.CQRS.Models;
using ACore.Models;
using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Storages;

namespace ACore.Server.Modules.SettingModule.CQRS.SettingSave;

public class SettingSaveHandler(IStorageResolver storageResolver) : SettingModuleRequestHandler<SettingSaveCommand, Result>
{
  public override async Task<Result> Handle(SettingSaveCommand command, CancellationToken cancellationToken)
  {
    List<Task> task = [..storageResolver.WriteStorages<ISettingStorageModule>(command.StorageType).Select(context 
      => context.Setting_SaveAsync(command.Key, command.Value, command.IsSystem))];
    await Task.WhenAll(task);
    return Result.Success();
  }
}