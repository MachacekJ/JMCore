using Microsoft.EntityFrameworkCore;

namespace JMCore.Server.MongoStorage.AuditModule.Models;

public class AuditMongoEntity
{
  public string Id { get; set; } = Guid.NewGuid().ToString();
  public string ObjectId { get; set; }
  public List<AuditMongoValues>? OldValues { get; set; }
  public List<AuditMongoValues>? NewValues { get; set; }
  public DateTime DateTime { get; set; }
  public EntityState EntityState { get; set; }
  public AuditMongoUser User { get; set; }
}

public class AuditMongoValues
{
  public string Column { get; set; }
  public string Value { get; set; }
}

public class AuditMongoUser
{
  public string  UserId { get; set; }
  public string UserName { get; set; }
}