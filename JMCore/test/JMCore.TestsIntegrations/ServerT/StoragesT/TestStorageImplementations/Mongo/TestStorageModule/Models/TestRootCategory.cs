using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JMCore.Server.Storages.Base.Audit.Configuration;
using MongoDB.Bson;

namespace JMCore.TestsIntegrations.ServerT.StoragesT.TestStorageImplementations.Mongo.TestStorageModule.Models
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

