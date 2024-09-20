using System.Reflection;
using System.Text.Json;
using ACore.Server.Modules.AuditModule.CQRS.AuditGet;
using ACore.Server.Modules.AuditModule.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Get;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Save;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using FluentAssertions;
using MediatR;
using Serilog.Events;
using Serilog.Sinks.InMemory;
using Serilog.Sinks.InMemory.Assertions;
using Xunit;

namespace ACore.Tests.Server.Modules.AuditModule;

/// <summary>
/// Test for different C# types and their persistence.
/// </summary>
public class AuditValueTypesTests : AuditTestsBase
{
  [Fact]
  public async Task AllTypes()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => await AuditValuesTHelper.AllTypes(Mediator, LogInMemorySink, GetTableName, GetColumnName));
  }
}

public static class AuditValuesTHelper
{
  public static async Task AllTypes(IMediator mediator, InMemorySink logInMemorySink, Func<string, string> getTableName, Func<string, string, string> getColumnName)
  {
    var entityName = "TestValueTypeEntity";
    
    // Arrange
    var item = new TestValueTypeData
    {
      IntNotNull = int.MaxValue,
      IntNull = int.MaxValue,
      BigIntNotNull = long.MaxValue,
      BigIntNull = long.MaxValue,
      Bit2 = true,
      Char2 = "Hello",
      Date2 = DateTime.Today.ToUniversalTime(),
      DateTime2 = DateTime.UtcNow,
      Decimal2 = 12345678901.12345678M,
      NChar2 = "Čau říá",
      NVarChar2 = "říkám já",
      SmallDateTime2 = new DateTime(2000, 10, 10, 10, 10, 0, DateTimeKind.Utc),
      SmallInt2 = short.MaxValue,
      TinyInt2 = byte.MaxValue,
      Guid2 = Guid.NewGuid(),
      VarBinary2 = new byte[10000],
      VarChar2 = "říkám já řřČŘÉÍÁ"
    };

    // Act.
    await mediator.Send(new TestValueTypeSaveHashCommand(item));
    // Assert
    item.Id.Should().BeGreaterThan(0);

    logInMemorySink.Should().HaveMessage("The value exceeded the maximum character length '{MaxStringSize}'. Value:{Value}")
      .Appearing().Once().WithLevel(LogEventLevel.Error);

    var allData = (await mediator.Send(new TestValueTypeGetQuery(false))).ResultValue;
    allData.Should().HaveCount(1);

    var savedItem = allData.Single();
    var resAuditItems = (await mediator.Send(new AuditGetQuery<TestValueTypeEntity, int>(getTableName(entityName), savedItem.Id))).ResultValue;
    resAuditItems.Should().HaveCount(1);
    resAuditItems.Single().EntityState.Should().Be(AuditStateEnum.Added);

    var auditItem = resAuditItems.Single();
    // 17 fields + 1 Id
    auditItem.Columns.Should().HaveCount(18);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.Id))).NewValue.Should().Be(item.Id);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.IntNotNull))).NewValue.Should().Be(item.IntNotNull);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.IntNull))).NewValue.Should().Be(item.IntNull);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.BigIntNotNull))).NewValue.Should().Be(item.BigIntNotNull);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.BigIntNull))).NewValue.Should().Be(item.BigIntNull);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.Bit2))).NewValue.Should().Be(item.Bit2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.Char2))).NewValue.Should().Be(item.Char2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.Date2))).NewValue.Should().Be(item.Date2.Ticks);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.DateTime2))).NewValue.Should().Be(item.DateTime2.Ticks);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.Decimal2))).NewValue.Should().Be(JsonSerializer.Serialize(item.Decimal2));
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.NChar2))).NewValue.Should().Be(item.NChar2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.NVarChar2))).NewValue.Should().Be(item.NVarChar2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.SmallDateTime2))).NewValue.Should().Be(item.SmallDateTime2.Ticks);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.SmallInt2))).NewValue.Should().Be(item.SmallInt2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.TinyInt2))).NewValue.Should().Be(item.TinyInt2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.Guid2))).NewValue.Should().Be(item.Guid2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.VarBinary2))).NewValue.Should().Be(JsonSerializer.Serialize(item.VarBinary2));
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.VarChar2))).NewValue.Should().Be(item.VarChar2);
  }
}