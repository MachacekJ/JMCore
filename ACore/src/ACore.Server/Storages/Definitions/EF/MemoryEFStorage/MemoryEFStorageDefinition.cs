using ACore.Server.Storages.Definitions.Models;
using ACore.Server.Storages.Definitions.Models.PK;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace ACore.Server.Storages.Definitions.EF.MemoryEFStorage;

public class MemoryEFStorageDefinition : EFStorageDefinition
{
  public override StorageTypeEnum Type => StorageTypeEnum.Memory;
  public override string DataAnnotationColumnNameKey => string.Empty;
  public override string DataAnnotationTableNameKey => string.Empty;
  public override bool IsTransactionEnabled => false;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
  public override int GetNewIntId<TEntity, TPK>(DbSet<TEntity> dbSet)
    => !dbSet.Any() ? 1 : dbSet.Max(i => (i as PKIntEntity).Id) + 1;
  
  public override long GetNewLongId<TEntity, TPK>(DbSet<TEntity> dbSet)
    => !dbSet.Any() ? 1 : dbSet.Max(i => (i as PKLongEntity).Id) + 1;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
  
  public override Guid GetNewGuidId<TEntity, TPK>()
    => PKGuidEntity.NewId;

  public override string GetNewStringId<TEntity, TPK>()
    => PKStringEntity.NewId;

  public override ObjectId GetNewObjectId<TEntity, TPK>()
    => throw new Exception($"PK {nameof(ObjectId)} is not allowed for memory EF.");
}