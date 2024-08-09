using MongoDB.Bson;

namespace ACore.UnitTests.Core.Extensions.ObjectExtensions.FakeData;

public class Fake1Class
{
  public int? Int { get; set; }
  public DateTime? DateTime { get; set; }
  public Guid? Guid { get; set; }
  public byte[]? ByteArr { get; set; }
  public string? String { get; set; }
  public ObjectId? MongoId { get; set; }
}