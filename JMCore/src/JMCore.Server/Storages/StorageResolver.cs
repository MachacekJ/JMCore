﻿using JMCore.Server.Modules.SettingModule.Storage;
using JMCore.Server.Storages.Configuration;
using JMCore.Server.Storages.Models;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Server.Storages;

public class StorageResolver : IStorageResolver
{
  private readonly List<StorageConfigurationBase> _allStorageModules = [];

  public void RegisterServices(IServiceCollection services)
  {
    services.AddMediatR((c) => { c.RegisterServicesFromAssemblyContaining(typeof(IBasicStorageModule)); });
  }

  public void RegisterStorage(IServiceCollection sc, StorageConfigurationBase storageModule)
  {
    _allStorageModules.Add(storageModule);
    storageModule.RegisterServices(sc);
  }

  public async Task ConfigureStorages(IServiceProvider sp)
  {
    foreach (var storage in _allStorageModules)
    {
      await storage.ConfigureServices(sp);
    }
  }

  public T FirstReadOnlyStorage<T>(StorageTypeEnum storageType = StorageTypeEnum.AllRegistered, StorageModeEnum storageMode = StorageModeEnum.Read) where T : IStorage => FirstReadWriteStorage<T>(storageType, storageMode);

  public T FirstReadWriteStorage<T>(StorageTypeEnum storageType = StorageTypeEnum.AllRegistered, StorageModeEnum storageMode = StorageModeEnum.ReadWrite) where T : IStorage
  {
    var storageImplementation = _allStorageModules.Where(sm => storageType.HasFlag(sm.StorageType) && sm.StorageMode.HasFlag(storageMode))
      .Select(storageModule => storageModule.StorageModuleImplementation<T>()).OfType<T>()
      .FirstOrDefault();

    if (storageImplementation != null)
      return storageImplementation;

    throw new Exception($"Storage module '{typeof(T).Name}' has no registered implementation.");
  }

  public List<T> AllWriteStorages<T>(StorageTypeEnum storageType = StorageTypeEnum.AllRegistered, StorageModeEnum storageMode = StorageModeEnum.ReadWrite) where T : IStorage
  {
    var ab = _allStorageModules.Where(sm => storageType.HasFlag(sm.StorageType) && sm.StorageMode.HasFlag(storageMode)).Select(storageModule => storageModule.StorageModuleImplementation<T>()).OfType<T>().ToList();
    if (ab.Count > 0)
      return ab;

    throw new Exception($"Storage module '{typeof(T).Name}' has no registered implementation.");
  }
}