using System.Collections;
using System.Reflection;
using JMCore.CQRS.JMCache.CacheGet;
using JMCore.Server.DB.DbContexts.BasicStructure.Models;
using JMCore.Server.Services.JMCache;
using JMCore.Services.JMCache;
using Xunit;

namespace JMCore.Tests.ServerT.DbT.DbContexts.BasicStructureT;

public class SettingsT : BasicStructureBaseT
{
    [Fact]
    public async Task Ok()
    {
        var method = MethodBase.GetCurrentMethod();
        await RunTestAsync(method, async () =>
        {
            await PGDb.Setting_SaveAsync("testkey", "10", true);

            var val = await PGDb.Setting_GetAsync("testkey");

            Assert.True(val == "10");

            await PGDb.Setting_SaveAsync("testkey", "12", true);

            var val2 = await PGDb.Setting_GetAsync("testkey");
            
            Assert.True(val2 == "12");

            var keyCache = JMCacheKey.Create(JMCacheServerCategory.DbTable, nameof(ISettingEntity));
            var cacheValue = await Mediator.Send(new CacheGetQuery(keyCache));
            var mem2 = cacheValue!.Value as IList;
            var mem = mem2?.Cast<ISettingEntity>().ToList();
            
            
            Assert.True(mem != null && mem.First(a => a.Key == "testkey").Value == "12");

            Exception? isError = null;
            try
            {
                await PGDb.Setting_GetAsync("testkey3");
            }
            catch (Exception ex)
            {
                isError = ex;
            }

            Assert.True(isError != null);
        });
    }
}