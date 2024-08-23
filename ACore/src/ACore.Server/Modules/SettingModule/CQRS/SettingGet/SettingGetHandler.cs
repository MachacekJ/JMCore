using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Storages;

namespace ACore.Server.Modules.SettingModule.CQRS.SettingGet;

public class SettingGetHandler(IStorageResolver storageResolver) : SettingModuleRequestHandler<SettingGetQuery, string?>()
{
  public override async Task<string?> Handle(SettingGetQuery request, CancellationToken cancellationToken)
  {
    var storageImplementation = storageResolver.FirstReadOnlyStorage<ISettingStorageModule>(request.StorageType);
    return await storageImplementation.Setting_GetAsync(request.Key, request.IsRequired);
  }
}