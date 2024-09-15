using MongoDB.Bson;

namespace ACore.Server.Storages.Models.PK;

public class PKMongoEntity() : PKEntity<ObjectId>(ObjectId.Empty);