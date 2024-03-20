using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JMCore.Server.DB.Audit;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once PropertyCanBeMadeInitOnly.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace JMCore.Tests.ServerT.DbT.TestDBContext.Models;

[Auditable]
[Table("test_value_type")]
public class TestValueTypeEntity
{
  [Key]
  [Column("test_value_type_id")]
  public int Id { get; set; }

  [Column("int_not_null")]
  public int IntNotNull { get; set; }

  [Column("int_null")]
  public int? IntNull { get; set; }

  [Column("big_int_not_null")]
  public long BigIntNotNull { get; set; }

  [Column("big_int_null")]
  public long? BigIntNull { get; set; }

  [Column("bit2")]
  public bool Bit2 { get; set; }

  [Column("char2")]
  [MaxLength(10)]
  public string? Char2 { get; set; }

  [Column("date2")]
  public DateTime Date2 { get; set; }

  [Column("datetime2")]
  public DateTime DateTime2 { get; set; }

  [Column("decimal2")]
  [Precision(19, 8)]
  public decimal Decimal2 { get; set; }

  [Column("nchar2")]
  [MaxLength(10)]
  public string NChar2 { get; set; } = null!;

  [Column("nvarchar2")]
  [MaxLength(10)]
  public string NVarChar2 { get; set; } = null!;

  [Column("smalldatetime2")]
  public DateTime SmallDateTime2 { get; set; }

  [Column("smallint2")]
  public short SmallInt2 { get; set; }

  [Column("tinyint2")]
  public byte TinyInt2 { get; set; }

  [Column("guid2")]
  public Guid Guid2 { get; set; }

  [Column("varbinary2")]
  public byte[] VarBinary2 { get; set; } = null!;

  [Column("varchar2")]
  [MaxLength(100)]
  public string VarChar2 { get; set; } = null!;
}