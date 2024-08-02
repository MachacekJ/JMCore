using System.Reflection;
using JMCore.Extensions;
using JMCore.Modules.CacheModule;
using JMCore.Server.Services.JMCache;
using JMCore.Server.Storages;
using JMCore.Server.Storages.Configuration;
using JMCore.Server.Storages.Models;
using JMCore.Tests.BaseInfrastructure.Models;
using JMCore.Tests.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.TestsIntegrations.ServerT.StoragesT;

public abstract class StorageBaseTests : ServerBaseTests
{
  protected abstract IEnumerable<string> RequiredBaseStorageModules { get; }
  private List<IStorageRegistrationT> _allStorages = [];

  protected IStorageResolver StorageResolver = null!;
  private StorageTypeEnum _storageType = StorageTypeEnum.AllRegistered;

  protected async Task RunStorageTestAsync(StorageTypeEnum storageType, MemberInfo? method, Func<Task> testCode)
  {
    Init(storageType);
    await RunTestAsync(method, testCode);
  }

  protected async Task RunStorageTestAsync(StorageTypeEnum storageType, MemberInfo? method, Func<StorageTypeEnum, Task> testCode)
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

  private void Init(StorageTypeEnum storageType)
  {
    _storageType = storageType;
    _allStorages = [];
  }

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);

    StorageResolver = new StorageResolver();
    sc.AddSingleton(StorageResolver);

    if (_storageType.HasFlag(StorageTypeEnum.Memory))
      throw new Exception("Memory stores use unit tests, not integration tests.");

    if (_storageType.HasFlag(StorageTypeEnum.Postgres))
      _allStorages.Add(new PGStorageRegistrationT(TestData));

    if (_storageType.HasFlag(StorageTypeEnum.Mongo) && !_allStorages.Any(a => a is MongoStorageRegistrationT))
      _allStorages.Add(new MongoStorageRegistrationT(TestData));

    foreach (var storage in _allStorages)
    {
      storage.RegisterServices(sc, Configuration, RequiredBaseStorageModules, StorageResolver);
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

  protected IEnumerable<StorageTypeEnum> GetAllStorageType(StorageTypeEnum storageType)
  {
    return storageType.ToValues().Where(a => (a & StorageTypeEnum.AllRegistered) != StorageTypeEnum.AllRegistered);
  }
}

public class MasterDb(DbContextOptions<MasterDb> options) : DbContext(options);