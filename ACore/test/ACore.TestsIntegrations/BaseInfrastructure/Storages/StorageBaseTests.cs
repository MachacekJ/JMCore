using System.Reflection;
using ACore.Extensions;
using ACore.Modules.CacheModule;
using ACore.Server.Services.JMCache;
using ACore.Server.Storages;
using ACore.Server.Storages.Models;
using ACore.Tests.BaseInfrastructure.Models;
using ACore.Tests.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.TestsIntegrations.BaseInfrastructure.Storages;

public abstract class StorageBaseTests : ServerBaseTests
{
  protected abstract IEnumerable<string> RequiredBaseStorageModules { get; }
  private List<IStorageRegistrationT> _allStorages = [];

  protected IStorageResolver? StorageResolver;
  private StorageTypeEnum? _storageType;

  protected async Task RunStorageTestAsync(StorageTypeEnum? storageType, MemberInfo? method, Func<Task> testCode)
  {
    Init(storageType);
    await RunTestAsync(method, testCode);
  }

  protected async Task RunStorageTestAsync(StorageTypeEnum? storageType, MemberInfo? method, Func<StorageTypeEnum, Task> testCode)
  {
    Init(storageType);
    await RunTestAsync(method, async () =>
    {
      var storageTypes = GetAllStorageType(storageType).ToList();
      foreach (var storageTypeLocal in storageTypes)
      {
        await testCode(storageTypeLocal);
      }
    });
  }

  protected async Task RunStorageTestAsync(StorageTypeEnum storageType, TestData testData, Func<StorageTypeEnum, Task> testCode)
  {
    Init(storageType);
    await RunTestAsync(testData, async () =>
    {
      var storageTypes = GetAllStorageType(storageType).ToList();
      foreach (var storageTypeLocal in storageTypes)
      {
        await testCode(storageTypeLocal);
      }
    });
  }

  private void Init(StorageTypeEnum? storageType)
  {
    _storageType = storageType;
    _allStorages = [];
  }

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);

    StorageResolver = new StorageResolver();
    sc.AddSingleton(StorageResolver);

    if (_storageType != null)
    {
      if (_storageType.Value.HasFlag(StorageTypeEnum.Memory))
        throw new Exception("Memory stores use unit tests, not integration tests.");

      if (_storageType.Value.HasFlag(StorageTypeEnum.Postgres))
        _allStorages.Add(new PGStorageRegistrationT(TestData));

      if (_storageType.Value.HasFlag(StorageTypeEnum.Mongo) && !_allStorages.Any(a => a is MongoStorageRegistrationT))
        _allStorages.Add(new MongoStorageRegistrationT(TestData));

      foreach (var storage in _allStorages)
      {
        storage.RegisterServices(sc, Configuration, RequiredBaseStorageModules, StorageResolver);
      }
    }

    StorageResolver.RegisterServices(sc);
    sc.AddJMMemoryCache<JMCacheServerCategory>();
  }

  protected override async Task GetServicesAsync(IServiceProvider sp)
  {
    await base.GetServicesAsync(sp);
    foreach (var storage in _allStorages)
    {
      storage.GetServices(sp);
    }

    if (StorageResolver == null)
      ArgumentNullException.ThrowIfNull(StorageResolver);
    
    await StorageResolver.ConfigureStorages(sp);
  }

  protected override async Task FinishedTestAsync()
  {
    foreach (var storage in _allStorages)
    {
      storage.FinishedTest();
    }

    await base.FinishedTestAsync();
  }

  protected IEnumerable<StorageTypeEnum> GetAllStorageType(StorageTypeEnum? storageType)
  {
    return storageType == null ? [] : storageType.Value.ToValues().Where(a => (a & StorageTypeEnum.AllRegistered) != StorageTypeEnum.AllRegistered);
  }
}

public class MasterDb(DbContextOptions<MasterDb> options) : DbContext(options);