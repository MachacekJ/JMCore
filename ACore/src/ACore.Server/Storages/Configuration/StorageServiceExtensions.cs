using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Server.Storages.Configuration;

public static class StorageServiceExtensions
{
  public static void AddDbMongoStorage<T>(this IServiceCollection services, StorageOptions storageOptions)
    where T : DbContext
  {
    if (storageOptions.MongoDb != null)
      services.AddDbContext<T>(opt => opt.UseMongoDB(storageOptions.MongoDb.ReadWriteConnectionString, storageOptions.MongoDb.CollectionName));
  }

  public static void AddDbPGStorage<T>(this IServiceCollection services, StorageOptions storageOptions)
    where T : DbContext
  {
    if (storageOptions.PGDb != null)
      services.AddDbContext<T>(opt => opt.UseNpgsql(storageOptions.PGDb.ReadWriteConnectionString));
  }

  public static void AddDbMemoryStorage<T>(this IServiceCollection services, StorageOptions storageOptions, string name)
    where T : DbContext
  {
    if (storageOptions is { UseMemoryStorage : true })
      services.AddDbContext<T>(dbContextOptionsBuilder => dbContextOptionsBuilder.UseInMemoryDatabase(StorageConst.MemoryConnectionString + name + Guid.NewGuid()));
  }

  public static async Task ConfigureMongoStorage<TIStorage, TImplementation>(this IServiceProvider provider, StorageOptions storageOptions)
    where TImplementation : DbContext
    where TIStorage : IStorage
  {
    var storageResolver = GetStorageResolver(provider);
    if (storageOptions.MongoDb != null)
    {
      var mongoImpl = provider.GetService<TImplementation>() as IStorage ?? throw new ArgumentNullException($"Missing implementation of {typeof(TImplementation).Name}.");
      await storageResolver.ConfigureStorage<TIStorage>(new StorageImplementation(mongoImpl));
    }
  }

  public static async Task ConfigurePGStorage<TIStorage, TImplementation>(this IServiceProvider provider, StorageOptions storageOptions)
    where TImplementation : DbContext
    where TIStorage : IStorage
  {
    var storageResolver = GetStorageResolver(provider);
    if (storageOptions.PGDb != null)
    {
      var pgImpl = provider.GetService<TImplementation>() as IStorage ?? throw new ArgumentNullException($"Missing implementation of {typeof(TImplementation).Name}.");
      await storageResolver.ConfigureStorage<TIStorage>(new StorageImplementation(pgImpl));
    }
  }

  public static async Task ConfigureMemoryStorage<TIStorage, TImplementation>(this IServiceProvider provider, StorageOptions storageOptions)
    where TImplementation : DbContext
    where TIStorage : IStorage
  {
    var storageResolver = GetStorageResolver(provider);
    if (storageOptions.UseMemoryStorage)
    {
      var pgImpl = provider.GetService<TImplementation>() as IStorage ?? throw new ArgumentNullException($"Missing implementation of {typeof(TImplementation).Name}.");
      await storageResolver.ConfigureStorage<TIStorage>(new StorageImplementation(pgImpl));
    }
  }

  private static IStorageResolver GetStorageResolver(IServiceProvider provider)
    => provider.GetService<IStorageResolver>() ?? throw new ArgumentNullException($"Missing implementation of {nameof(IStorageResolver)}.");
}