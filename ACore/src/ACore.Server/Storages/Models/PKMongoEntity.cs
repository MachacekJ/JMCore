using ACore.Server.Storages.Models.PK;
using MongoDB.Bson;

namespace ACore.Server.Storages.Models;

public class PKMongoEntity() : PKEntity<ObjectId>(ObjectId.Empty);