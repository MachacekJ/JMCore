using System.Reflection;
using ACore.Tests.Core.ServicesT.JMCacheT;
using ACore.Tests.Core.ServicesT.JMCacheT.JMMemoryCacheT;
using ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheGet;
using ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheRemove;
using ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheSave;
using ACore.Services.Cache;
using ACore.Services.Cache.Models;
using Xunit;

namespace ACore.Tests.Core.CQRST;

public class CacheTests : MemoryBaseTests
{
  [Fact]
  public async Task Ok()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      var cacheValue = "Hello";
      var cacheKey = CacheKey.Create(CacheTestCategoryT.CacheTest, "Test");

      // Act.
      await Mediator.Send(new MemoryCacheModuleSaveCommand(cacheKey, cacheValue));

      // Assert.
      var result1 = await Mediator.Send(new MemoryCacheModuleGetQuery(cacheKey));
      Assert.NotNull(result1!.ResultValue);
      Assert.Equal(result1.ResultValue.ObjectValue, cacheValue);

      // Test 2
      // Act.
      await Mediator.Send(new MemoryCacheModuleRemoveKeyCommand(cacheKey));

      // Assert.
      var result2 = (await Mediator.Send(new MemoryCacheModuleGetQuery(cacheKey))).ResultValue;
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
      CacheKey cacheKey;

      // Arrange.
      for (int i = 0; i < count; i++)
      {
        cacheKey = CacheKey.Create(CacheTestCategoryT.CacheTest, "Test" + i);
        await Mediator.Send(new MemoryCacheModuleSaveCommand(cacheKey, cacheValue + i));
      }

      // Test 1
      for (int i = 0; i < count; i++)
      {
        // Act.
        cacheKey = CacheKey.Create(CacheTestCategoryT.CacheTest, "Test" + i);
        var cacheValueRes = cacheValue + i;
        var result1 = (await Mediator.Send(new MemoryCacheModuleGetQuery(cacheKey))).ResultValue;

        // Assert.
        Assert.NotNull(result1!.ObjectValue);
        Assert.Equal(result1.ObjectValue, cacheValueRes);
      }

      // Act. Remove all items in category.
      await Mediator.Send(new MemoryCacheModuleRemoveCategoryCommand(CacheTestCategoryT.CacheTest));

      // Assert. Remove items in category start with.
      for (int i = 0; i < count; i++)
      {
        cacheKey = CacheKey.Create(CacheTestCategoryT.CacheTest, "Test" + i);
        var result2 = (await Mediator.Send(new MemoryCacheModuleGetQuery(cacheKey))).ResultValue;
        Assert.Null(result2);
      }
    });
  }


  [Fact]
  public async Task RemoveSubCategoryTest()
  {
    var subCategory = new CacheCategory("subCategory");
    const int count = 10;
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      var cacheValue = "Hello";
      CacheKey cacheKey;

      // Arrange.
      for (var i = 0; i < count; i++)
      {
        cacheKey = i > 4
          ? CacheKey.Create(CacheTestCategoryT.CacheTest, subCategory, "Prefix-Test" + i)
          : CacheKey.Create(CacheTestCategoryT.CacheTest, "Test" + i);

        await Mediator.Send(new MemoryCacheModuleSaveCommand(cacheKey, cacheValue + i));
      }

      // Act
      await Mediator.Send(new MemoryCacheModuleRemoveCategoryCommand(CacheTestCategoryT.CacheTest, subCategory));

      // Assert
      for (var i = 0; i < count; i++)
      {
        cacheKey = CacheKey.Create(CacheTestCategoryT.CacheTest, "Test" + i);
        var result2 = (await Mediator.Send(new MemoryCacheModuleGetQuery(cacheKey))).ResultValue;
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
      var cacheKey = CacheKey.Create(new CacheCategory("UnknownFakeCategory"), "Test");

      // Assert.
      await Assert.ThrowsAsync<Exception>(async () => _ = await Mediator.Send(new MemoryCacheModuleGetQuery(cacheKey)));

      await Task.Delay(0);
    });
  }
}