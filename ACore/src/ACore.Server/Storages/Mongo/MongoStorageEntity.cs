using ACore.Server.Storages.Models;
using MongoDB.Bson;

namespace ACore.Server.Storages.Mongo;

public class MongoStorageEntity() : StorageEntity<ObjectId>(ObjectId.Empty)
{
}