using System.Reflection;
using System.Text.Json;
using ACore.AppTest.Modules.TestModule.CQRS.Models;
using ACore.AppTest.Modules.TestModule.CQRS.TestValueType;
using ACore.AppTest.Modules.TestModule.Storages.Models;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Modules.AuditModule.Storage.Helper;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using Serilog.Sinks.InMemory;
using Serilog.Sinks.InMemory.Assertions;
using Xunit;

namespace ACore.Tests.Server.Modules.TestModule;

/// <summary>
/// Test for different C# types and their persistence.
/// </summary>
public class AuditValuesTests : AuditAttributeBaseTests
{
  [Fact]
  public async Task AllTypes()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => await AuditValuesTHelper.AllTypes(AuditStorageModule, Mediator, LogInMemorySink, GetTableName, GetColumnName));
  }
}

public static class AuditValuesTHelper
{
  public static async Task AllTypes(IAuditStorageModule auditStorageModule, IMediator mediator, InMemorySink logInMemorySink, Func<string, string> getTableName, Func<string, string, string> getColumnName)
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
      VarBinary2 = new byte[AuditValueConverterHelper.MaxStringSize],
      VarChar2 = "říkám já řřČŘÉÍÁ"
    };
    
    var res = await mediator.Send(new TestValueTypeSaveCommand(item));
    res.Should().Be(true);

    logInMemorySink.Should().HaveMessage("The value exceeded the maximum character length '{MaxStringSize}'. Value:{Value}")
      .Appearing().Once().WithLevel(LogEventLevel.Error);

    var auditItem = (await auditStorageModule.AllAuditItemsAsync(getTableName(entityName))).Where(a => a.EntityState == EntityState.Added).ToList();

    // 17 fields + 1 Id
    auditItem.Should().HaveCount(18);
    auditItem.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.Id))).NewValueInt.Should().Be(item.Id);
    auditItem.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.IntNotNull))).NewValueInt.Should().Be(item.IntNotNull);
    auditItem.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.IntNull))).NewValueInt.Should().Be(item.IntNull);
    auditItem.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.BigIntNotNull))).NewValueLong.Should().Be(item.BigIntNotNull);
    auditItem.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.BigIntNull))).NewValueLong.Should().Be(item.BigIntNull);
    auditItem.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.Bit2))).NewValueBool.Should().Be(item.Bit2);
    auditItem.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.Char2))).NewValueString.Should().Be(item.Char2);
    auditItem.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.Date2))).NewValueLong.Should().Be(item.Date2.Ticks);
    auditItem.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.DateTime2))).NewValueLong.Should().Be(item.DateTime2.Ticks);
    auditItem.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.Decimal2))).NewValueString.Should().Be(JsonSerializer.Serialize(item.Decimal2));
    auditItem.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.NChar2))).NewValueString.Should().Be(item.NChar2);
    auditItem.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.NVarChar2))).NewValueString.Should().Be(item.NVarChar2);
    auditItem.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.SmallDateTime2))).NewValueLong.Should().Be(item.SmallDateTime2.Ticks);
    auditItem.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.SmallInt2))).NewValueInt.Should().Be(item.SmallInt2);
    auditItem.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.TinyInt2))).NewValueInt.Should().Be(item.TinyInt2);
    auditItem.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.Guid2))).NewValueGuid.Should().Be(item.Guid2);
    auditItem.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.VarBinary2))).NewValueString.Should().Be(JsonSerializer.Serialize(item.VarBinary2));
    auditItem.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeData.VarChar2))).NewValueString.Should().Be(item.VarChar2);
  }
}