using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JMCore.Server.Modules.AuditModule.Configuration;
using MongoDB.Bson;

namespace JMCore.Tests.Implementations.Storages.TestModule.Storages.Mongo.Models
{
  [Auditable]
  [Table("test_rootcategory")]
  public class TestRootCategory
  {
    [Key]
    [Column("_id")]
    public ObjectId Id { get; set; }
  
    [Column("name")]
    public string Name { get; set; } = null!;
  
    [Column("last_modify")]
    //[ConcurrencyCheck]
    public DateTime LastModify { get; set; }
  
    public ICollection<TestCategory> SubCategories { get; set; }
  }
}

