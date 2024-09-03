using ACore.CQRS;
using ACore.Server.Storages.Models;

namespace ACore.Server.Modules.SettingModule.CQRS.SettingGet;

public class SettingGetQuery(StorageTypeEnum storageType, string key, bool isRequired = false) : SettingModuleRequest<string?>
{
  public StorageTypeEnum StorageType { get; } = storageType;

  public string Key { get; } = key;
  public bool IsRequired { get; } = isRequired;

  public SettingGetQuery(string key, bool isRequired = false) : this(StorageTypeEnum.AllRegistered, key, isRequired)
  {
  }
}