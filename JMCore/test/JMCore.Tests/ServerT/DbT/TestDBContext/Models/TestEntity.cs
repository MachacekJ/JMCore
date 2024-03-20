using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace JMCore.Tests.ServerT.DbT.TestDBContext.Models;

[Table("test")]
public class TestEntity
{
  [Key]
  [Column("test_id")]
  public int Id { get; set; }
  
  [Column("name")]
  public string Name { get; set; } = null!;
  
  [Column("created")]
  public DateTime Created { get; set; }
}