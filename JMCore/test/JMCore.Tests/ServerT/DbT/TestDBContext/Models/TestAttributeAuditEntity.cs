using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JMCore.Server.DB.Audit;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace JMCore.Tests.ServerT.DbT.TestDBContext.Models;

[Auditable]
[Table("TestAttributeAudit")]
public class TestAttributeAuditEntity
{
    [Key] public int Id { get; set; }
    public string Name { get; set; } = null!;

    [NotAuditable] public string NotAuditableColumn { get; set; } = null!;
    public DateTime Created { get; set; }
}