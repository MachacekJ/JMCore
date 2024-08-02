using System.Reflection;
using ACore.Modules.CacheModule.CQRS.Models;
using FluentAssertions;
using ACore.Modules.CacheModule;
using Xunit;

namespace ACore.Tests.Core.ServicesT.JMCacheT.JMMemoryCacheT.JMCacheT;

public class JMCacheInterfaceTests : MemoryBaseTests
{
    [Fact]
    public async Task Ok()
    {
        var method = MethodBase.GetCurrentMethod();
        await RunTestAsync(method, async () =>
        {
            // Arrange.
            var cacheValue = "Hello";

            var cacheKey = JMCacheKey.Create(JMCacheTestCategoryT.TestCache, "Test");
            MemoryCache.Set(cacheKey, "Hello");

            // Act.
            var aa = MemoryCache.Get<string>(cacheKey);
            string? res;
            MemoryCache.TryGetValue(cacheKey, out res);

            // Assert.
            Assert.Equal(aa, cacheValue);
            Assert.Equal(res, cacheValue);


            // Test 2
            // Act.
            MemoryCache.Remove(cacheKey);
            var bb = MemoryCache.Get<string>(cacheKey);
            var bb2 = MemoryCache.TryGetValue(cacheKey, out string? res2);


            // Assert.
            Assert.Null(bb);
            res2.Should().BeNull();
            Assert.False(bb2);

            await Task.Delay(0);
        });
    }

    [Fact]
    public async Task CheckCategories()
    {
        var method = MethodBase.GetCurrentMethod();
        await RunTestAsync(method, async () =>
        {
            // Assert.
            Assert.True(MemoryCache.Categories.All.Count == 1);
            Assert.True(MemoryCache.Categories.All.ContainsKey(JMCacheTestCategoryT.TestCache));
            await Task.Delay(0);
        });
    }

    [Fact]
    public async Task NotExistValue()
    {
        var method = MethodBase.GetCurrentMethod();
        await RunTestAsync(method, async () =>
        {
            // Arrange.
            var cacheKey = JMCacheKey.Create(JMCacheTestCategoryT.TestCache, "Test");
            // Act.
            var aa = MemoryCache.Get<string>(cacheKey);
            // Assert.
            Assert.Null(aa);
            await Task.Delay(0);
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
            Assert.Throws<Exception>(() => MemoryCache.Get<string>(cacheKey));

            await Task.Delay(0);
        });
    }
}