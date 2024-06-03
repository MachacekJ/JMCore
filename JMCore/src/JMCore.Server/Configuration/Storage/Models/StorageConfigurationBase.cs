using JMCore.Server.Storages.Base.EF;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Server.Configuration.Storage.Models;

public abstract class StorageConfigurationBase(IEnumerable<string> requiredStorageModules, StorageModeEnum storageMode = StorageModeEnum.ReadWrite)
{
  private readonly Dictionary<string, object> _implementations = [];
  protected readonly IEnumerable<string> RequiredStorageModules = requiredStorageModules;
  
  public abstract StorageTypeEnum StorageType { get; }
  public StorageModeEnum StorageMode => storageMode;

  public abstract void RegisterServices(IServiceCollection services);

  public abstract Task ConfigureServices(IServiceProvider serviceProvider);
  
  public T? StorageModuleImplementation<T>()
  {
    var storageName = typeof(T).Name;
    if (!_implementations.TryGetValue(storageName, out var implementation))
      return default;
    return (T)implementation;
  }

  protected async Task ConfigureEfSqlServiceLocal<TInterface, TImpl>(IServiceProvider serviceProvider) where TImpl : IDbContextBase
  {
    var name = typeof(TInterface).Name;
    try
    {
      var implementation = serviceProvider.GetService<TImpl>();
      if (implementation == null)
        throw new Exception($"Cannot find any implementation of {name}.");


      _implementations.Add(name, implementation);
      await implementation.Init();
    }
    catch (Exception e)
    {
      throw new Exception($"Cannot configure '{name}' storage", e);
    }
  }
}