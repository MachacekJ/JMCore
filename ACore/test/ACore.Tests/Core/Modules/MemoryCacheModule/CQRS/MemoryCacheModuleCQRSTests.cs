using System.Reflection;
using ACore.Base.Cache;
using ACore.Base.CQRS.Models;
using ACore.Base.CQRS.Models.Results;
using ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheGet;
using ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheRemove;
using ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheSave;
using FluentAssertions;
using Xunit;

namespace ACore.Tests.Core.Modules.MemoryCacheModule.CQRS;

public class MemoryCacheModuleCQRSTests : MemoryCacheModuleBaseTests
{
  [Fact]
  public async Task BaseTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      var cacheValue = "Hello";
      var cacheKey = CacheKey.Create(CacheTestCategories.CacheTest, "Test");

      // Act.
      await Mediator.Send(new MemoryCacheModuleSaveCommand(cacheKey, cacheValue));

      // Assert.
      var result1 = await Mediator.Send(new MemoryCacheModuleGetQuery(cacheKey));
      result1.ResultValue.Should().NotBeNull();
      result1.ResultValue?.GetValue<string?>().Should().Be(cacheValue);

      // Test 2
      // Act.
      await Mediator.Send(new MemoryCacheModuleRemoveKeyCommand(cacheKey));

      // Assert.
      var result2 = (await Mediator.Send(new MemoryCacheModuleGetQuery(cacheKey))).ResultValue;
      result2.Should().BeNull();
    });
  }

  [Fact]
  public async Task RemoveCategoryTest()
  {
    const string cacheValue = "Hello";
    const int count = 10;
    
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      CacheKey cacheKey;
      // Test 1 (adding values to cache) arrange for Test 2
      // Act.
      for (var i = 0; i < count; i++)
      {
        cacheKey = CacheKey.Create(CacheTestCategories.CacheTest, "Test" + i);
        await Mediator.Send(new MemoryCacheModuleSaveCommand(cacheKey, cacheValue + i));
      }

      // Assert
      for (var i = 0; i < count; i++)
      {
        // Act.
        cacheKey = CacheKey.Create(CacheTestCategories.CacheTest, "Test" + i);
        var cacheValueResult = (await Mediator.Send(new MemoryCacheModuleGetQuery(cacheKey))).ResultValue?.GetValue<string>();

        // Assert.
        var expectedCacheValueResult = cacheValue + i;
        cacheValueResult.Should().NotBeNull().And.Be(expectedCacheValueResult);
      }

      // Test 2 - Check if all items in category are removed.
      // Arrange
      var cacheKey2 = CacheKey.Create(CacheTestCategories.CacheTest2, "Test");
      await Mediator.Send(new MemoryCacheModuleSaveCommand(cacheKey2, cacheValue));
      
      // Act. Remove all items in category.
      await Mediator.Send(new MemoryCacheModuleRemoveCategoryCommand(CacheTestCategories.CacheTest));

      // Assert. Check if some cache item exists.
      for (var i = 0; i < count; i++)
      {
        cacheKey = CacheKey.Create(CacheTestCategories.CacheTest, "Test" + i);
        var result2 = (await Mediator.Send(new MemoryCacheModuleGetQuery(cacheKey))).ResultValue;
        result2.Should().BeNull();
      }
      var cacheKey2Result = (await Mediator.Send(new MemoryCacheModuleGetQuery(cacheKey2))).ResultValue?.GetValue<string>();
      cacheKey2Result.Should().NotBeNull().And.Be(cacheValue);
    });
  }
  
  [Fact]
  public async Task RemoveSubCategoryTest()
  {
    const string cacheValue = "Hello";
    const int count = 10;
    const int lengthWithoutCategory = 4; 
    var subCategory = new CacheCategory("subCategory");
    
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      CacheKey cacheKey;

      // Arrange.
      for (var i = 0; i < count; i++)
      {
        cacheKey = i > lengthWithoutCategory
          ? CacheKey.Create(CacheTestCategories.CacheTest, subCategory, "Prefix-Test" + i)
          : CacheKey.Create(CacheTestCategories.CacheTest, "Test" + i);

        await Mediator.Send(new MemoryCacheModuleSaveCommand(cacheKey, cacheValue + i));
      }

      // Act
      await Mediator.Send(new MemoryCacheModuleRemoveCategoryCommand(CacheTestCategories.CacheTest, subCategory));

      // Assert
      for (var i = 0; i < count; i++)
      {
        cacheKey = CacheKey.Create(CacheTestCategories.CacheTest, "Test" + i);
        var result2 = (await Mediator.Send(new MemoryCacheModuleGetQuery(cacheKey))).ResultValue?.GetValue<string>();
        if (i > lengthWithoutCategory)
          result2.Should().BeNull();
        else
          result2.Should().NotBeNull().And.Be(cacheValue + i);
      }
    });
  }

  [Fact]
  public async Task NotExistCategoryTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      // Arrange.
      var cacheKey = CacheKey.Create(new CacheCategory("UnknownFakeCategory"), "Test");

      // Act.
      var exceptionError = await Mediator.Send(new MemoryCacheModuleGetQuery(cacheKey));

      // Assert.
      exceptionError.IsFailure.Should().BeTrue();
      exceptionError.Should().BeAssignableTo(typeof(ExceptionResult<>));
    });
  }
}