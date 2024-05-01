using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace JMCore.Server.MSSQLStorage.AuditStructure.Models;

[Table("AuditTable")]
public class AuditTableEntity
{
    [Key] public int Id { get; set; }
    public string TableName { get; set; } = null!;
    public string? SchemaName { get; set; }
}