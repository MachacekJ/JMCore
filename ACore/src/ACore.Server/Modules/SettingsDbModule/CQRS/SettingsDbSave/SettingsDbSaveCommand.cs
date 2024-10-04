using ACore.Base.CQRS.Models.Results;
using ACore.Server.Storages.Definitions.Models;

namespace ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbSave;

public class SettingsDbSaveCommand(StorageTypeEnum storageType, string key, string value, bool isSystem = false) : SettingsDbModuleRequest<Result>
{
  public StorageTypeEnum StorageType { get; } = storageType;
  public string Key { get; } = key;
  public string Value { get; } = value;
  public bool IsSystem { get; } = isSystem;

  public SettingsDbSaveCommand(string key, string value, bool isSystem = false) : this(StorageTypeEnum.All, key, value, isSystem)
  {
  }
}