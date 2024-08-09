using System.Reflection;
using ACore.AppTest.Modules.TestModule.Configuration.Options;
using ACore.Extensions;
using ACore.Modules.CacheModule;
using ACore.Server.Services.JMCache;
using ACore.Server.Storages;
using ACore.Server.Storages.Configuration.Options;
using ACore.Server.Storages.Models;
using ACore.Tests.BaseInfrastructure.Models;
using ACore.Tests.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using InvalidOperationException = System.InvalidOperationException;

namespace ACore.TestsIntegrations.BaseInfrastructure.Storages;

public abstract class StorageBase : ServerBase
{
  private List<IStorageRegistrationT> _allStorages = [];

  protected IStorageResolver? StorageResolver;
  private StorageTypeEnum _storageType;

  public TestModuleOptions GetTestConfig(string? dbName = null)
  {
    return _storageType switch
    {
      StorageTypeEnum.Memory => throw new Exception("Memory is used in Unit test"),
      StorageTypeEnum.Mongo => new TestModuleOptions()
      {
        MongoDb = MongoDb()
      },
      StorageTypeEnum.Postgres => new TestModuleOptions()
      {
        PGDb = PGDb(dbName)
      },
      StorageTypeEnum.AllRegistered => new TestModuleOptions()
      {
        MongoDb = MongoDb(),
        PGDb = PGDb(dbName)
      },
      _ => throw new Exception("Unknow test config")
    };
  }

  private ACoreStorageMongoOptions MongoDb()
  {
    return new ACoreStorageMongoOptions(
      Configuration["TestSettings:ConnectionStringMongo"] ?? throw new InvalidOperationException(),
      TestData.GetDbName());
  }

  private ACoreStoragePGOptions PGDb(string? dbName = null)
  {
    var connectionString = Configuration["TestSettings:ConnectionStringPG"] ?? throw new InvalidOperationException();
    return string.IsNullOrEmpty(dbName)
      ? new ACoreStoragePGOptions(connectionString)
      : new ACoreStoragePGOptions(string.Format(connectionString, dbName));
  }


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

    StorageResolver = new DefaultStorageResolver();
    sc.AddSingleton(StorageResolver);

    // if (_storageType != null)
    //{
    if (_storageType.HasFlag(StorageTypeEnum.Memory))
      throw new Exception("Memory stores use unit tests, not integration tests.");

    if (_storageType.HasFlag(StorageTypeEnum.Postgres))
      _allStorages.Add(new PGStorageRegistrationT(TestData));

    if (_storageType.HasFlag(StorageTypeEnum.Mongo) && !_allStorages.Any(a => a is MongoStorageRegistrationT))
      _allStorages.Add(new MongoStorageRegistrationT(TestData));

    foreach (var storage in _allStorages)
    {
      storage.RegisterServices(sc, GetTestConfig());
    }
    //}

    //StorageResolver.RegisterServices(sc);
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

    // await StorageResolver.ConfigureStorages(sp);
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