using System.Text;
using ACore.Extensions;
using ACore.UnitTests.Core.Extensions.ObjectExtensions.FakeData;
using FluentAssertions;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace ACore.UnitTests.Core.Extensions.ObjectExtensions;

public class CompareTests
{
  private static DateTime _testDate = new DateTime(2020, 12, 23);
  private static Guid _testGuid = Guid.NewGuid();
  private static byte[] _testByteArray = Encoding.UTF8.GetBytes("Hello World");
  private static ObjectId _testObjectId = ObjectId.GenerateNewId();

  [Theory]
  [MemberData(nameof(Data))]
  public void BaseTest(Fake1Class oldObject, Fake1Class? newObject, ComparisonResult[] expectedResults)
  {
    // Act
    var res = oldObject.Compare(newObject);

    // Assert
    res.Should().HaveCount(expectedResults.Length);
    for (int i = 0; i < res.Length; i++)
    {
      CompareResult(res[i], expectedResults[i]);
    }
  }

  private void CompareResult(ComparisonResult result, ComparisonResult expectedResult)
  {
    result.Name.Should().Be(expectedResult.Name);
    result.Type.Should().Be(expectedResult.Type);
    result.IsChange.Should().Be(expectedResult.IsChange);

    if (result.LeftValue == null)
      expectedResult.LeftValue.Should().BeNull();

    var lrv = JsonConvert.SerializeObject(result.LeftValue);
    var lev = JsonConvert.SerializeObject(expectedResult.LeftValue);
    lrv.Should().Be(lev);

    if (result.RightValue == null)
      expectedResult.RightValue.Should().BeNull();

    var rrv = JsonConvert.SerializeObject(result.RightValue);
    var rev = JsonConvert.SerializeObject(expectedResult.RightValue);
    rrv.Should().Be(rev);
  }

  public static IEnumerable<object?[]> Data =>
    new List<object?[]>
    {
      new object?[]
      {
        new Fake1Class(), null, BaseComparisionWithEx([], true)
      },
      new object?[]
      {
        new Fake1Class(), new Fake1Class(), BaseComparisionWithEx([], false),
      },
      new object?[]
      {
        new Fake1Class { Int = 1 }, new Fake1Class(), BaseComparisionWithEx([
          new ComparisonResult(nameof(Fake1Class.Int), typeof(int?), true, 1, null)
        ], false),
      },
      new object?[]
      {
        new Fake1Class(), new Fake1Class { Int = 1 }, BaseComparisionWithEx([
          new ComparisonResult(nameof(Fake1Class.Int), typeof(int?), true, null, 1)
        ], false),
      },
      new object?[]
      {
        new Fake1Class { Int = 1 }, new Fake1Class { Int = 1 }, BaseComparisionWithEx([
          new ComparisonResult(nameof(Fake1Class.Int), typeof(int?), false, 1, 1)
        ], false),
      },
      new object?[]
      {
        new Fake1Class { DateTime = _testDate }, new Fake1Class { DateTime = _testDate }, BaseComparisionWithEx([
          new ComparisonResult(nameof(Fake1Class.DateTime), typeof(DateTime?), false, _testDate, _testDate)
        ], false),
      },
      new object?[]
      {
        new Fake1Class { Guid = _testGuid }, new Fake1Class { Guid = _testGuid }, BaseComparisionWithEx([
          new ComparisonResult(nameof(Fake1Class.Guid), typeof(Guid?), false, _testGuid, _testGuid)
        ], false),
      },
      new object?[]
      {
        new Fake1Class { ByteArr = _testByteArray }, new Fake1Class { ByteArr = new List<byte>(_testByteArray).ToArray() }, BaseComparisionWithEx([
          new ComparisonResult(nameof(Fake1Class.ByteArr), typeof(byte[]), false, new List<byte>(_testByteArray).ToArray(), new List<byte>(_testByteArray).ToArray())
        ], false),
      },
      new object?[]
      {
        new Fake1Class { String = "fake" }, new Fake1Class { String = "fake" }, BaseComparisionWithEx([
          new ComparisonResult(nameof(Fake1Class.String), typeof(string), false, "fake", "fake")
        ], false),
      },
      new object?[]
      {
        new Fake1Class { MongoId = _testObjectId }, new Fake1Class { MongoId = new ObjectId(_testObjectId.ToByteArray()) }, BaseComparisionWithEx([
          new ComparisonResult(nameof(Fake1Class.MongoId), typeof(ObjectId?), false, new ObjectId(_testObjectId.ToByteArray()), new ObjectId(_testObjectId.ToByteArray()))
        ], false),
      },
      new object?[]
      {
        new Fake1Class { MongoId = _testObjectId }, new Fake1Class { DateTime = _testDate }, BaseComparisionWithEx([
          new ComparisonResult(nameof(Fake1Class.MongoId), typeof(ObjectId?), true, new ObjectId(_testObjectId.ToByteArray()), null),
          new ComparisonResult(nameof(Fake1Class.DateTime), typeof(DateTime?), true, null, _testDate)
        ], false),
      },
    };

  private static List<ComparisonResult> BaseComparisionWithEx(List<ComparisonResult> results, bool isChange)
  {
    var baseR = new List<ComparisonResult>();

    var single = results.SingleOrDefault(e => e.Name == nameof(Fake1Class.Int));
    baseR.Add(single ?? new ComparisonResult(nameof(Fake1Class.Int), typeof(int?), isChange, null, null));

    single = results.SingleOrDefault(e => e.Name == nameof(Fake1Class.DateTime));
    baseR.Add(single ?? new ComparisonResult(nameof(Fake1Class.DateTime), typeof(DateTime?), isChange, null, null));

    single = results.SingleOrDefault(e => e.Name == nameof(Fake1Class.Guid));
    baseR.Add(single ?? new ComparisonResult(nameof(Fake1Class.Guid), typeof(Guid?), isChange, null, null));

    single = results.SingleOrDefault(e => e.Name == nameof(Fake1Class.ByteArr));
    baseR.Add(single ?? new ComparisonResult(nameof(Fake1Class.ByteArr), typeof(byte[]), isChange, null, null));

    single = results.SingleOrDefault(e => e.Name == nameof(Fake1Class.String));
    baseR.Add(single ?? new(nameof(Fake1Class.String), typeof(string), isChange, null, null));

    single = results.SingleOrDefault(e => e.Name == nameof(Fake1Class.MongoId));
    baseR.Add(single ?? new(nameof(Fake1Class.MongoId), typeof(ObjectId?), isChange, null, null));

    return baseR;
  }
}