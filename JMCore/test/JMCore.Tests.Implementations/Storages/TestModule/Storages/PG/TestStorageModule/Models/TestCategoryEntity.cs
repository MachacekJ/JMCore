using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JMCore.Server.Modules.AuditModule.Configuration;

namespace JMCore.Tests.Implementations.Storages.TestModule.Storages.PG.TestStorageModule.Models;

[Auditable]
[Table("test_category")]
public class TestCategoryEntity
{
  [Key]
  [Column("test_category_id")]
  public int Id { get; set; }
  
  [Column("name")]
  public string Name { get; set; } = null!;
  
  [Column("test_menu_id")]
  public int MenuId { get; set; }
 
  [Column("test_category_parent_id")]
  public int? ParentCategoryId { get; set; }
  
  [ForeignKey(nameof(MenuId))]
  public TestMenuEntity MenuEntity { get; set; }
  
  [ForeignKey(nameof(ParentCategoryId))]
  public TestCategoryEntity? ParentCategory { get; set; }
  
  public ICollection<TestCategoryEntity>? SubCategories { get; set; }
}