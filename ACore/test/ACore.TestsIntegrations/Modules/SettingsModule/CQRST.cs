using System.Reflection;
using ACore.Server.Modules.SettingsDbModule.CQRS.SettingsGet;
using ACore.Server.Modules.SettingsDbModule.CQRS.SettingsSave;
using ACore.Server.Storages.Models;
using FluentAssertions;
using Xunit;

namespace ACore.TestsIntegrations.Modules.SettingModule;

public class CQRST : BasicStructureBase
{
  [Fact]
  public async Task SettingsCommandAndQueryGlobalTest()
  {
    const string key = "key";
    const string value = "value";

    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async () =>
    {
      await Mediator.Send(new SettingsDbSaveCommand(StorageTypeEnum.AllRegistered, key, value));
      foreach (var storageType in GetAllStorageType(StorageTypesToTest))
      {
        await CheckSettingValue(storageType, key, value);
      }

      var result3 = await Mediator.Send(new SettingsDbGetQuery(key));
      result3.Should().Be(value);
    });
  }

  [Fact]
  public async Task SettingsCommandAndQuerySpecificTest()
  {
    const string key = "key";
    const string value = "value";

    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
    {
      await Mediator.Send(new SettingsDbSaveCommand(storageType, key, value));
      await CheckSettingValue(storageType, key, value);
    });
  }

  private async Task CheckSettingValue(StorageTypeEnum storageType, string key, string value)
  {
    var result = await Mediator.Send(new SettingsDbGetQuery(storageType, key));
    result.Should().Be(value);
  }
}