using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace JMCore.Server.DB.DbContexts.AuditStructure.Models;

[Table("audit_table")]
public class AuditTableEntity
{
  [Key]
  [Column("audit_table_id")]
  public int Id { get; set; }

  [Column("table_name")]
  [MaxLength(255)]
  public string TableName { get; set; } = null!;

  [Column("schema_name")]
  [MaxLength(255)]
  public string? SchemaName { get; set; }
}