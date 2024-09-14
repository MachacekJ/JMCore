using ACore.Base.CQRS.Models;
using ACore.Server.Modules.SettingsDbModule.Storage;
using ACore.Server.Storages;

namespace ACore.Server.Modules.SettingsDbModule.CQRS.SettingsSave;

public class SettingsDbSaveHandler(IStorageResolver storageResolver) : SettingsDbModuleRequestHandler<SettingsDbSaveCommand, Result>
{
  public override async Task<Result> Handle(SettingsDbSaveCommand command, CancellationToken cancellationToken)
  {
    List<Task> task = [..storageResolver.WriteStorages<ISettingsDbStorageModule>(command.StorageType).Select(context 
      => context.Setting_SaveAsync(command.Key, command.Value, command.IsSystem))];
    await Task.WhenAll(task);
    return Result.Success();
  }
}