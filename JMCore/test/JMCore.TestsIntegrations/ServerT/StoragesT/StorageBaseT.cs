using System.Reflection;
using JMCore.Server.Configuration.Storage;
using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Services.JMCache;
using JMCore.Services.JMCache;
using JMCore.Tests.ServerT;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.TestsIntegrations.ServerT.StoragesT;

/// <summary>
/// Working with postgres database (msssql oboslete).
/// </summary>
public class StorageBaseT : ServerTestBaseT
{
  private const string DbNameRemove = "JMCore_TestsIntegrations_ServerT_DbT_";
  private readonly List<IStorageRegistrationT> _allStorages = [];

  private IStorageResolver _storageResolver = null!;
  private StorageTypeEnum _storageType = StorageTypeEnum.AllRegistered;

  private string DbName { get; set; } = null!;

  protected async Task RunTestAsync2(StorageTypeEnum storageType, MemberInfo? method, Func<Task> testCode)
  {
    _storageType = storageType;
    await RunTestAsync(method, testCode);
  }

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    DbName = TestData.TestName.Replace(DbNameRemove, string.Empty).ToLower();
    
    _storageResolver = new StorageResolver();
    sc.AddSingleton(_storageResolver);
    
    if (_storageType.HasFlag(StorageTypeEnum.Postgres))
      _allStorages.Add(new PGStorageRegistrationT(DbName));

    if (_storageType.HasFlag(StorageTypeEnum.Mongo))
      _allStorages.Add(new MongoStorageRegistrationT(DbName));
    
    foreach (var storage in _allStorages)
    {
      storage.RegisterServices(sc, Configuration, _storageResolver);
    }
    
    _storageResolver.RegisterServices(sc);
    sc.AddJMMemoryCache<JMCacheServerCategory>();
  }

  protected override async Task GetServicesAsync(IServiceProvider sp)
  {
    await base.GetServicesAsync(sp);
    foreach (var storage in _allStorages)
    {
      storage.GetServices(sp);
    }
    await _storageResolver.ConfigureStorages(sp);
   
  }

  protected override async Task FinishedTestAsync()
  {
    foreach (var storage in _allStorages)
    {
      storage.FinishedTest();
    }
    await base.FinishedTestAsync();
  }
 
}

public class MasterDb(DbContextOptions<MasterDb> options) : DbContext(options);