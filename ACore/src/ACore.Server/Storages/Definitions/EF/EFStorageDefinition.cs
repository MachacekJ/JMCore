using ACore.Server.Storages.Definitions.Models.PK;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace ACore.Server.Storages.Definitions.EF;

public abstract class EFStorageDefinition : StorageDefinition
{
  public abstract string DataAnnotationColumnNameKey { get; }
  public abstract string DataAnnotationTableNameKey { get; }
  public abstract bool IsTransactionEnabled { get; }

  public abstract int GetNewIntId<TEntity, TPK>(DbSet<TEntity> dbSet)
    where TEntity : PKEntity<TPK>;

  public abstract long GetNewLongId<TEntity, TPK>(DbSet<TEntity> dbSet)
    where TEntity : PKEntity<TPK>;

  public abstract Guid GetNewGuidId<TEntity, TPK>()
    where TEntity : PKEntity<TPK>;

  public abstract string GetNewStringId<TEntity, TPK>()
    where TEntity : PKEntity<TPK>;

  public abstract ObjectId GetNewObjectId<TEntity, TPK>()
    where TEntity : PKEntity<TPK>;


  public TPK NewId<TEntity, TPK>(DbSet<TEntity> dbSet)
    where TEntity : PKEntity<TPK>
  {
    return typeof(TPK) switch
    {
      { } entityType when entityType == typeof(int) => (TPK)Convert.ChangeType(GetNewIntId<TEntity, TPK>(dbSet), typeof(TPK)),
      { } entityType when entityType == typeof(long) => (TPK)Convert.ChangeType(GetNewLongId<TEntity, TPK>(dbSet), typeof(TPK)),
      { } entityType when entityType == typeof(string) => (TPK)Convert.ChangeType(GetNewStringId<TEntity, TPK>(), typeof(TPK)),
      { } entityType when entityType == typeof(Guid) => (TPK)Convert.ChangeType(GetNewGuidId<TEntity, TPK>(), typeof(TPK)),
      { } entityType when entityType == typeof(ObjectId) => (TPK)Convert.ChangeType(GetNewObjectId<TEntity, TPK>(), typeof(TPK)),
      _ => throw new Exception("Unknown primary data type {}")
    };
  }

  public bool IsNew<TPK>(TPK id)
  {
    if (id == null)
      ArgumentNullException.ThrowIfNull(id);
    
    return typeof(TPK) switch
    {
      { } entityType when entityType == typeof(int) => (int)Convert.ChangeType(id, typeof(int)) == PKIntEntity.EmptyId,
      { } entityType when entityType == typeof(long) => (long)Convert.ChangeType(id, typeof(long)) == PKLongEntity.EmptyId,
      { } entityType when entityType == typeof(string) => (string)Convert.ChangeType(id, typeof(string)) == PKStringEntity.EmptyId,
      { } entityType when entityType == typeof(Guid) => (Guid)Convert.ChangeType(id, typeof(Guid)) == PKGuidEntity.EmptyId,
      { } entityType when entityType == typeof(ObjectId) => (ObjectId)Convert.ChangeType(id, typeof(ObjectId)) == PKMongoEntity.EmptyId,
      _ => throw new Exception("Unknown primary data type {}")
    };
  }
}