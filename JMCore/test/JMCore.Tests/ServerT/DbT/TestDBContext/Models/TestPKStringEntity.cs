using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JMCore.Server.DB.Audit;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace JMCore.Tests.ServerT.DbT.TestDBContext.Models;

[Auditable]
[Table("TestPKString")]
public class TestPKStringEntity
{
    [Key] public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
}