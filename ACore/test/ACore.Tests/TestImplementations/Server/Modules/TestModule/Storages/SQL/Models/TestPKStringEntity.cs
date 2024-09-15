using System.ComponentModel.DataAnnotations;
using ACore.Extensions;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Storages.Models.PK;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Models;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

[Auditable(1)]
internal class TestPKStringEntity : PKStringEntity
{
  [MaxLength(20)]
  public string? Name { get; set; }
  
  public static TestPKStringEntity Create(TestPKStringData data)
  {
    var en = new TestPKStringEntity();
    en.CopyPropertiesFrom(data);
    return en;
  }
}