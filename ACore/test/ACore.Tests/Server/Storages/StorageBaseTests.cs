﻿using ACore.Server.Services.JMCache;
using ACore.Server.Storages;
using ACore.Modules.CacheModule;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.Storages;

/// <summary>
/// Working with memory EF.
/// </summary>
public class StorageBaseTests : ServerBaseTests
{
  protected readonly IStorageResolver StorageResolver = new StorageResolver();

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    StorageResolver.RegisterServices(sc);
    sc.AddSingleton(StorageResolver);
    sc.AddJMMemoryCache<JMCacheServerCategory>();
  }

  protected override async Task GetServicesAsync(IServiceProvider sp)
  {
    await StorageResolver.ConfigureStorages(sp);
    await base.GetServicesAsync(sp);
  }
}