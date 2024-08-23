using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Storages.Models;
using ACore.TestsIntegrations.BaseInfrastructure.Storages;

namespace ACore.TestsIntegrations.Modules.SettingModule;

public class BasicStructureBase : StorageBase
{
  protected static StorageTypeEnum StorageTypesToTest => StorageTypeEnum.Mongo | StorageTypeEnum.Postgres;

  
  protected ISettingStorageModule GetBasicStorageModule(StorageTypeEnum storageType)
    => StorageResolver?.FirstReadOnlyStorage<ISettingStorageModule>(storageType) ?? throw new ArgumentNullException($"{nameof(ISettingStorageModule)} is not implemented.");
}