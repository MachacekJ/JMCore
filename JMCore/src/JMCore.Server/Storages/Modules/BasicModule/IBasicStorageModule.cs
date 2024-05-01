namespace JMCore.Server.Storages.Modules.BasicModule;

public interface IBasicStorageModule : IStorage
{
  Task<string?> Setting_GetAsync(string key, bool isRequired = true);
  Task Setting_SaveAsync(string key, string value, bool isSystem = false);
}