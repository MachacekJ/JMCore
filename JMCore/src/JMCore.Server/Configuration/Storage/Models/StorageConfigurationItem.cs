using JMCore.Server.Storages.Base.EF;
using JMCore.Server.Storages.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Server.Configuration.Storage.Models;

public abstract class StorageConfigurationItem(StorageNativeModuleTypeEnum registerCoreNativeModules, StorageModeEnum storageMode = StorageModeEnum.ReadWrite)
{
  protected readonly StorageNativeModuleTypeEnum RegisterCoreNativeModules = registerCoreNativeModules;
  public abstract StorageTypeEnum StorageType { get; }
  public abstract void RegisterServices(IServiceCollection services);
  public abstract Task ConfigureServices(IServiceProvider serviceProvider);


  private readonly Dictionary<string, object> _implementations = [];

  public StorageModeEnum StorageMode => storageMode;

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

  public T? StorageModuleImplementation<T>()
  {
    var storageName = typeof(T).Name;
    if (!_implementations.TryGetValue(storageName, out var implementation))
      return default;
    return (T)implementation;
  }
}