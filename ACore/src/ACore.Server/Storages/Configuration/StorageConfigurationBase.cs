// using ACore.Server.Storages.EF;
// using ACore.Server.Storages.Models;
// using Microsoft.Extensions.DependencyInjection;
//
// namespace ACore.Server.Storages.Configuration;
//
// public abstract class StorageConfigurationBase(IEnumerable<string> requiredStorageModules, StorageModeEnum storageMode = StorageModeEnum.ReadWrite)
// {
//   private readonly Dictionary<string, object> _implementations = [];
//   protected readonly IEnumerable<string> RequiredStorageModules = requiredStorageModules;
//   
//   public abstract StorageTypeEnum StorageType { get; }
//   public StorageModeEnum StorageMode => storageMode;
//
//   public abstract void RegisterServices(IServiceCollection services);
//
//   public abstract Task ConfigureServices(IServiceProvider serviceProvider);
//   
//   public T? StorageModuleImplementation<T>()
//   {
//     var storageName = typeof(T).Name;
//     if (!_implementations.TryGetValue(storageName, out var implementation))
//       return default;
//     return (T)implementation;
//   }
//
//   public async Task ConfigureEfSqlServiceLocal<TInterface, TImpl>(IServiceProvider serviceProvider) where TImpl : DbContextBase
//   {
//     var name = typeof(TInterface).Name;
//     try
//     {
//       var implementation = serviceProvider.GetService<TImpl>();
//       if (implementation == null)
//         throw new Exception($"Cannot find any implementation of {name}.");
//
//
//       _implementations.Add(name, implementation);
//       await implementation.UpdateDatabase();
//     }
//     catch (Exception e)
//     {
//       throw new Exception($"Cannot configure '{name}' storage", e);
//     }
//   }
// }