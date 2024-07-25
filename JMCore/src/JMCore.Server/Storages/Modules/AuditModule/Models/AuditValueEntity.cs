using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace JMCore.Server.Storages.Modules.AuditModule.Models;

public class AuditValueEntity
{
  [Key]
  public long Id { get; set; }

  public long AuditId { get; set; }
  public int AuditColumnId { get; set; }
  public string? OldValueString { get; set; }
  public string? NewValueString { get; set; }
  public int? OldValueInt { get; set; }
  public int? NewValueInt { get; set; }
  public long? OldValueLong { get; set; }
  public long? NewValueLong { get; set; }
  public bool? OldValueBool { get; set; }
  public bool? NewValueBool { get; set; }
  public Guid? OldValueGuid { get; set; }
  public Guid? NewValueGuid { get; set; }

  [ForeignKey("AuditId")]
  public AuditEntity Audit { get; set; } = null!;


  [ForeignKey("AuditColumnId")]
  public AuditColumnEntity AuditColumn { get; set; } = null!;
}