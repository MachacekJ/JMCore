using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace JMCore.Server.DB.DbContexts.AuditStructure.Models;

//[Table("vw_Audit")]
public class Audit_VwAuditEntity
{
    [Key]
    public long Id { get; set; }

    public string TableName { get; set; } = null!;
    public long? PKValue { get; set; }
    public string? PKValueString { get; set; }
    
    public string? UserName { get; set; }
    public DateTime DateTime { get; set; }
    public EntityState EntityState { get; set; }
    public string ColumnName { get; set; } = null!;

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
}