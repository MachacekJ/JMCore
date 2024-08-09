using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace ACore.Server.Modules.AuditModule.Storage.SQL.Models;

internal class AuditValueEntity
{
  [Key]
  public long Id { get; init; }
  public long AuditId { get; init; }
  public int AuditColumnId { get; set; }
  public bool IsChanged { get; init; }
  public string? OldValueString { get; init; }
  public string? NewValueString { get; init; }
  public int? OldValueInt { get; init; }
  public int? NewValueInt { get; init; }
  public long? OldValueLong { get; init; }
  public long? NewValueLong { get; init; }
  public bool? OldValueBool { get; init; }
  public bool? NewValueBool { get; init; }
  public Guid? OldValueGuid { get; init; }
  public Guid? NewValueGuid { get; init; }

  [ForeignKey("AuditId")]
  public AuditEntity Audit { get; init; }


  [ForeignKey("AuditColumnId")]
  public AuditColumnEntity AuditColumn { get; init; }
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