using System.ComponentModel.DataAnnotations;
using JMCore.Server.Storages.Base.Audit.Configuration;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once PropertyCanBeMadeInitOnly.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace JMCore.Tests.ServerT.StoragesT.Implementations.TestStorageModule.Models;

[Auditable]
public class TestValueTypeEntity
{
  [Key]
  public int Id { get; set; }
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
  public string NChar2 { get; set; } = null!;
  [MaxLength(10)]
  public string NVarChar2 { get; set; } = null!;
  public DateTime SmallDateTime2 { get; set; }
  public short SmallInt2 { get; set; }
  public byte TinyInt2 { get; set; }
  public Guid Guid2 { get; set; }
  public byte[] VarBinary2 { get; set; } = null!;
  [MaxLength(100)]
  public string VarChar2 { get; set; } = null!;
}