using ACore.Server.Storages;
using Microsoft.EntityFrameworkCore;

namespace ACore.AppTest.Modules.TestModule.Storages.EF;

public interface IEFTestStorageModule : IStorage
{
  Task<TPK> Save<TEntity, TPK>(TEntity data) where TEntity : class;
  Task<TEntity?> Get<TEntity, TPK>(TPK id) where TEntity : class;
  Task Delete<TEntity, TPK>(TPK id) where TEntity : class;
  DbSet<TEntity> DbSet<TEntity>() where TEntity : class;
}