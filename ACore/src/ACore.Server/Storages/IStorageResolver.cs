﻿using ACore.Server.Storages.Configuration;
using ACore.Server.Storages.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Server.Storages;

public interface IStorageResolver
{
  void RegisterServices(IServiceCollection services);
  void RegisterStorage(IServiceCollection sc, StorageConfigurationBase storageModule);
  Task ConfigureStorages(IServiceProvider sp);
  T FirstReadOnlyStorage<T>(StorageTypeEnum storageType = StorageTypeEnum.AllRegistered, StorageModeEnum storageMode = StorageModeEnum.Read) where T : IStorage;
  T FirstReadWriteStorage<T>(StorageTypeEnum storageType = StorageTypeEnum.AllRegistered, StorageModeEnum storageMode = StorageModeEnum.ReadWrite) where T : IStorage;
  List<T> AllWriteStorages<T>(StorageTypeEnum storageType = StorageTypeEnum.AllRegistered, StorageModeEnum storageMode = StorageModeEnum.Write) where T : IStorage;
}