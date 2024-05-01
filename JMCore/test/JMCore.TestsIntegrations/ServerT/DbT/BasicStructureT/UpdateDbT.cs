using System.Reflection;
using FluentAssertions;
using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.CQRS.DB.BasicStructure.SettingGet;
using JMCore.Server.CQRS.DB.BasicStructure.SettingSave;
using Xunit;

namespace JMCore.TestsIntegrations.ServerT.DbT.BasicStructureT;

public class UpdateDbT : BasicStructureBaseT
{
  [Fact]
  public async Task Ok()
  {
    const string key = "key";
    const string value = "value";
    
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      await Mediator.Send(new SettingSaveCommand(StorageTypeEnum.AllRegistered, key, value));
      var result = await Mediator.Send(new SettingGetQuery(StorageTypeEnum.Memory, key));
      result.Should().Be(value);

      var result2 = await Mediator.Send(new SettingGetQuery(StorageTypeEnum.Postgres, key));
      result2.Should().Be(value);
      
      //var result3 = await Mediator.Send(new SettingGetQuery(StorageTypeEnum.Mongo, key));
      //result3.Should().Be(value);
      
      foreach (var storage in AllDbStorages)
      {
        var res = await storage.Setting_GetAsync(key);
        res.Should().Be(value);
      }
    });
  }
}