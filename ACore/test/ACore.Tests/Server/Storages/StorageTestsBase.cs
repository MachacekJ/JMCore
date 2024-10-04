﻿using ACore.Server.Storages;
using ACore.Server.Configuration;
using ACore.Server.Storages.Services.StorageResolvers;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.Storages;

public class StorageTestsBase : ServerTestsBase
{
  protected IStorageResolver? StorageResolver;

  protected readonly Action<ACoreServerOptionBuilder> MemoryStorageConfiguration = builder =>
  {
    builder.DefaultStorage(storageOptionBuilder => storageOptionBuilder.AddMemoryDb());
    builder.ACore(a => 
      a.AddMemoryCacheModule(memoryCacheOptionsBuilder => memoryCacheOptionsBuilder.AddCacheCategories(CacheCategories.Entity))
        .AddSaltForHash("fakesalt")
      );
  };
  
  protected override async Task GetServices(IServiceProvider sp)
  {
    await base.GetServices(sp);
    StorageResolver = sp.GetService<IStorageResolver>() ?? throw new ArgumentNullException($"{nameof(IStorageResolver)} not found.");
  }
}