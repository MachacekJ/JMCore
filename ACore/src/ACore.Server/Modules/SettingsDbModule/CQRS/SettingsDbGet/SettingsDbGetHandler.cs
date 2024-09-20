using ACore.Base.CQRS.Models;
using ACore.Base.CQRS.Models.Results;
using ACore.Server.Modules.SettingsDbModule.Storage;
using ACore.Server.Storages;

namespace ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbGet;

public class SettingsDbGetHandler(IStorageResolver storageResolver) : SettingsDbModuleRequestHandler<SettingsDbGetQuery, Result<string?>>
{
  public override async Task<Result<string?>> Handle(SettingsDbGetQuery request, CancellationToken cancellationToken)
  {
    var storageImplementation = storageResolver.FirstReadOnlyStorage<ISettingsDbModuleStorage>(request.StorageType);
    var res= await storageImplementation.Setting_GetAsync(request.Key, request.IsRequired);
    return Result.Success(res);
  }
}