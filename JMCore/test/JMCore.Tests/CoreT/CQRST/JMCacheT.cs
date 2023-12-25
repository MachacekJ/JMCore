using System.Reflection;
using JMCore.CQRS.JMCache.CacheGet;
using JMCore.CQRS.JMCache.CacheRemove;
using JMCore.CQRS.JMCache.CacheSave;
using JMCore.Services.JMCache;
using JMCore.Tests.CoreT.ServicesT.JMCacheT;
using JMCore.Tests.CoreT.ServicesT.JMCacheT.JMMemoryCacheT;
using Xunit;

namespace JMCore.Tests.CoreT.CQRST;

public class JMCacheT : MemoryBaseT
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
            await Mediator.Send(new CacheSaveCommand(cacheKey, cacheValue));

            // Assert.
            var result1 = await Mediator.Send(new CacheGetQuery(cacheKey));
            Assert.NotNull(result1!.Value);
            Assert.Equal(result1.Value, cacheValue);

            // Test 2
            // Act.
            await Mediator.Send(new CacheRemoveCommand(cacheKey));

            // Assert.
            var result2 = await Mediator.Send(new CacheGetQuery(cacheKey));
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
                await Mediator.Send(new CacheSaveCommand(cacheKey, cacheValue + i));
            }

            // Test 1
            for (int i = 0; i < count; i++)
            {
                // Act.
                cacheKey = JMCacheKey.Create(JMCacheTestCategoryT.TestCache, "Test" + i);
                var cacheValueRes = cacheValue + i;
                var result1 = await Mediator.Send(new CacheGetQuery(cacheKey));

                // Assert.
                Assert.NotNull(result1!.Value);
                Assert.Equal(result1.Value, cacheValueRes);
            }
            
            // Act.
            await Mediator.Send(new CacheRemoveCommand(JMCacheTestCategoryT.TestCache));

            // Assert.
            for (int i = 0; i < count; i++)
            {
                cacheKey = JMCacheKey.Create(JMCacheTestCategoryT.TestCache, "Test" + i);
                var result2 = await Mediator.Send(new CacheGetQuery(cacheKey));
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

                await Mediator.Send(new CacheSaveCommand(cacheKey, cacheValue + i));
            }
            // Act
            await Mediator.Send(new CacheRemoveCommand(JMCacheTestCategoryT.TestCache, "Prefix"));

            // Assert
            for (var i = 0; i < count; i++)
            {
                cacheKey = JMCacheKey.Create(JMCacheTestCategoryT.TestCache, "Test" + i);
                var result2 = await Mediator.Send(new CacheGetQuery(cacheKey));
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
            await Assert.ThrowsAsync<Exception>(async () => _ = await Mediator.Send(new CacheGetQuery(cacheKey)));

            await Task.Delay(0);
        });
    }
}