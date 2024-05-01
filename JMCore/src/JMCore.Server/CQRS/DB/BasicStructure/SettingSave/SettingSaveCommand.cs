using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Base.EF;

namespace JMCore.Server.CQRS.DB.BasicStructure.SettingSave;

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
}
