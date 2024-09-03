using System.Reflection;
using ACore.Modules.CacheModule.CQRS.CacheGet;
using ACore.Modules.CacheModule.CQRS.CacheRemove;
using ACore.Modules.CacheModule.CQRS.CacheSave;
using ACore.Modules.CacheModule.CQRS.Models;
using ACore.Tests.Core.ServicesT.JMCacheT;
using ACore.Tests.Core.ServicesT.JMCacheT.JMMemoryCacheT;
using ACore.Modules.CacheModule;
using Xunit;

namespace ACore.Tests.Core.CQRST;

public class JMCacheTests : MemoryBaseTests
{
    [Fact]
    public async Task Ok()
    {
        var method = MethodBase.GetCurrentMethod();
        await RunTestAsync(method, async () =>
        {
            var cacheValue = "Hello";
            var cacheKey = JMCacheKey.Create(JMCacheTestCategoryT.TestCache, "Test");

            // Act.
            await Mediator.Send(new CacheModuleSaveCommand(cacheKey, cacheValue));

            // Assert.
            var result1 = await Mediator.Send(new CacheModuleGetQuery(cacheKey));
            Assert.NotNull(result1!.ResultValue);
            Assert.Equal(result1.ResultValue.CacheValue, cacheValue);

            // Test 2
            // Act.
            await Mediator.Send(new CacheModuleRemoveCommand(cacheKey));

            // Assert.
            var result2 = (await Mediator.Send(new CacheModuleGetQuery(cacheKey))).ResultValue;
            Assert.Null(result2);
        });
    }

    [Fact]
    public async Task RemoveCategory()
    {
        var count = 10;
        var method = MethodBase.GetCurrentMethod();
        await RunTestAsync(method, async () =>
        {
            var cacheValue = "Hello";
            JMCacheKey cacheKey;

            // Arrange.
            for (int i = 0; i < count; i++)
            {
                cacheKey = JMCacheKey.Create(JMCacheTestCategoryT.TestCache, "Test" + i);
                await Mediator.Send(new CacheModuleSaveCommand(cacheKey, cacheValue + i));
            }

            // Test 1
            for (int i = 0; i < count; i++)
            {
                // Act.
                cacheKey = JMCacheKey.Create(JMCacheTestCategoryT.TestCache, "Test" + i);
                var cacheValueRes = cacheValue + i;
                var result1 = (await Mediator.Send(new CacheModuleGetQuery(cacheKey))).ResultValue;

                // Assert.
                Assert.NotNull(result1!.CacheValue);
                Assert.Equal(result1.CacheValue, cacheValueRes);
            }
            
            // Act.
            await Mediator.Send(new CacheModuleRemoveCommand(JMCacheTestCategoryT.TestCache));

            // Assert.
            for (int i = 0; i < count; i++)
            {
                cacheKey = JMCacheKey.Create(JMCacheTestCategoryT.TestCache, "Test" + i);
                var result2 = (await Mediator.Send(new CacheModuleGetQuery(cacheKey))).ResultValue;
                Assert.Null(result2);
            }
        });
    }


    [Fact]
    public async Task RemoveCategoryWithPrefix()
    {
        const int count = 10;
        var method = MethodBase.GetCurrentMethod();
        await RunTestAsync(method, async () =>
        {
            var cacheValue = "Hello";
            JMCacheKey cacheKey;

            // Arrange.
            for (var i = 0; i < count; i++)
            {
                cacheKey = i > 4
                    ? JMCacheKey.Create(JMCacheTestCategoryT.TestCache, "Prefix-Test" + i)
                    : JMCacheKey.Create(JMCacheTestCategoryT.TestCache, "Test" + i);

                await Mediator.Send(new CacheModuleSaveCommand(cacheKey, cacheValue + i));
            }
            // Act
            await Mediator.Send(new CacheModuleRemoveCommand(JMCacheTestCategoryT.TestCache, "Prefix"));

            // Assert
            for (var i = 0; i < count; i++)
            {
                cacheKey = JMCacheKey.Create(JMCacheTestCategoryT.TestCache, "Test" + i);
                var result2 = (await Mediator.Send(new CacheModuleGetQuery(cacheKey))).ResultValue;
                if (i > 4)
                    Assert.Null(result2);
                else
                    Assert.NotNull(result2);
            }
        });
    }


    [Fact]
    public async Task NotExistCategory()
    {
        var method = MethodBase.GetCurrentMethod();
        await RunTestAsync(method, async () =>
        {
            // Arrange.
            var cacheKey = JMCacheKey.Create(300, "Test");

            // Assert.
            await Assert.ThrowsAsync<Exception>(async () => _ = await Mediator.Send(new CacheModuleGetQuery(cacheKey)));

            await Task.Delay(0);
        });
    }
}