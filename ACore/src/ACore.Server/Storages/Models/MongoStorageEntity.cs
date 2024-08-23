using MongoDB.Bson;

namespace ACore.Server.Storages.Models;

public class MongoStorageEntity() : StorageEntity<ObjectId>(ObjectId.Empty)
{
}