using ACore.Base.CQRS.Models;
using ACore.Server.Modules.SettingsDbModule.Storage;
using ACore.Server.Storages;
using ACore.Server.Storages.CQRS;

namespace ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbSave;

public class SettingsDbSaveHandler(IStorageResolver storageResolver) : SettingsDbModuleRequestHandler<SettingsDbSaveCommand, Result>
{
  public override async Task<Result> Handle(SettingsDbSaveCommand request, CancellationToken cancellationToken)
  {
    var allTask = new List<SaveHandlerData<string>>();

    foreach (var storage in storageResolver.WriteStorages<ISettingsDbModuleStorage>())
    {
        allTask.Add(new SaveHandlerData<string>(request.Key, storage, storage.Setting_SaveAsync(request.Key, request.Value, request.IsSystem)));
    }

    await Task.WhenAll(allTask.Select(e => e.Task));
    return DbSaveResult.SuccessWithPkValues(allTask.ToDictionary(k => k.Storage.StorageDefinition.Type, v => (object)v.Entity));
  }
}