using System.Reflection;
using ACore.Services.Cache;
using ACore.Services.Cache.Models;
using FluentAssertions;
using Xunit;

namespace ACore.Tests.Core.ServicesT.JMCacheT.JMMemoryCacheT.JMCacheT;

public class CacheInterfaceTests : MemoryBaseTests
{
    [Fact]
    public async Task Ok()
    {
        if (MemoryCacheStorage == null)
            throw new Exception($"{nameof(MemoryCacheStorage)} is null.");
        
        var method = MethodBase.GetCurrentMethod();
        await RunTestAsync(method, async () =>
        {
            // Arrange.
            var cacheValue = "Hello";

            var cacheKey = CacheKey.Create(CacheTestCategoryT.CacheTest, "Test");
            MemoryCacheStorage.Set(cacheKey, "Hello");

            // Act.
            var aa = MemoryCacheStorage.Get<string>(cacheKey);
            string? res;
            MemoryCacheStorage.TryGetValue(cacheKey, out res);

            // Assert.
            Assert.Equal(aa, cacheValue);
            Assert.Equal(res, cacheValue);


            // Test 2
            // Act.
            MemoryCacheStorage.Remove(cacheKey);
            var bb = MemoryCacheStorage.Get<string>(cacheKey);
            var bb2 = MemoryCacheStorage.TryGetValue(cacheKey, out string? res2);


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
            MemoryCacheStorage?.Categories.Length.Should().Be(1);
            MemoryCacheStorage?.Categories.Should().Contain(CacheTestCategoryT.CacheTest);
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
            var cacheKey = CacheKey.Create(CacheTestCategoryT.CacheTest, "Test");
            // Act.
            var aa = MemoryCacheStorage?.Get<string>(cacheKey);
            // Assert.
            aa.Should().BeNull();
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
            var cacheKey = CacheKey.Create( new CacheCategory("UnknownFakeCategory"), "Test");

            // Assert.
            Assert.Throws<Exception>(() => MemoryCacheStorage?.Get<string>(cacheKey));

            await Task.Delay(0);
        });
    }
}