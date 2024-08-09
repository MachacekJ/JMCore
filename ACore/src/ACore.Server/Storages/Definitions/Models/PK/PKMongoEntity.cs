using MongoDB.Bson;

namespace ACore.Server.Storages.Definitions.Models.PK;

public abstract class PKMongoEntity() : PKEntity<ObjectId>(EmptyId)
{
  public static ObjectId NewId => ObjectId.GenerateNewId();
  public static ObjectId EmptyId => ObjectId.Empty;
}
