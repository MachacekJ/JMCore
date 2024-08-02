using System.Reflection;
using JMCore.Modules.CacheModule;
using JMCore.Modules.CacheModule.CQRS.CacheGet;
using JMCore.Modules.CacheModule.CQRS.CacheRemove;
using JMCore.Modules.CacheModule.CQRS.CacheSave;
using JMCore.Modules.CacheModule.CQRS.Models;
using JMCore.Tests.Core.ServicesT.JMCacheT;
using JMCore.Tests.Core.ServicesT.JMCacheT.JMMemoryCacheT;
using Xunit;

namespace JMCore.Tests.Core.CQRST;

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
            Assert.NotNull(result1!.Value);
            Assert.Equal(result1.Value, cacheValue);

            // Test 2
            // Act.
            await Mediator.Send(new CacheModuleRemoveCommand(cacheKey));

            // Assert.
            var result2 = await Mediator.Send(new CacheModuleGetQuery(cacheKey));
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
                var result1 = await Mediator.Send(new CacheModuleGetQuery(cacheKey));

                // Assert.
                Assert.NotNull(result1!.Value);
                Assert.Equal(result1.Value, cacheValueRes);
            }
            
            // Act.
            await Mediator.Send(new CacheModuleRemoveCommand(JMCacheTestCategoryT.TestCache));

            // Assert.
            for (int i = 0; i < count; i++)
            {
                cacheKey = JMCacheKey.Create(JMCacheTestCategoryT.TestCache, "Test" + i);
                var result2 = await Mediator.Send(new CacheModuleGetQuery(cacheKey));
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
                var result2 = await Mediator.Send(new CacheModuleGetQuery(cacheKey));
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