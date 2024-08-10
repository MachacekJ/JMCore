using ACore.Server.Storages.Models;
using MongoDB.Bson;

namespace ACore.Server.MongoStorage;

public class MongoStorageEntity() : StorageEntity<ObjectId>(ObjectId.Empty)
{
}