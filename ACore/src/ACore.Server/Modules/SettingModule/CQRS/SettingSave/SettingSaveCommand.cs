using ACore.Base.CQRS.Models;
using ACore.Models;
using ACore.Server.Storages.Models;

namespace ACore.Server.Modules.SettingModule.CQRS.SettingSave;

public class SettingSaveCommand(StorageTypeEnum storageType, string key, string value, bool isSystem = false) : SettingModuleRequest<Result>
{
  public StorageTypeEnum StorageType { get; } = storageType;
  public string Key { get; } = key;
  public string Value { get; } = value;
  public bool IsSystem { get; } = isSystem;

  public SettingSaveCommand(string key, string value, bool isSystem = false) : this(StorageTypeEnum.AllRegistered, key, value, isSystem)
  {
  }
}