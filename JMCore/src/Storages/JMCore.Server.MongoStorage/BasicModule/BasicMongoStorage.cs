using JMCore.Server.MongoStorage.BasicModule.Models;
using JMCore.Server.Storages.Modules.BasicModule;
using MongoDB.Driver;

namespace JMCore.Server.MongoStorage.BasicModule;

public class BasicMongoStorage(IMongoCollection<SettingCollection> settingg) :  IBasicStorageModule
{
  
  public async Task<string?> Setting_GetAsync(string key, bool isRequired = true)
  {
    return "aa";
  }

  public Task Setting_SaveAsync(string key, string value, bool isSystem = false)
  {
    return Task.CompletedTask;
  }

  public string ModuleName => nameof(IBasicStorageModule);
}