using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace JMCore.Tests.ServerT.DbT.TestDBContext.Models;

[Table("TestManualAudit")]
public class TestManualAuditEntity
{
    [Key] public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string NotAuditableColumn { get; set; } = null!;
    public DateTime Created { get; set; }
}