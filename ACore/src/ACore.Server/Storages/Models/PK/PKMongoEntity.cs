using MongoDB.Bson;

namespace ACore.Server.Storages.Models.PK;

public abstract class PKMongoEntity() : PKEntity<ObjectId>(ObjectId.Empty);
