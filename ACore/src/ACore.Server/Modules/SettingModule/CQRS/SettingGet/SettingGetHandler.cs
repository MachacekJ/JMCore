using ACore.Models;
using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Storages;

namespace ACore.Server.Modules.SettingModule.CQRS.SettingGet;

public class SettingGetHandler(IStorageResolver storageResolver) : SettingModuleRequestHandler<SettingGetQuery, Result<string?>>
{
  public override async Task<Result<string?>> Handle(SettingGetQuery request, CancellationToken cancellationToken)
  {
    var storageImplementation = storageResolver.FirstReadOnlyStorage<ISettingStorageModule>(request.StorageType);
    var res= await storageImplementation.Setting_GetAsync(request.Key, request.IsRequired);
    return Result.Success(res);
  }
}