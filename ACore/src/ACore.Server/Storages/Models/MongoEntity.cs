using ACore.Server.Storages.Models.PK;
using MongoDB.Bson;

namespace ACore.Server.Storages.Models;

public class MongoEntity() : PKEntity<ObjectId>(ObjectId.Empty)
{
}