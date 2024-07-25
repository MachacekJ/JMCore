using System.Reflection;
using JMCore.Extensions;
using JMCore.Server.Configuration.Storage;
using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Services.JMCache;
using JMCore.Services.JMCache;
using JMCore.Tests.ServerT;
using JMCore.TestsIntegrations.ServerT.StoragesT.ModulesT.TestStorageModuleT.PGT;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.TestsIntegrations.ServerT.StoragesT;

public abstract class StorageBaseT : ServerTestBaseT
{
  /// <summary>
  /// Mongo size database name limitation.
  /// </summary>
  private const int MaximumLengthOfDb = 63;
  
  protected abstract IEnumerable<string> RequiredBaseStorageModules { get; }
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
      var storageTypes = GetAllStorageType(storageType).ToList();
      foreach (var storageTypeLocal in storageTypes)
      {
        await testCode(storageTypeLocal);
      }
    });
  }

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    var shrinkStrings = new List<string>() { 
      nameof(JMCore) ,
      nameof(TestsIntegrations),
      nameof(ServerT),
      nameof(StoragesT),
      nameof(ModulesT)
     };
    var testName = shrinkStrings.Aggregate(TestData.TestName, (current, name) 
      => current.Replace($"{name}_", string.Empty)).ToLower();
    if (testName.Length > MaximumLengthOfDb)
      testName = testName.Substring(testName.Length - MaximumLengthOfDb);
      //throw new Exception($"Name of database '{testName}' is longer then {MaximumLengthOfDb}.");
    DbName = testName;

    StorageResolver = new StorageResolver();
    sc.AddSingleton(StorageResolver);

    if (_storageType.HasFlag(StorageTypeEnum.Memory))
      throw new Exception("Memory stores use unit tests, not integration tests.");

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