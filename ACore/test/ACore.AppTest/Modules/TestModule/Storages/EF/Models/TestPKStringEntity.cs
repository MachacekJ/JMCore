using System.ComponentModel.DataAnnotations;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Storages.Models;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace ACore.AppTest.Modules.TestModule.Storages.EF.Models;

[Auditable]
internal class TestPKStringEntity : StringStorageEntity
{
  [MaxLength(20)]
  public string Name { get; set; } = string.Empty;
}