using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Storages.Models;
using ACore.TestsIntegrations.BaseInfrastructure.Storages;

namespace ACore.TestsIntegrations.Modules.SettingModule;

public class BasicStructureBaseTests : StorageBaseTests
{
  protected static StorageTypeEnum StorageTypesToTest => StorageTypeEnum.Mongo | StorageTypeEnum.Postgres;

  protected override IEnumerable<string> RequiredBaseStorageModules => new[]
  {
    nameof(IBasicStorageModule)
  };
  
  protected IBasicStorageModule GetBasicStorageModule(StorageTypeEnum storageType) => StorageResolver.FirstReadWriteStorage<IBasicStorageModule>(storageType);
}