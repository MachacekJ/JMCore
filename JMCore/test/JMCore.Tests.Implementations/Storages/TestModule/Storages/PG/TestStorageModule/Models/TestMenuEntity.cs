using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JMCore.Server.Modules.AuditModule.Configuration;

namespace JMCore.Tests.Implementations.Storages.TestModule.Storages.PG.TestStorageModule.Models;

[Auditable]
[Table("test_menu")]
public class TestMenuEntity
{
  [Key]
  [Column("test_menu_id")]
  public int Id { get; set; }
  
  [Column("name")]
  public string Name { get; set; } = null!;
  
  [Column("last_modify")]
  //[ConcurrencyCheck]
  public DateTime LastModify { get; set; }
  
  public ICollection<TestCategoryEntity> Categories { get; set; }
}

