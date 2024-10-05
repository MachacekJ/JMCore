using ACore.Base.CQRS.Results;
using ACore.Extensions;
using ACore.Server.Configuration;
using ACore.Server.Modules.SettingsDbModule.Storage;
using ACore.Server.Storages.CQRS;
using ACore.Server.Storages.Services.StorageResolvers;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbSave;

public class SettingsDbSaveHandler(IStorageResolver storageResolver, IOptions<ACoreServerOptions> serverOptions) : SettingsDbModuleRequestHandler<SettingsDbSaveCommand, Result>
{
  public override async Task<Result> Handle(SettingsDbSaveCommand request, CancellationToken cancellationToken)
  {
    var allTask = new List<SaveProcessExecutor<string>>();

    foreach (var storage in storageResolver.WriteStorages<ISettingsDbModuleStorage>())
    {
      allTask.Add(new SaveProcessExecutor<string>(request.Key, storage, storage.Setting_SaveAsync(request.Key, request.Value, request.IsSystem)));
    }

    await Task.WhenAll(allTask.Select(e => e.Task));
    return DbSaveResult.SuccessWithValues(allTask.ToDictionary(
      k => k.Storage.StorageDefinition.Type,
      v => new DbSaveResultData(
        v.Entity,
        v.WithHash ? v.Entity.HashObject(serverOptions.Value.ACoreOptions.SaltForHash) : null
      )));
  }
}