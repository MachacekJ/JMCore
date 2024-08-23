using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACore.Server.Modules.AuditModule.Configuration;

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace ACore.AppTest.Modules.TestModule.Storages.SQL.Models;

[Auditable]
[Table("test_menu")]
public class TestMenuEntity
{
  [Key]
  [Column("test_menu_id")]
  public int Id { get; set; }
  
  [Column("name")]
  public string Name { get; set; } = string.Empty;
  
  [Column("last_modify")]
  //[ConcurrencyCheck]
  public DateTime LastModify { get; set; }

  public ICollection<TestCategoryEntity> Categories { get; set; } = [];
}

