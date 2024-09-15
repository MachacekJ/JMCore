using ACore.Server.Modules.SettingsDbModule.Storage;
using ACore.Server.Storages.Models;
using ACore.TestsIntegrations.BaseInfrastructure.Storages;

namespace ACore.TestsIntegrations.Modules.SettingModule;

public class BasicStructureBase : StorageBase
{
  protected static StorageTypeEnum StorageTypesToTest => StorageTypeEnum.Mongo | StorageTypeEnum.Postgres;

  
  protected ISettingsDbMoudleStorage GetBasicStorageModule(StorageTypeEnum storageType)
    => StorageResolver?.FirstReadOnlyStorage<ISettingsDbMoudleStorage>(storageType) ?? throw new ArgumentNullException($"{nameof(ISettingsDbMoudleStorage)} is not implemented.");
}