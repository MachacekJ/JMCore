using JMCore.Server.Storages.EF;
using JMCore.Server.Storages.Models;

namespace JMCore.Server.Modules.SettingModule.CQRS.SettingSave;

/// <summary>
/// 
/// </summary>
/// <param name="storageType">Use flag for multiple storage save.</param>
/// <param name="key"></param>
/// <param name="value"></param>
/// <param name="isSystem"></param>
public class SettingSaveCommand(StorageTypeEnum storageType, string key, string value, bool isSystem = false) : IDbRequest
{
  public StorageTypeEnum StorageType { get; } = storageType;
  public string Key { get; } = key;
  public string Value { get; } = value;
  public bool IsSystem { get; } = isSystem;

  public SettingSaveCommand(string key, string value, bool isSystem = false) : this(StorageTypeEnum.AllRegistered, key, value, isSystem)
  {
  }
}