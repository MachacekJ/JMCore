using ACore.Server.Storages;
using Microsoft.EntityFrameworkCore;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages;

public interface ITestStorageModule : IStorage
{
  Task Save<TEntity, TPK>(TEntity data, string? hashToCheck = null)
    where TEntity : class;
  
  Task Delete<TEntity, TPK>(TPK id)
    where TEntity : class;

  DbSet<TEntity> DbSet<TEntity>() where TEntity : class;
}