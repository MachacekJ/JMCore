using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACore.Server.Modules.AuditModule.Configuration;
using MongoDB.Bson;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.Mongo.Models
{
  [Auditable(1)]
  public class TestCategory
  {
    [Key]
    [Column("_id")]
    public ObjectId Id { get; set; }
  
    [Column("name")]
    public string Name { get; set; } = null!;
  
    [Column("test_rootcategory_id")]
    public ObjectId RootCategoryId { get; set; }
 
    [Column("test_category_parent_id")]
    public ObjectId? ParentCategoryId { get; set; }
  
    [ForeignKey(nameof(RootCategoryId))]
    public TestRootCategory? RootCategory { get; set; }
  
    [ForeignKey(nameof(ParentCategoryId))]
    public TestCategory? ParentCategory { get; set; }
  
    public ICollection<TestCategory>? SubCategories { get; set; }
  }
}