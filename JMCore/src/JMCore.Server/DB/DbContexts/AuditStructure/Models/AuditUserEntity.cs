using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace JMCore.Server.DB.DbContexts.AuditStructure.Models;

[Table("AuditUser")]
public class AuditUserEntity
{
    [Key] public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public string? UserName { get; set; }
}