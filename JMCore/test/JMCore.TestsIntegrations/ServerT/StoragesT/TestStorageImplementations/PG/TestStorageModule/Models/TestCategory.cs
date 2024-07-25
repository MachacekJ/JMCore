using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JMCore.Server.Storages.Base.Audit.Configuration;

namespace JMCore.TestsIntegrations.ServerT.StoragesT.TestStorageImplementations.PG.TestStorageModule.Models;

[Auditable]
[Table("test_category")]
public class TestCategory
{
  [Key]
  [Column("test_category_id")]
  public int Id { get; set; }
  
  [Column("name")]
  public string Name { get; set; } = null!;
  
  [Column("test_rootcategory_id")]
  public int RootCategoryId { get; set; }
 
  [Column("test_category_parent_id")]
  public int? ParentCategoryId { get; set; }
  
  [ForeignKey(nameof(RootCategoryId))]
  public TestRootCategory RootCategory { get; set; }
  
  [ForeignKey(nameof(ParentCategoryId))]
  public TestCategory? ParentCategory { get; set; }
  
  public ICollection<TestCategory>? SubCategories { get; set; }
}