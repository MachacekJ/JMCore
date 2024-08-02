using ACore.AppTest.Modules.TestModule.Storages.Models;
using ACore.Extensions;

namespace ACore.AppTest.Modules.TestModule.CQRS.Models;

public class TestValueTypeData
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
  public string NChar2 { get; set; } = null!;
  public string NVarChar2 { get; set; } = null!;
  public DateTime SmallDateTime2 { get; set; }
  public short SmallInt2 { get; set; }
  public byte TinyInt2 { get; set; }
  public Guid Guid2 { get; set; }
  public byte[] VarBinary2 { get; set; } = null!;
  public string VarChar2 { get; set; } = null!;
  
  internal static TestValueTypeData Create(TestValueTypeEntity entity)
  {
    var testPKGuidData = new TestValueTypeData();
    testPKGuidData.CopyPropertiesFrom(entity);
    return testPKGuidData;
  }
}