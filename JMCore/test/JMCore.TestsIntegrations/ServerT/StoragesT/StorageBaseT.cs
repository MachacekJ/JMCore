using System.Reflection;
using JMCore.Extensions;
using JMCore.Server.Configuration.Storage;
using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Services.JMCache;
using JMCore.Services.JMCache;
using JMCore.Tests.ServerT;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.TestsIntegrations.ServerT.StoragesT;

public abstract class StorageBaseT : ServerTestBaseT
{
  protected abstract IEnumerable<string> RequiredBaseStorageModules { get; }

  private const string DbNameRemove = "JMCore_TestsIntegrations_ServerT_DbT_";
  private readonly List<IStorageRegistrationT> _allStorages = [];

  protected IStorageResolver StorageResolver = null!;
  private StorageTypeEnum _storageType = StorageTypeEnum.AllRegistered;

  private string DbName { get; set; } = null!;

  protected async Task RunStorageTestAsync(StorageTypeEnum storageType, MemberInfo? method, Func<Task> testCode)
  {
    _storageType = storageType;
    await RunTestAsync(method, testCode);
  }

  protected async Task RunStorageTestAsync(StorageTypeEnum storageType, MemberInfo? method, Func<StorageTypeEnum, Task> testCode)
  {
    _storageType = storageType;
    await RunTestAsync(method, async () =>
    {
      var aa = GetAllStorageType(storageType).ToList();
      foreach (var storageTypeLocal in aa)
      {
        await testCode(storageTypeLocal);
      }
    });
  }

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    DbName = TestData.TestName.Replace(DbNameRemove, string.Empty).ToLower();

    StorageResolver = new StorageResolver();
    sc.AddSingleton(StorageResolver);

    if (_storageType.HasFlag(StorageTypeEnum.Memory))
      _allStorages.Add(new MemoryStorageRegistrationT());

    if (_storageType.HasFlag(StorageTypeEnum.Postgres))
      _allStorages.Add(new PGStorageRegistrationT(DbName));

    if (_storageType.HasFlag(StorageTypeEnum.Mongo))
      _allStorages.Add(new MongoStorageRegistrationT(DbName));

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