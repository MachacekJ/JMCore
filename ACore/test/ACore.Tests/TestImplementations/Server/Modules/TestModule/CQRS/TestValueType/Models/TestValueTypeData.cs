using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using Mapster;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Models;

public class TestValueTypeData //: HashData
{
  public int Id { get; set; }
  public int IntNotNull { get; set; }
  public int? IntNull { get; set; }
  public long BigIntNotNull { get; set; }
  public long? BigIntNull { get; set; }
  public bool Bit2 { get; set; }
  public string? Char2 { get; set; }
  public DateTime Date2 { get; set; }
  public DateTime DateTime2 { get; set; }
  public decimal Decimal2 { get; set; }
  public string NChar2 { get; set; } = string.Empty;
  public string NVarChar2 { get; set; } = string.Empty;
  public DateTime SmallDateTime2 { get; set; }
  public short SmallInt2 { get; set; }
  public byte TinyInt2 { get; set; }
  public Guid Guid2 { get; set; }
  public byte[] VarBinary2 { get; set; } = [];
  public string VarChar2 { get; set; } = string.Empty;

  internal static TestValueTypeData Create(TestValueTypeEntity entity)
    => entity.Adapt<TestValueTypeData>();
}