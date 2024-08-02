using System.Reflection;
using Xunit;

namespace JMCore.TestsIntegrations.ServerT.StoragesT.ModulesT.BasicStorageT;

public class SettingsTests : BasicStructureBaseTests
{
  [Fact]
  public async Task SaveGetTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
    {
      var db = GetBasicStorageModule(storageType);
      await Tests.Server.Modules.SettingModule.SettingsTests.CheckSettingEntity(db, Mediator); 
    });
  }
}