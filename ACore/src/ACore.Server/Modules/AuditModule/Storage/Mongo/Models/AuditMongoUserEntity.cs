using MongoDB.Bson.Serialization.Attributes;
// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace ACore.Server.Modules.AuditModule.Storage.Mongo.Models;

internal class AuditMongoUserEntity
{
  [BsonElement("id")]
  public string Id { get; set; }
}