using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACore.Server.Modules.AuditModule.Configuration;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

[Auditable(1)]
[Table("test_category")]
public class TestCategoryEntity
{
  [Key]
  [Column("test_category_id")]
  public int Id { get; set; }
  
  [Column("name")]
  [MaxLength(200)]
  public string Name { get; set; } = string.Empty;
  
  [Column("test_menu_id")]
  public int MenuId { get; set; }
 
  [Column("test_category_parent_id")]
  public int? ParentCategoryId { get; set; }

  [ForeignKey(nameof(MenuId))]
  public TestMenuEntity? MenuEntity { get; set; }
  
  [ForeignKey(nameof(ParentCategoryId))]
  public TestCategoryEntity? ParentCategory { get; set; }

  public ICollection<TestCategoryEntity>? SubCategories { get; set; }
}