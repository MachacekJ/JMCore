using JMCore.Server.Storages.Abstract;

namespace JMCore.Server.CQRS.DB.BasicStructure.SettingSave;

public class SettingSaveCommand(string key, string value, bool isSystem = false) : IDbRequest
{
    public string Key { get; } = key;
    public string Value { get; } = value;
    public bool IsSystem { get; } = isSystem;
}
