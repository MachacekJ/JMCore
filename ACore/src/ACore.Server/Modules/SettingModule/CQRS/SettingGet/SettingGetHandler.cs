using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Storages;
using ACore.Server.Storages.Models;

namespace ACore.Server.Modules.SettingModule.CQRS.SettingGet;

public class SettingGetHandler(IStorageResolver storageResolver) : SettingModuleRequestHandler<SettingGetQuery, string?>()
{
  public override Task<string?> Handle(SettingGetQuery request, CancellationToken cancellationToken)
  {
    var storageImplementation = storageResolver.FirstReadOnlyStorage<IBasicStorageModule>(request.StorageType);
    return storageImplementation.Setting_GetAsync(request.Key, request.IsRequired);
  }
}