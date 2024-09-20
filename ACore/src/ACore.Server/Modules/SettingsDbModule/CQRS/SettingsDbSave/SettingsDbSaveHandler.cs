using ACore.Base.CQRS.Models.Results;
using ACore.Server.Configuration;
using ACore.Server.Modules.SettingsDbModule.Storage;
using ACore.Server.Storages;
using ACore.Server.Storages.CQRS;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbSave;

public class SettingsDbSaveHandler(IStorageResolver storageResolver, IOptions<ACoreServerOptions> serverOptions) : SettingsDbModuleRequestHandler<SettingsDbSaveCommand, Result>
{
  public override async Task<Result> Handle(SettingsDbSaveCommand request, CancellationToken cancellationToken)
  {
    var allTask = new List<SaveHandlerData<string>>();

    foreach (var storage in storageResolver.WriteStorages<ISettingsDbModuleStorage>())
    {
        allTask.Add(new SaveHandlerData<string>(request.Key, storage, storage.Setting_SaveAsync(request.Key, request.Value, request.IsSystem)));
    }

    await Task.WhenAll(allTask.Select(e => e.Task));
    return DbSaveResult.SuccessWithData(allTask, serverOptions.Value.ACoreOptions.SaltForHash);
  }
}