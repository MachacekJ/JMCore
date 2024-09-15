using System.Reflection;
using ACore.Base.Cache;
using ACore.Configuration;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace ACore.Tests.Core.Modules.MemoryCacheModule.Storages;

public class MemoryCacheModuleStoragesTests : MemoryCacheModuleBaseTests
{
  private IOptions<ACoreOptions>? _options;

  protected override async Task GetServices(IServiceProvider sp)
  {
    await base.GetServices(sp);
    _options = sp.GetService<IOptions<ACoreOptions>>() ?? throw new ArgumentException($"{nameof(ACoreOptions)} is null.");
  }

  [Fact]
  public async Task BaseTest()
  {
    const string cacheValue = "Hello";

    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      if (MemoryCacheStorage == null)
        throw new Exception($"{nameof(MemoryCacheStorage)} is null.");

      // Arrange.
      var cacheKey = CacheKey.Create(CacheTestCategories.CacheTest, "Test");
      MemoryCacheStorage.Set(cacheKey, "Hello");

      // Act.
      var resultCQRS = MemoryCacheStorage.Get<string>(cacheKey);
      var resultGetValue = MemoryCacheStorage.TryGetValue(cacheKey, out string? resultInterface);

      // Assert.
      resultCQRS.Should().Be(cacheValue);
      resultInterface.Should().Be(cacheValue);
      resultGetValue.Should().BeTrue();

      // Test 2
      // Act.
      MemoryCacheStorage.Remove(cacheKey);
      resultCQRS = MemoryCacheStorage.Get<string>(cacheKey);
      resultGetValue = MemoryCacheStorage.TryGetValue(cacheKey, out string? resultInterface2);


      // Assert.
      resultCQRS.Should().BeNull();
      resultInterface2.Should().BeNull();
      resultGetValue.Should().Be(false);

      await Task.Delay(0);
    });
  }

  [Fact]
  public async Task CheckCategoriesTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      // Assert.
      MemoryCacheStorage?.Categories.Length.Should().Be(_options?.Value.MemoryCacheModuleOptions.Categories.Count);
      MemoryCacheStorage?.Categories.Any(e => e.CategoryNameKey == CacheTestCategories.CacheTest.CategoryNameKey).Should().BeTrue();
      await Task.Delay(0);
    });
  }

  [Fact]
  public async Task NotExistValueTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      // Arrange.
      var cacheKey = CacheKey.Create(CacheTestCategories.CacheTest, "Test");
      // Act.
      var result = MemoryCacheStorage?.Get<string>(cacheKey);
      // Assert.
      result.Should().BeNull();
      await Task.Delay(0);
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

      // Assert.
      Assert.Throws<Exception>(() => MemoryCacheStorage?.Get<string>(cacheKey));

      await Task.Delay(0);
    });
  }
}