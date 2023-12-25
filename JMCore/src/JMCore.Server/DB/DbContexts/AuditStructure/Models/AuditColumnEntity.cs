using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace JMCore.Server.DB.DbContexts.AuditStructure.Models;

[Table("AuditColumn")]
public class AuditColumnEntity
{
    [Key] public int Id { get; set; }
    public int AuditTableId { get; set; }

    [MaxLength(255)]
    public string ColumnName { get; set; } = null!;

    [ForeignKey("AuditTableId")]
    public AuditTableEntity AuditTable { get; set; } = null!;
}