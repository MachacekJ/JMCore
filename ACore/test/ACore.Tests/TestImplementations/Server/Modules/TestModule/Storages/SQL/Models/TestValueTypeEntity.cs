using System.ComponentModel.DataAnnotations;
using ACore.Extensions;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Storages.Definitions.Models.PK;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Models;
using Microsoft.EntityFrameworkCore;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

[Auditable(1)]
internal class TestValueTypeEntity : PKIntEntity
{
  public int IntNotNull { get; set; }
  public int? IntNull { get; set; }
  public long BigIntNotNull { get; set; }
  public long? BigIntNull { get; set; }
  public bool Bit2 { get; set; }

  [MaxLength(10)]
  public string? Char2 { get; set; }

  public DateTime Date2 { get; set; }
  public DateTime DateTime2 { get; set; }

  [Precision(19, 8)]
  public decimal Decimal2 { get; set; }

  [MaxLength(10)]
  public string NChar2 { get; set; } = string.Empty;

  [MaxLength(10)]
  public string NVarChar2 { get; set; } = string.Empty;

  public DateTime SmallDateTime2 { get; set; }
  public short SmallInt2 { get; set; }
  public byte TinyInt2 { get; set; }
  public Guid Guid2 { get; set; }
  public byte[] VarBinary2 { get; set; } = [];

  [MaxLength(100)]
  public string VarChar2 { get; set; } = string.Empty;

  public static TestValueTypeEntity Create(TestValueTypeData data)
    => ToEntity<TestValueTypeEntity>(data);
}