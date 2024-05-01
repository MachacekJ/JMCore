using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace JMCore.Server.Storages.Modules.AuditModule.Models;

[Table("audit_value")]
public class AuditValueEntity
{
  [Key]
  [Column("audit_value_id")]
  public long Id { get; set; }

  [Column("audit_id")]
  public long AuditId { get; set; }
  
  [Column("audit_column_id")]
  public int AuditColumnId { get; set; }
  
  [Column("old_value_string")]
  public string? OldValueString { get; set; }
  
  [Column("new_value_string")]
  public string? NewValueString { get; set; }
  
  [Column("old_value_int")]
  public int? OldValueInt { get; set; }
  
  [Column("new_value_int")]
  public int? NewValueInt { get; set; }
  
  [Column("old_value_long")]
  public long? OldValueLong { get; set; }
  
  [Column("new_value_long")]
  public long? NewValueLong { get; set; }
  
  [Column("old_value_bool")]
  public bool? OldValueBool { get; set; }
  
  [Column("new_value_bool")]
  public bool? NewValueBool { get; set; }
  
  [Column("old_value_guid")]
  public Guid? OldValueGuid { get; set; }
  
  [Column("new_value_guid")]
  public Guid? NewValueGuid { get; set; }

  [ForeignKey("AuditId")]
  public AuditEntity Audit { get; set; } = null!;


  [ForeignKey("AuditColumnId")]
  public AuditColumnEntity AuditColumn { get; set; } = null!;
}