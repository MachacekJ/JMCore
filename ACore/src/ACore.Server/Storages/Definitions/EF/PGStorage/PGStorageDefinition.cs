using ACore.Server.Storages.Definitions.Models;
using ACore.Server.Storages.Definitions.Models.PK;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace ACore.Server.Storages.Definitions.EF.PGStorage;

public class PGStorageDefinition : EFStorageDefinition
{
  public override StorageTypeEnum Type => StorageTypeEnum.Postgres;
  public override string DataAnnotationColumnNameKey => "Relational:ColumnName";
  public override string DataAnnotationTableNameKey => "Relational:TableName";
  public override bool IsTransactionEnabled => true;

  public override int GetNewIntId<TEntity, TPK>(DbSet<TEntity> dbSet)
    => PKIntEntity.NewId;

  public override long GetNewLongId<TEntity, TPK>(DbSet<TEntity> dbSet)
    => PKLongEntity.NewId;

  public override Guid GetNewGuidId<TEntity, TPK>()
    => PKGuidEntity.NewId;

  public override string GetNewStringId<TEntity, TPK>()
    => PKStringEntity.NewId;

  public override ObjectId GetNewObjectId<TEntity, TPK>()
    => throw new Exception($"PK {nameof(ObjectId)} is not allowed for postgres.");
}