using System.ComponentModel.DataAnnotations;

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace ACore.Server.Modules.AuditModule.Storage.SQL.Models;

internal class AuditUserEntity
{
  [Key]
  public int Id { get; set; }
  [MaxLength(450)]
  public string UserId { get; set; } = null!;
  
  [MaxLength(255)]
  public string? UserName { get; set; }
}