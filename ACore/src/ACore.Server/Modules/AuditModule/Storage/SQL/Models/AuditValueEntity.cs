using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace ACore.Server.Modules.AuditModule.Storage.SQL.Models;

internal class AuditValueEntity
{
  [Key]
  public long Id { get; set; }

  public bool IsChanged { get; set; }
  public string DataType { get; set; } = string.Empty;
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
  public AuditEntity Audit { get; set; }


  [ForeignKey("AuditColumnId")]
  public AuditColumnEntity AuditColumn { get; set; }
}

internal static class AuditValueEntityExtensions
{
  public static object? GetNewValueObject(this AuditValueEntity auditSqlValueItem)
  {
    if (auditSqlValueItem.NewValueInt != null)
      return auditSqlValueItem.NewValueInt;
    if (auditSqlValueItem.NewValueLong != null)
      return auditSqlValueItem.NewValueLong;
    if (auditSqlValueItem.NewValueString != null)
      return auditSqlValueItem.NewValueString;
    if (auditSqlValueItem.NewValueBool != null)
      return auditSqlValueItem.NewValueBool;
    return auditSqlValueItem.NewValueGuid ?? null;
  }

  public static object? GetOldValueObject(this AuditValueEntity auditSqlValueItem)
  {
    if (auditSqlValueItem.OldValueInt != null)
      return auditSqlValueItem.OldValueInt;
    if (auditSqlValueItem.OldValueLong != null)
      return auditSqlValueItem.OldValueLong;
    if (auditSqlValueItem.OldValueString != null)
      return auditSqlValueItem.OldValueString;
    if (auditSqlValueItem.OldValueBool != null)
      return auditSqlValueItem.OldValueBool;
    return auditSqlValueItem.OldValueGuid ?? null;
  }
}