using System.Reflection;
using FluentAssertions;
using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.CQRS.Storages.BasicModule.SettingGet;
using JMCore.Server.CQRS.Storages.BasicModule.SettingSave;
using Xunit;

namespace JMCore.TestsIntegrations.ServerT.StoragesT.ModulesT.BasicStorageT;

public class CQRST : BasicStructureBaseT
{
  [Fact]
  public async Task SettingsCommandAndQueryGlobalTest()
  {
    const string key = "key";
    const string value = "value";

    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async () =>
    {
      await Mediator.Send(new SettingSaveCommand(StorageTypeEnum.AllRegistered, key, value));
      foreach (var storageType in GetAllStorageType(StorageTypesToTest))
      {
        await CheckSettingValue(storageType, key, value);
      }

      var result3 = await Mediator.Send(new SettingGetQuery(key));
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
      await Mediator.Send(new SettingSaveCommand(storageType, key, value));
      await CheckSettingValue(storageType, key, value);
    });
  }

  private async Task CheckSettingValue(StorageTypeEnum storageType, string key, string value)
  {
    var result = await Mediator.Send(new SettingGetQuery(storageType, key));
    result.Should().Be(value);
  }
}