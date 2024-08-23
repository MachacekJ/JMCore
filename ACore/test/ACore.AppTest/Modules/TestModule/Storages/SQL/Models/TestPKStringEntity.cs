using System.ComponentModel.DataAnnotations;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Storages.Models;
using ACore.Server.Storages.Models.PK;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace ACore.AppTest.Modules.TestModule.Storages.SQL.Models;

[Auditable]
internal class TestPKPKStringEntity : PKStringEntity
{
  [MaxLength(20)]
  public string Name { get; set; } = string.Empty;
}