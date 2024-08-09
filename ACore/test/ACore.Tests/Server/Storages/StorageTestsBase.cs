using ACore.Server.Storages;
using ACore.Server.Configuration;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Server.Configuration;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.Storages;

public class StorageTestsBase : ServerTestsBase
{
  protected IStorageResolver? StorageResolver;

  protected readonly Action<ACoreServerOptionBuilder, string, string> MongoStorageConfiguration = (builder, connectionString, collectionName) =>
  {
    builder.DefaultStorage(storageOptionBuilder => storageOptionBuilder.AddMongo(connectionString, collectionName)); 
    builder.ACore(a =>
      a.AddMemoryCacheModule(memoryCacheOptionsBuilder => memoryCacheOptionsBuilder.AddCacheCategories(CacheCategories.Entity))
        .AddSaltForHash("fakesalt")
    );
  };

  protected readonly Action<ACoreServerOptionBuilder, string> PGStorageConfiguration = (builder, connectionString) =>
  {
    builder.DefaultStorage(storageOptionBuilder => storageOptionBuilder.AddPG(connectionString));
    builder.ACore(a =>
      a.AddMemoryCacheModule(memoryCacheOptionsBuilder => memoryCacheOptionsBuilder.AddCacheCategories(CacheCategories.Entity))
        .AddSaltForHash("fakesalt")
    );
  };

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

  protected override void SetContainer(ContainerBuilder containerBuilder)
  {
    base.SetContainer(containerBuilder);
    containerBuilder.AddACoreTest();
  }
}