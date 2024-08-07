using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.Storages.EF;

internal interface IEFTestStorageModule : IStorage
{
  Task<TPK> Save<TEntity, TPK>(TEntity data) where TEntity : class;
  Task<TEntity?> Get<TEntity, TPK>(TPK id) where TEntity : class;
  Task Delete<TEntity, TPK>(TPK id) where TEntity : class;
  Task<T[]> All<T>() where T : class;
}