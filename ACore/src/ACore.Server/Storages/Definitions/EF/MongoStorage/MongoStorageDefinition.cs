using ACore.Server.Storages.Definitions.Models;
using ACore.Server.Storages.Definitions.Models.PK;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace ACore.Server.Storages.Definitions.EF.MongoStorage;

public class MongoStorageDefinition : EFStorageDefinition
{
  private const string ErrorNotSupportedPK = $"Only PK {nameof(ObjectId)} is allowed for mongodb.";
  public override StorageTypeEnum Type => StorageTypeEnum.Mongo;
  public override string DataAnnotationColumnNameKey => "Mongo:ElementName";
  public override string DataAnnotationTableNameKey => "Mongo:CollectionName";
  public override bool IsTransactionEnabled => false;

  public override int GetNewIntId<TEntity, TPK>(DbSet<TEntity> dbSet)
    => throw new NotImplementedException(ErrorNotSupportedPK);


  public override long GetNewLongId<TEntity, TPK>(DbSet<TEntity> dbSet)
    => throw new NotImplementedException(ErrorNotSupportedPK);

  public override Guid GetNewGuidId<TEntity, TPK>()
    => throw new NotImplementedException(ErrorNotSupportedPK);

  public override string GetNewStringId<TEntity, TPK>()
    => throw new NotImplementedException(ErrorNotSupportedPK);

  public override ObjectId GetNewObjectId<TEntity, TPK>()
    => PKMongoEntity.NewId;
}