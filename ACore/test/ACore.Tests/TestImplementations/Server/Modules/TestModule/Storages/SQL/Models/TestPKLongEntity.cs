// ReSharper disable UnusedAutoPropertyAccessor.Global
using System.ComponentModel.DataAnnotations;
using ACore.Extensions;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Storages.Models.PK;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKLong.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

[Auditable(1)]
internal class TestPKLongEntity : PKLongEntity
{
  [MaxLength(200)]
  public string Name { get; set; } = string.Empty;

  public static TestPKLongEntity Create(TestPKLongData data)
    => ToEntity<TestPKLongEntity>(data);
}

