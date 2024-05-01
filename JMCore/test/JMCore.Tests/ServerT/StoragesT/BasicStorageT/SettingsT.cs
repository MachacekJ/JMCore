using System.Reflection;
using FluentAssertions;
using JMCore.CQRS.JMCache.CacheGet;
using JMCore.Server.Services.JMCache;
using JMCore.Server.Storages.Modules.BasicModule.Models;
using JMCore.Services.JMCache;
using Xunit;

namespace JMCore.Tests.ServerT.StoragesT.BasicStorageT;

public class SettingsT : BasicStorageModuleEfContextT
{
  [Fact]
  public async Task Ok()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      await Db.Setting_SaveAsync("testkey", "10", true);
      var val = await Db.Setting_GetAsync("testkey");
      val.Should().Be("10");

      await Db.Setting_SaveAsync("testkey", "12", true);

      var val2 = await Db.Setting_GetAsync("testkey");

      Assert.True(val2 == "12");

      var keyCache = JMCacheKey.Create(JMCacheServerCategory.DbTable, nameof(SettingEntity));
      var cacheValue = await Mediator.Send(new CacheGetQuery(keyCache));
      var mem = cacheValue!.Value as List<SettingEntity>;
      Assert.True(mem != null && mem.First(a => a.Key == "testkey").Value == "12");

      Exception? isError = null;
      try
      {
        await Db.Setting_GetAsync("testkey3");
      }
      catch (Exception ex)
      {
        isError = ex;
      }

      Assert.True(isError != null);
    });
  }
}