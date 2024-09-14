using ACore.Base.CQRS.Models;
using ACore.Server.Modules.SettingsDbModule.Storage;
using ACore.Server.Storages;

namespace ACore.Server.Modules.SettingsDbModule.CQRS.SettingsGet;

public class SettingsDbsDbGetHandler(IStorageResolver storageResolver) : SettingsDbModuleRequestHandler<SettingsesDbDbGetQuery, Result<string?>>
{
  public override async Task<Result<string?>> Handle(SettingsesDbDbGetQuery request, CancellationToken cancellationToken)
  {
    var storageImplementation = storageResolver.FirstReadOnlyStorage<ISettingsDbStorageModule>(request.StorageType);
    var res= await storageImplementation.Setting_GetAsync(request.Key, request.IsRequired);
    return Result.Success(res);
  }
}